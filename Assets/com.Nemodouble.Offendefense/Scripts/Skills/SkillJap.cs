using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillJap", menuName = "Offendefense/Skill/Attack/Jap")]
    public class SkillJap : Skill
    {
        [SerializeField] private int damage = 15;
        [SerializeField] private bool canBeBlocked = true;
        [SerializeField] private bool canBeDodged = true;

        private void OnEnable()
        {
            SkillDescription.Add("damage", new IntVariable { Value = damage });
        }

        public override void GetAttack(AttackProperties attackProperties)
        {
            if (isGetAttack) return;
            if(attackProperties.CanInterrupt)
                isGetAttack = true;
            base.GetAttack(attackProperties);
        }
        
        public override void AfterAttack()
        {
            if (isGetAttack) return;
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