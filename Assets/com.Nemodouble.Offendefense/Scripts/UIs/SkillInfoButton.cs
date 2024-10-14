using com.Nemodouble.Offendefense.Scripts.Skills;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace com.Nemodouble.Offendefense.Scripts.UIs
{
    public abstract class SkillInfoButton : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected Skill _skill;

        public Button _button;

        public abstract void OnClickButton();
        
        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            if(_skill != null)
                SetSkillInfo(_skill);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            SetSkillInfo(_skill);
        }

        public void SetSkillInfo(Skill skill)
        {
            _skill = skill;
            _button.image.sprite = _skill.Icon;
            transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = _skill.SkillName.GetLocalizedString();
            var gageGain = _skill.GetGageGain();
            var gageCost = _skill.GetGageCost();
            if (gageGain > 0 && gageCost == 0)
            {
                transform.Find("Gage").GetComponent<TextMeshProUGUI>().text = gageGain.ToString();
                transform.Find("GageBg").GetComponent<Image>().color = new Color(0x55/255f, 0x9A/255f, 0x27/255f);
            }
            else
            {
                transform.Find("Gage").GetComponent<TextMeshProUGUI>().text = gageCost.ToString();
                transform.Find("GageBg").GetComponent<Image>().color = new Color(0xE0/255f, 0xC8/255f, 0x12/255f);
            }
        }
    }
}