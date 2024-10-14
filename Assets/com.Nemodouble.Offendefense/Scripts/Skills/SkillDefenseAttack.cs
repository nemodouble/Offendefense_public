using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillDefenseAttack", menuName = "Offendefense/Skill/Defense/DefenseAttack")]
    public class SkillDefenseAttack : SkillDefense
    {
        [SerializeField] private int damage = 40;
        [SerializeField] private bool canBeBlocked = false;
        [SerializeField] private bool canBeDodged = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            SkillDescription.Add("damage", new IntVariable { Value = damage });
        }

        public override void GetAttack(AttackProperties attackProperties)
        {
            base.GetAttack(attackProperties);
            if(attackProperties.CanInterrupt)
                isGetAttack = true;
        }
        
        public override void AfterAttack()
        {
            if (!isGetAttack) return;
            var attackProperties = new AttackProperties
            {
                Damage = damage,
                CanBeBlocked = canBeBlocked,
                CanBeDodged = canBeDodged,
                CanInterrupt = false
            };
            OpponentSkill.GetAttack(attackProperties);
        }
    }
}