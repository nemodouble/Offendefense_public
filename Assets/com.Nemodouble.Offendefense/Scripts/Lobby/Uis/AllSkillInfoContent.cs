using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts.Lobby.Uis
{
    public class AllSkillInfoContent : MonoBehaviour
    {
        private void OnEnable()
        {
            SkillDeckManager.Instance.InitAbleSkillList(this);
        }
    }
}