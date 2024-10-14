using com.Nemodouble.Offendefense.Scripts.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.Nemodouble.Offendefense.Scripts.Lobby.Uis
{
    [ExecuteInEditMode]
    public class SkillInfoContent : MonoBehaviour
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI skillDescriptionText;
        public Skill skill;

        public void Init(Skill skill, Sprite sprite, string skillName, string skillDescription)
        {
            this.skill = skill;
            skillImage.sprite = sprite;
            skillNameText.text = skillName;
            skillDescriptionText.text = skillDescription;
            var gageGain = skill.GetGageGain();
            var gageCost = skill.GetGageCost();
            if (gageGain > 0 && gageCost == 0)
            {
                transform.Find("Image").Find("Gage").GetComponent<TextMeshProUGUI>().text = gageGain.ToString();
                transform.Find("Image").Find("GageBg").GetComponent<Image>().color = new Color(0x55/255f, 0x9A/255f, 0x27/255f);
            }
            else
            {
                transform.Find("Image").Find("Gage").GetComponent<TextMeshProUGUI>().text = gageCost.ToString();
                transform.Find("Image").Find("GageBg").GetComponent<Image>().color = new Color(0xE0/255f, 0xC8/255f, 0x12/255f);
            }
        }
        
        public void OnClickButton()
        {
            SkillDeckManager.Instance.SetSkill(skill);
        }
    }
}