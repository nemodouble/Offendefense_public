using com.Nemodouble.Offendefense.Scripts.Skills;
using com.Nemodouble.Offendefense.Scripts.UIs;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts.Networks
{
    public class PlayerPropertiesManager : MonoBehaviourPunCallbacks
    {
        // Singleton
        private static PlayerPropertiesManager _instance;
        public static PlayerPropertiesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PlayerPropertiesManager>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        [SerializeField] private DisplayManager displayManager;

        private int _health = 100;
        private int _gage = 0;
        private int _oppHealth = 100;
        private int _oppGage = 0;
        
        private int _myPlayerId;
        private Skill mySkill;
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (Equals(targetPlayer, PhotonNetwork.LocalPlayer))
            {
                if (changedProps.ContainsKey(Offendefense.PLAYER_SELECT_SKILL))
                {
                    var skillType = (SkillType)changedProps[Offendefense.PLAYER_SELECT_SKILL];
                    mySkill = SkillFactory.GetSkillBySkillType(skillType);
                    displayManager.UpdateMySkill(skillType);
                    BattleLogger.Instance.UpdateMySkill(skillType);
                }
                if (changedProps.ContainsKey(Offendefense.PLAYER_HEALTH))
                {
                    _health = (int)changedProps[Offendefense.PLAYER_HEALTH];
                    displayManager.SetMyHealth(_health);
                }
                if (changedProps.ContainsKey(Offendefense.PLAYER_GAGE))
                {
                    _gage = (int)changedProps[Offendefense.PLAYER_GAGE];
                    displayManager.SetMyGage(_gage);
                }
                if (changedProps.ContainsKey(Offendefense.PLAYER_SKILL_RESULT))
                {
                    var mySkillResult = GetMySkillResult();
                    displayManager.UpdateMySkillResult(mySkillResult);
                }
            }
            else
            {
                if (changedProps.ContainsKey(Offendefense.PLAYER_HEALTH))
                {
                    _oppHealth = (int)changedProps[Offendefense.PLAYER_HEALTH];
                    displayManager.SetOppHealth(_oppHealth);
                }
                if (changedProps.ContainsKey(Offendefense.PLAYER_GAGE))
                {
                    _oppGage = (int)changedProps[Offendefense.PLAYER_GAGE];
                    displayManager.SetOppGage(_oppGage);
                }
                if (changedProps.ContainsKey(Offendefense.PLAYER_SELECT_SKILL))
                {
                    var skillType = (SkillType)changedProps[Offendefense.PLAYER_SELECT_SKILL];
                    displayManager.UpdateOppSkill(skillType);
                    BattleLogger.Instance.UpdateOppSkill(skillType);
                }
                if (changedProps.ContainsKey(Offendefense.PLAYER_SKILL_RESULT))
                {
                    var mySkillResult = GetOppSkillResult();
                    displayManager.UpdateOppSkillResult(mySkillResult);
                }
            }
        }

        public void InitPlayerProperties()
        {
            var hash = new Hashtable
            {
                { Offendefense.PLAYER_SELECT_SKILL, (int)SkillType.None },
                { Offendefense.PLAYER_HEALTH, 100 },
                { Offendefense.PLAYER_GAGE, 0 },
                { Offendefense.PLAYER_UPDATING_SKILL_RESULT, false }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        
        public int GetMyHealth()
        {
            return _health;
        }
        
        public int AddMyHealth(int health)
        {
            _health += health;
            if(health != 0)
            {
                var hash = new Hashtable {{Offendefense.PLAYER_HEALTH, _health}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
            return _health;
        }
        
        public int GetMyGage()
        {
            return _gage;
        }
        
        public int AddMyGage(int gage)
        {
            _gage += gage;
            if(gage != 0)
            {
                var hash = new Hashtable {{Offendefense.PLAYER_GAGE, _gage}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }

            return _gage;
        }
        
        public int GetOppHealth()
        {
            return _oppHealth;
        }

        public int AddOppHealth(int health)
        {
            _oppHealth += health;
            if(health != 0)
            {
                var hash = new Hashtable {{Offendefense.PLAYER_HEALTH, _oppHealth}};
                var opp = PhotonNetwork.PlayerListOthers[0];
                opp.SetCustomProperties(hash);
            }

            return _oppHealth;
        }

        public int GetOppGage()
        {
            return _oppGage;
        }
        
        public int AddOppGage(int gage)
        {
            _oppGage += gage;
            if(gage != 0)
            {
                var hash = new Hashtable {{Offendefense.PLAYER_GAGE, _oppGage}};
                var opp = PhotonNetwork.PlayerListOthers[0];
                opp.SetCustomProperties(hash);
            }

            return _oppGage;
        }

        public Skills.Skill GetMySkill()
        {
            if(mySkill == null)
            {
                var skill = SkillFactory.GetSkillBySkillType(SkillType.None);
                SetMySkill(skill);
            }
            return mySkill;
        }
        
        public void SetMySkill(Skills.Skill skill)
        {
            var hash = new Hashtable {{Offendefense.PLAYER_SELECT_SKILL, (int)skill.SkillType}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            var skillType = (SkillType) PhotonNetwork.LocalPlayer.CustomProperties[Offendefense.PLAYER_SELECT_SKILL];
            mySkill = SkillFactory.GetSkillBySkillType(skillType);
        }

        public Skills.Skill GetOppSkill()
        {
            var opp = PhotonNetwork.PlayerListOthers[0];
            var oppSkillType = (SkillType) opp.CustomProperties[Offendefense.PLAYER_SELECT_SKILL];
            return SkillFactory.GetSkillBySkillType(oppSkillType);
        }
        
        public void SetMySkillResult(SkillResult skillResult)
        {
            var hash = new Hashtable {{Offendefense.PLAYER_SKILL_RESULT, SkillResult.Serialize(skillResult)}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        public void SetOppSkillResult(SkillResult oppSkillSkillResult)
        {
            var hash = new Hashtable {{Offendefense.PLAYER_SKILL_RESULT, SkillResult.Serialize(oppSkillSkillResult)}};
            var opp = PhotonNetwork.PlayerListOthers[0];
            opp.SetCustomProperties(hash);
        }
        
        public SkillResult GetMySkillResult()
        {
            var mySkillResultBytes = (byte[])PhotonNetwork.LocalPlayer.CustomProperties[Offendefense.PLAYER_SKILL_RESULT];
            return (SkillResult)SkillResult.Deserialize(mySkillResultBytes);
        }
        
        public SkillResult GetOppSkillResult()
        {
            var oppSkillResultBytes = (byte[])PhotonNetwork.PlayerListOthers[0].CustomProperties[Offendefense.PLAYER_SKILL_RESULT];
            return (SkillResult)SkillResult.Deserialize(oppSkillResultBytes);
        }

        public void SetPLAYER_LOADED_LEVEL(bool isLoaded)
        {
            var hash = new Hashtable {{Offendefense.PLAYER_LOADED_LEVEL, isLoaded}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        
        public bool GetPLAYER_LOADED_LEVEL()
        {
            return (bool)PhotonNetwork.LocalPlayer.CustomProperties[Offendefense.PLAYER_LOADED_LEVEL];
        }
    }
}