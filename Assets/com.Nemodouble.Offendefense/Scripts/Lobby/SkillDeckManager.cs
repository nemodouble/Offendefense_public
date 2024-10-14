using System.Collections.Generic;
using com.Nemodouble.Offendefense.Scripts.Lobby.Uis;
using com.Nemodouble.Offendefense.Scripts.Skills;
using com.Nemodouble.Offendefense.Scripts.UIs;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts.Lobby
{
    public class SkillDeckManager : MonoBehaviourPunCallbacks
    {
        // Singleton
        private static SkillDeckManager _instance;
        public static SkillDeckManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SkillDeckManager>();
                }
                return _instance;
            }
        }

        [SerializeField] private GameObject skillListContent;
        [SerializeField] private GameObject inRoomSkillListContent;
        [SerializeField] private GameObject skillInfoContentPrefab;
        
        public List<Skill> SkillDeck;
        private int nowSelectSlotIndex = 0;
        private bool isReady = false;
        private SkillSlotButton nowSelectSlotButton;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!Equals(targetPlayer, PhotonNetwork.LocalPlayer)) return;
            if (!changedProps.ContainsKey(Scripts.Offendefense.PLAYER_READY)) return;
            isReady = (bool) changedProps[Scripts.Offendefense.PLAYER_READY];
            if(isReady)
            {
                nowSelectSlotButton.SetSelected(false);
                nowSelectSlotButton = null;
            }
        }

        public void InitAbleSkillList(AllSkillInfoContent allSkillInfoContent)
        {
            foreach (var skillInfoContent in allSkillInfoContent.transform.GetComponentsInChildren<SkillInfoContent>())
            {
                Destroy(skillInfoContent.gameObject);
            }

            foreach (var skill in SkillFactory.Instance.skillList)
            {
                Instantiate(skillInfoContentPrefab, allSkillInfoContent.transform).GetComponent<SkillInfoContent>()
                    .Init(skill, skill.Icon, skill.SkillName.GetLocalizedString(), skill.SkillDescription.GetLocalizedString());
            }
        }

        public void SkillSlotSelected(SkillSlotButton button)
        {
            if(nowSelectSlotButton != null)
                nowSelectSlotButton.SetSelected(false);
            
            nowSelectSlotButton = button;
            nowSelectSlotIndex = button.index;
            nowSelectSlotButton.SetSelected(true);
        }

        public void SetSkill(Skill skill)
        {
            if (nowSelectSlotButton == null) return;
            nowSelectSlotButton.SetSkillInfo(skill);
            SkillDeck[nowSelectSlotIndex] = skill;
        }
    }
}