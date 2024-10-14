using com.Nemodouble.Offendefense.Scripts.Networks;
using UnityEngine;
using UnityEngine.Localization;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    public class Skill : ScriptableObject
    {
        public Sprite Icon;
        public LocalizedString SkillName;
        public LocalizedString SkillDescription;
        public SkillType SkillType;
        public SkillResult SkillResult;
        
        [SerializeField] protected int gageCost;
        [SerializeField] protected int gageGain;
        protected bool isGetAttack = false;
        
        protected bool isMySkill;
        protected Skill OpponentSkill;

        public virtual void UseStart()
        {
            SkillResult = new SkillResult();
            if(gageCost == 0) return;
            SkillResult.GageChange -= gageCost;
            if (isMySkill)
                PlayerPropertiesManager.Instance.AddMyGage(-gageCost);
            else
                PlayerPropertiesManager.Instance.AddOppGage(-gageCost);
        }
        public virtual void BeforeDefense() { }
        public virtual void Defense() { }
        public virtual void AfterDefense() { }
        public virtual void BeforeAttack() { }
        public virtual void Attack() { }
        public virtual void AfterAttack() { }

        public virtual void GetAttack(AttackProperties attackProperties)
        {
            var damage = attackProperties.Damage;
            SkillResult.HealthChange -= damage;
            if (isMySkill)
                PlayerPropertiesManager.Instance.AddMyHealth(-damage);
            else
                PlayerPropertiesManager.Instance.AddOppHealth(-damage);
            
            if(attackProperties.CanInterrupt)
                isGetAttack = true;
        }

        public virtual void BeforeCharge() { }
        public virtual void Charge() { }
        public virtual void AfterCharge() { }
        
        public virtual void UseEnd() { }

        public void SetIsMySkill(bool isMySkill)
        {
            this.isMySkill = isMySkill;
        }
        
        public void SetOppSkill(Skill oppSkill)
        {
            OpponentSkill = oppSkill;
        }
        
        public int GetGageCost()
        {
            return gageCost;
        }

        public int GetGageGain()
        {
            return gageGain;
        }
        
        public string GetDescription()
        {
            return SkillDescription.GetLocalizedString();
        }
    }
}