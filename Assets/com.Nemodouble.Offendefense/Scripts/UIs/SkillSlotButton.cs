using com.Nemodouble.Offendefense.Scripts.Lobby;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts.UIs
{
    public class SkillSlotButton : SkillInfoButton
    {
        [SerializeField] private GameObject selected;
        
        public int index;

        protected override void Awake()
        {
            base.Awake();
            if (SkillDeckManager.Instance != null)
            {
                _skill = SkillDeckManager.Instance.SkillDeck[index];
                SetSkillInfo(_skill);
            }
        }

        public override void OnClickButton()
        {
            SkillDeckManager.Instance.SkillSlotSelected(this);
        }
        
        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                selected.SetActive(true);
            }
            else
            {
                selected.SetActive(false);
            }
        }
    }
}