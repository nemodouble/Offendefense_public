using com.Nemodouble.Offendefense.Scripts.Networks;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillMeditation", menuName = "Offendefense/Skill/Charge/Meditation")]
    public class SkillMeditation : Skill
    {
        private void OnEnable()
        {
            SkillDescription.Add("gageGain", new IntVariable { Value = gageGain });
        }

        public override void Charge()
        {
            if(isGetAttack) return;
            SkillResult.GageChange += gageGain;
            if (isMySkill) 
                PlayerPropertiesManager.Instance.AddMyGage(gageGain);
            else
                PlayerPropertiesManager.Instance.AddOppGage(gageGain);
        }
    }
}