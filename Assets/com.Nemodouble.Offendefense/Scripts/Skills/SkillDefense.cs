using System.Collections.Generic;
using com.Nemodouble.Offendefense.Scripts.Networks;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    [CreateAssetMenu(fileName = "SkillDefense", menuName = "Offendefense/Skill/Defense/Defense")]
    public class SkillDefense : Skill
    {
        private Queue<int> _guardDamageQueue;
        [SerializeField]
        private List<int> guardDamageList;

        protected virtual void OnEnable()
        {
            for (var i = 0; i < guardDamageList.Count; i++)
            {
                SkillDescription.Add("guardDamage" + i, new IntVariable{ Value = guardDamageList[i]});
            }
        }
        
        public override void Defense()
        {
            _guardDamageQueue = new Queue<int>();
            foreach (var guardDamage in guardDamageList)
            {
                _guardDamageQueue.Enqueue(guardDamage);
            }
        }

        public override void GetAttack(AttackProperties attackProperties)
        {
            if (_guardDamageQueue.Count == 0 || !attackProperties.CanBeBlocked)
            {
                base.GetAttack(attackProperties);
                return;
            }
            var defense = _guardDamageQueue.Dequeue();
            if (defense < attackProperties.Damage)
            {
                SkillResult.HealthChange += attackProperties.Damage - defense;
                if (isMySkill)
                    PlayerPropertiesManager.Instance.AddMyHealth(defense - attackProperties.Damage);
                else
                    PlayerPropertiesManager.Instance.AddOppHealth(defense - attackProperties.Damage);
            }
        }
    }
}