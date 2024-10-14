using System.Collections.Generic;
using com.Nemodouble.Offendefense.Scripts.Skills;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts
{
    public enum SkillType
    {
        None = 0x00,
        Attack = 0x10,
        StrongAttack = 0x11,
        Execute = 0x12,
        Jap = 0x13,
        DoubleAttack = 0x14,
        PredictAttack = 0x15,
        Defense = 0x30,
        Dodge = 0x31,
        DefenseAttack = 0x32,
        Charge = 0x50,
        Meditation = 0x51,
    }
    
    public class SkillFactory : MonoBehaviour
    {
        // Singleton
        private static SkillFactory _instance;
        public static SkillFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SkillFactory>();
                }
                return _instance;
            }
        }
        
        public List<Skill> skillList;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }

        public static Skill GetSkillBySkillType(SkillType skillType)
        {
            var skill = Instance.skillList.Find(skill => skill.SkillType == skillType);
            if (skill != null) return Instantiate(skill);
            Debug.LogError("Skill is null : " + skillType);
            return null;
        }
    }
}