using com.Nemodouble.Offendefense.Scripts.Networks;
using com.Nemodouble.Offendefense.Scripts.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.Nemodouble.Offendefense.Scripts.UIs
{
    public class BattleLogUi : MonoBehaviour
    {
        public TextMeshProUGUI my;
        public Image myImage;
        public TextMeshProUGUI opp;
        public Image oppImage;
        
        public void SetText(SkillResult mySkillResult, SkillResult oppSkillResult)
        {
            var myHp = PlayerPropertiesManager.Instance.GetMyHealth();
            var oppHp = PlayerPropertiesManager.Instance.GetOppHealth();
            var myGage = PlayerPropertiesManager.Instance.GetMyGage();
            var oppGage = PlayerPropertiesManager.Instance.GetOppGage();
            my.text =
                $"<color=#6abe30>{myHp}</color> / <color=#fbf236>{myGage}</color>";
            opp.text =
                $"<color=#d95763>{oppHp}</color> / <color=#fbf236>{oppGage}</color>";
        }
    }
}