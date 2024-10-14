using com.Nemodouble.Offendefense.Scripts.Lobby;
using com.Nemodouble.Offendefense.Scripts.Lobby.Uis;
using com.Nemodouble.Offendefense.Scripts.Networks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.Nemodouble.Offendefense.Scripts.UIs
{
    [ExecuteInEditMode]
    public class SkillSelectButton : SkillInfoButton, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private int skillIndex;
        private bool isMouseOver = false;
        private float mouseOverTime = 0f;
        
        public GameObject _skillInfoPopup;
        private RectTransform _skillInfoPopupRectTransform;
        private RectTransform _canvasRectTransform;
        
        protected override void Awake()
        {
            base.Awake();
            if (SkillDeckManager.Instance != null)
            {
                _skill = SkillDeckManager.Instance.SkillDeck[skillIndex];
                SetSkillInfo(_skill);
            }
            _skillInfoPopupRectTransform = _skillInfoPopup.GetComponent<RectTransform>();
            _canvasRectTransform = _skillInfoPopup.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (isMouseOver)
            {
                OnMouseOver();
            }
        }

        public override void OnClickButton()
        {
            if(_skill == null)
            {
                Debug.LogError("Skill is null");
                return;
            }

            PlayerPropertiesManager.Instance.SetMySkill(_skill);
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!Equals(targetPlayer, PhotonNetwork.LocalPlayer)) return;
            if (!changedProps.ContainsKey(Offendefense.PLAYER_GAGE)) return;
            var gage = (int) changedProps[Offendefense.PLAYER_GAGE];
            _button.interactable = gage >= _skill.GetGageCost();
        }

        public void OnMouseOver()
        {
            if(mouseOverTime < 0.5f)
                mouseOverTime += Time.deltaTime;
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, Input.mousePosition, null,
                    out var mousePosition);
                var rect = _canvasRectTransform.rect;
                var x = Mathf.Clamp(mousePosition.x,
                    -rect.width / 2 + _skillInfoPopupRectTransform.rect.width,
                    rect.width / 2);
                mousePosition.x = x;
                _skillInfoPopup.SetActive(true);
                _skillInfoPopupRectTransform.anchoredPosition = mousePosition;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
            mouseOverTime = 0f;
            _skillInfoPopup.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOver = true;
            _skillInfoPopup.GetComponent<SkillInfoContent>().Init(_skill, _skill.Icon, _skill.SkillName.GetLocalizedString(), _skill.SkillDescription.GetLocalizedString());
        }
    }
}