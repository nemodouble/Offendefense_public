using com.Nemodouble.Offendefense.Scripts.Networks;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillDodge", menuName = "Offendefense/Skill/Defense/Dodge")]
    public class SkillDodge : Skill
    {
        [SerializeField] private int dodgeCount = 1;

        private void OnEnable()
        {
            SkillDescription.Add("dodgeCount", new IntVariable { Value = dodgeCount });
        }
        
        public override void GetAttack(AttackProperties attackProperties)
        {
            if (!attackProperties.CanBeDodged) return;
            if (dodgeCount-- > 0) return;
            SkillResult.HealthChange -= attackProperties.Damage;
            if (isMySkill)
                PlayerPropertiesManager.Instance.AddMyHealth(-attackProperties.Damage);
            else
                PlayerPropertiesManager.Instance.AddOppHealth(-attackProperties.Damage);
        }
    }
}