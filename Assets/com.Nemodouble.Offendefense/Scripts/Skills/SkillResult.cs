using System;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts.Skills
{
    public struct SkillResult
    {
        public int HealthChange;
        public int GageChange;
        
        // serialize to byte array
        public static byte[] Serialize(object skillResult)
        {
            var skillResultObj = (SkillResult) skillResult;
            var bytes = new byte[8];
            var healthChangeBytes = BitConverter.GetBytes(skillResultObj.HealthChange);
            var gageChangeBytes = BitConverter.GetBytes(skillResultObj.GageChange);
            Array.Copy(healthChangeBytes, bytes, 4);
            Array.Copy(gageChangeBytes, 0, bytes, 4, 4);
            return bytes;
        }
        
        // deserialize from byte array
        public static object Deserialize(byte[] bytes)
        {
            var healthChangeBytes = new byte[4];
            var gageChangeBytes = new byte[4];
            Array.Copy(bytes, healthChangeBytes, 4);
            Array.Copy(bytes, 4, gageChangeBytes, 0, 4);
            var healthChange = BitConverter.ToInt32(healthChangeBytes, 0);
            var gageChange = BitConverter.ToInt32(gageChangeBytes, 0);
            return new SkillResult
            {
                HealthChange = healthChange,
                GageChange = gageChange
            };
        }
    }
}