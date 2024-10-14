using System;
using System.Collections;
using com.Nemodouble.Offendefense.Scripts.Networks;
using com.Nemodouble.Offendefense.Scripts.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace com.Nemodouble.Offendefense.Scripts.UIs
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI myNickname;
        [SerializeField] private TextMeshProUGUI oppNickname;
        
        [SerializeField] private TextMeshProUGUI LeftTimeText;
        [SerializeField] private TextMeshProUGUI MyHealthText;
        [SerializeField] private Slider MyHealthSlider;
        [SerializeField] private TextMeshProUGUI OppHealthText;
        [SerializeField] private Slider OppHealthSlider;
        [SerializeField] private TextMeshProUGUI MyGageText;
        [SerializeField] private Slider MyGageSlider;
        [SerializeField] private TextMeshProUGUI OppGageText;
        [SerializeField] private Slider OppGageSlider;
        [SerializeField] private LocalizedString localizedLeftTimeText;
        [SerializeField] private LocalizedString localizedMyHealthText;
        [SerializeField] private LocalizedString localizedOppHealthText;
        [SerializeField] private LocalizedString localizedMyGageText;
        [SerializeField] private LocalizedString localizedOppGageText;
        
        [Header("Selected Skill")]
        [SerializeField] private Image mySelectedSkill;
        [SerializeField] private Image myBluffSkill;
        [SerializeField] private GameObject myBluffLock;
        [SerializeField] private Image oppBluffSkill;
        [SerializeField] private GameObject oppBluffLock;
        
        [Header("Battle Result")]
        [SerializeField] private GameObject battleResultPanel;
        [SerializeField] private TextMeshProUGUI myUsedSkillText;
        [SerializeField] private TextMeshProUGUI oppUsedSkillText;
        [SerializeField] private Image myBattleSkill;
        [SerializeField] private Image oppBattleSkill;
        [SerializeField] private TextMeshProUGUI myHealthChangeText;
        [SerializeField] private TextMeshProUGUI oppHealthChangeText;
        [SerializeField] private TextMeshProUGUI myGageChangeText;
        [SerializeField] private TextMeshProUGUI oppGageChangeText;
        [SerializeField] private Slider battleResultSlider;
        
        [Space(10)]
        [SerializeField] private Slider timerSlider;
        [SerializeField] private GameObject gameEndPanel;
        [SerializeField] private TextMeshProUGUI gameEndText;
        
        private float time;

        private void Awake()
        {
            timerSlider.maxValue = GameManager.TIMER_MAX;
        }

        public void Init()
        {
            var nowHealth = PlayerPropertiesManager.Instance.GetMyHealth();
            var nowGage = PlayerPropertiesManager.Instance.GetMyGage();
            var oppHealth = PlayerPropertiesManager.Instance.GetOppHealth();
            var oppGage = PlayerPropertiesManager.Instance.GetOppGage();
            MyHealthSlider.maxValue = nowHealth;
            OppHealthSlider.maxValue = oppHealth;
            SetMyHealth(nowHealth);
            SetMyGage(nowGage);
            SetOppHealth(oppHealth);
            SetOppGage(oppGage);
            SetMyNickname();
            SetOppNickname();
        }

        public void SetMyHealth(int nowHealth)
        {
            localizedMyHealthText.Add("hp", new IntVariable{Value = nowHealth});
            MyHealthText.text = localizedMyHealthText.GetLocalizedString();
            MyHealthSlider.value = nowHealth;
        }
        
        public void SetMyGage(int nowGage)
        {
            localizedMyGageText.Add("gage", new IntVariable{Value = nowGage});
            MyGageText.text = localizedMyGageText.GetLocalizedString();
            MyGageSlider.value = nowGage;
        }

        public void UpdateMySkill()
        {
            var mySkill = PlayerPropertiesManager.Instance.GetMySkill().SkillType;
            UpdateMySkill(mySkill);
        }
        
        public void UpdateMySkill(SkillType skillType)
        {
            var mySkill = PlayerPropertiesManager.Instance.GetMySkill();
            mySelectedSkill.sprite = mySkill.Icon;
            if (time > GameManager.BLUFF_LOCK_TIME)
            {
                myBluffSkill.sprite = mySkill.Icon;
            }
        }
        
        public void UpdateOppSkill()
        {
            var oppSkill = PlayerPropertiesManager.Instance.GetOppSkill().SkillType;
            UpdateOppSkill(oppSkill);
        }
        
        public void UpdateOppSkill(SkillType skillType)
        {
            var oppSkill = PlayerPropertiesManager.Instance.GetOppSkill();
            if (time > GameManager.BLUFF_LOCK_TIME)
            {
                oppBluffSkill.sprite = oppSkill.Icon;
            }
        }

        public void UpdateMySkillResult(SkillResult skillResult)
        {
            myHealthChangeText.text = skillResult.HealthChange.ToString();
            myGageChangeText.text = skillResult.GageChange.ToString();
        }
        
        public void UpdateOppSkillResult(SkillResult skillResult)
        {
            oppHealthChangeText.text = skillResult.HealthChange.ToString();
            oppGageChangeText.text = skillResult.GageChange.ToString();
        }

        public void SetOppHealth(int oppHealth)
        {
            localizedOppHealthText.Add("hp", new IntVariable{Value = oppHealth});
            OppHealthText.text = localizedOppHealthText.GetLocalizedString();
            OppHealthSlider.value = oppHealth;
        }

        public void SetOppGage(int changeOppGage)
        {
            localizedOppGageText.Add("gage", new IntVariable{Value = changeOppGage});
            OppGageText.text = localizedOppGageText.GetLocalizedString();
            OppGageSlider.value = changeOppGage;
        }
        
        public void SetTimer(float timer)
        {
            time = timer;
            localizedLeftTimeText.Add("seconds", new StringVariable{Value = timer.ToString("F1")});
            LeftTimeText.text = localizedLeftTimeText.GetLocalizedString();
            timerSlider.value = timer;
            timerSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, timer / GameManager.TIMER_MAX);

            if (time <= GameManager.BLUFF_LOCK_TIME)
            {
                myBluffLock.SetActive(true);
                oppBluffLock.SetActive(true);
            }
        }

        private void SetMyNickname()
        {
            myNickname.text = Photon.Pun.PhotonNetwork.LocalPlayer.NickName;
        }
        
        private void SetOppNickname()
        {
            oppNickname.text = Photon.Pun.PhotonNetwork.PlayerListOthers[0].NickName;
        }

        public void EndGame(GameManager.GameResult isWin)
        {
            gameEndPanel.SetActive(true);
            switch (isWin)
            {
                case GameManager.GameResult.Win:
                    gameEndText.text = "You Win!";
                    break;
                case GameManager.GameResult.Lose:
                    gameEndText.text = "You Lose!";
                    break;
                case GameManager.GameResult.Draw:
                    gameEndText.text = "Draw!";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(isWin), isWin, null);
            }
        }

        public void UpdateBattle(float displayTime)
        {
            var mySKill = PlayerPropertiesManager.Instance.GetMySkill();
            var oppSkill = PlayerPropertiesManager.Instance.GetOppSkill();
            myUsedSkillText.text = $"{mySKill.SkillName.GetLocalizedString()}";
            oppUsedSkillText.text = $"{oppSkill.SkillName.GetLocalizedString()}";
            myBattleSkill.sprite = mySKill.Icon;
            oppBattleSkill.sprite = oppSkill.Icon;
            
            StartCoroutine(DisplayBattle(displayTime));
        }

        private IEnumerator DisplayBattle(float second)
        {
            battleResultPanel.SetActive(true);
            var time = second;
            battleResultSlider.maxValue = second;
            battleResultSlider.value = second;
            while (time > 0)
            {
                time -= Time.deltaTime;
                battleResultSlider.value = time;
                battleResultSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, time / second);
                yield return null;
            }
            battleResultPanel.SetActive(false);
            myBluffLock.SetActive(false);
            oppBluffLock.SetActive(false);
        }
    }
}