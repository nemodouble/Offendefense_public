using System;
using System.Collections;
using System.Linq;
using com.Nemodouble.Offendefense.Scripts.Networks;
using com.Nemodouble.Offendefense.Scripts.Skills;
using com.Nemodouble.Offendefense.Scripts.UIs;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace com.Nemodouble.Offendefense.Scripts
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private DisplayManager displayManager;
        
        private bool everyoneReady = false;
        private Player[] _players;
        
        private const float FIRST_TURN_TIMER = 8f;
        public const float TIMER_MAX = 6f;
        public const float BLUFF_LOCK_TIME = 2f;
        public const float BATTLE_RESULT_DISPLAY_TIME = 2f;
        
        private float _timer = FIRST_TURN_TIMER;
        private bool isGameEnd = false;
        private GameResult _isGameWin;

        public enum GameResult
        {
            Win,
            Lose,
            Draw
        }

        #region Unity Callbacks

        private void Awake()
        {
            _players = PhotonNetwork.PlayerList;
            PlayerPropertiesManager.Instance.InitPlayerProperties();
            PlayerPropertiesManager.Instance.SetPLAYER_LOADED_LEVEL(true);
            displayManager.Init();
        }

        private void Update()
        {
            if (isGameEnd) return;
            if (!everyoneReady)
            {
                everyoneReady = true;
                try
                {
                    if (_players.Any(player => !(bool)player.CustomProperties[Offendefense.PLAYER_LOADED_LEVEL]))
                    {
                        everyoneReady = false;
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            if(PhotonNetwork.IsMasterClient)
            {
                if(_timer > 0f)
                {
                    photonView.RPC("UpdateTimer", RpcTarget.All, Time.deltaTime);
                }
            }
        }

        #endregion
        
        #region Photon Callbacks

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Main");
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if(Equals(targetPlayer, PhotonNetwork.LocalPlayer))
            {
                if (changedProps.ContainsKey(Offendefense.PLAYER_HEALTH))
                {
                    var nowHealth = PlayerPropertiesManager.Instance.GetMyHealth();
                    if (nowHealth > 0) return;
                    photonView.RPC("EndGameRPC", RpcTarget.All);
                }
            }
            else
            {
                if (changedProps.ContainsKey(Offendefense.PLAYER_HEALTH))
                {
                    var oppHealth = PlayerPropertiesManager.Instance.GetMyHealth();
                    if (oppHealth > 0) return;
                    photonView.RPC("EndGameRPC", RpcTarget.All);
                }
            }
        }

        #endregion
        
        #region UI Callbacks
        
        public void OnClickBackToRoom()
        {
            // player not ready
            var hash = new Hashtable { { Offendefense.PLAYER_READY, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            
            SceneManager.LoadScene("Main");
        }

        #endregion
        
        public float GetTimer()
        {
            return _timer;
        }
        
        /// <summary>
        /// Timer update
        /// </summary>
        /// <param name="deltaTime"></param>
        [PunRPC]
        public void UpdateTimer(float deltaTime)
        {
            _timer -= deltaTime;
            displayManager.SetTimer(_timer);
            if (_timer <= 0f)
            {
                _timer = 0f;
                displayManager.SetTimer(_timer);
                var hash = new Hashtable { { Offendefense.PLAYER_UPDATING_SKILL_RESULT, true } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                CalSkillResultByMaster();
            }
        }
        
        private void CalSkillResultByMaster()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            var mySkillType = (SkillType)PhotonNetwork.LocalPlayer.CustomProperties[Offendefense.PLAYER_SELECT_SKILL];
            var mySkill = SkillFactory.GetSkillBySkillType(mySkillType);
            var oppSkillType = (SkillType)PhotonNetwork.PlayerListOthers[0].CustomProperties[Offendefense.PLAYER_SELECT_SKILL];
            var oppSkill = SkillFactory.GetSkillBySkillType(oppSkillType);
            
            mySkill.SetIsMySkill(true);
            mySkill.SetOppSkill(oppSkill);
            oppSkill.SetIsMySkill(false);
            oppSkill.SetOppSkill(mySkill);
            
            mySkill.UseStart();
            oppSkill.UseStart();
            mySkill.BeforeDefense();
            oppSkill.BeforeDefense();
            mySkill.Defense();
            oppSkill.Defense();
            mySkill.AfterDefense();
            oppSkill.AfterDefense();
            mySkill.BeforeAttack();
            oppSkill.BeforeAttack();
            mySkill.Attack();
            oppSkill.Attack();
            mySkill.AfterAttack();
            oppSkill.AfterAttack();
            mySkill.BeforeCharge();
            oppSkill.BeforeCharge();
            mySkill.Charge();
            oppSkill.Charge();
            mySkill.AfterCharge();
            oppSkill.AfterCharge();
            mySkill.UseEnd();
            oppSkill.UseEnd();
            
            PlayerPropertiesManager.Instance.SetMySkillResult(mySkill.SkillResult);
            PlayerPropertiesManager.Instance.SetOppSkillResult(oppSkill.SkillResult);
            
            photonView.RPC("AddLog", RpcTarget.All, mySkill.SkillResult, oppSkill.SkillResult);
            photonView.RPC("CalEnd", RpcTarget.All);
        }

        [PunRPC]
        private void AddLog(SkillResult mySkillResult, SkillResult oppSkillResult)
        {
            BattleLogger.Instance.AddLog(mySkillResult, oppSkillResult);
        }

        /// <summary>
        /// Target All
        /// </summary>
        [PunRPC]
        public void CalEnd()
        {
            displayManager.UpdateBattle(BATTLE_RESULT_DISPLAY_TIME);
            var hash = new Hashtable { { Offendefense.PLAYER_UPDATING_SKILL_RESULT, false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            StartCoroutine(WaitUpdateBattleDisplay(BATTLE_RESULT_DISPLAY_TIME));
        }

        private IEnumerator WaitUpdateBattleDisplay(float time)
        {
            // 모든 플레이어 이전 전투 결과 표시 업데이트 확인
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if ((bool)player.CustomProperties[Offendefense.PLAYER_UPDATING_SKILL_RESULT])
                    yield return null;
            }
            var skillNone = SkillFactory.GetSkillBySkillType(SkillType.None);
            PlayerPropertiesManager.Instance.SetMySkill(skillNone);
            yield return new WaitForSeconds(time);
            _timer = TIMER_MAX;
        }
        
        [PunRPC]
        public void EndGameRPC()
        {
            var myHealth = PlayerPropertiesManager.Instance.GetMyHealth();
            var oppHealth = PlayerPropertiesManager.Instance.GetOppHealth();
            if (myHealth > oppHealth) 
                _isGameWin = GameResult.Win;
            else if (myHealth < oppHealth) 
                _isGameWin = GameResult.Lose;
            else 
                _isGameWin = GameResult.Draw;
            displayManager.EndGame(_isGameWin);
            isGameEnd = true;
        }
    }
}