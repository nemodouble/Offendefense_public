using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillDoubleAttack", menuName = "Offendefense/Skill/Attack/DoubleAttack")]
    public class SkillDoubleAttack : Skill
    {
        [SerializeField] private int firstDamage = 40;
        [SerializeField] private int secondDamage = 40;
        [SerializeField] private bool canBeBlocked = true;
        [SerializeField] private bool canBeDodged = true;

        private void OnEnable()
        {
            SkillDescription.Add("firstDamage", new IntVariable { Value = firstDamage });
            SkillDescription.Add("secondDamage", new IntVariable { Value = secondDamage });
        }

        public override void Attack()
        {
            var attackProperties = new AttackProperties
            {
                Damage = firstDamage,
                CanBeBlocked = canBeBlocked,
                CanBeDodged = canBeDodged,
                CanInterrupt = true
            };
            OpponentSkill.GetAttack(attackProperties);
            attackProperties.Damage = secondDamage;
            OpponentSkill.GetAttack(attackProperties);
        }
    }
}