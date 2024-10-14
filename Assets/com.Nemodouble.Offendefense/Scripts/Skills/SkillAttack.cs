using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillAttack", menuName = "Offendefense/Skill/Attack/Attack")]
    public class SkillAttack : Skill
    {
        [SerializeField] private int damage = 40;
        [SerializeField] private bool canBeBlocked = true;
        [SerializeField] private bool canBeDodged = true;

        private void OnEnable()
        {
            SkillDescription.Add("damage", new IntVariable { Value = damage });
        }

        public override void Attack()
        {
            var attackProperties = new AttackProperties
            {
                Damage = damage,
                CanBeBlocked = canBeBlocked,
                CanBeDodged = canBeDodged,
                CanInterrupt = true
            };
            OpponentSkill.GetAttack(attackProperties);
        }
    }
}