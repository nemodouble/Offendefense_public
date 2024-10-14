using com.Nemodouble.Offendefense.Scripts.Networks;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillCharge", menuName = "Offendefense/Skill/Charge/Charge")]
    public class SkillCharge : Skill
    {
        private void OnEnable()
        {
            SkillDescription.Add("gageGain", new IntVariable { Value = gageGain });
        }
        
        public override void Charge()
        {
            SkillResult.GageChange += gageGain;
            if (isMySkill) 
                PlayerPropertiesManager.Instance.AddMyGage(gageGain);
            else
                PlayerPropertiesManager.Instance.AddOppGage(gageGain);
        }
    }
}