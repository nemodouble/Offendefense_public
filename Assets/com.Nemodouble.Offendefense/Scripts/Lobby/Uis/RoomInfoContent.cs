using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.Nemodouble.Offendefense.Scripts.Lobby.Uis
{
    public class RoomInfoContent : MonoBehaviour
    {
        private RoomInfo _roomInfo;
        [SerializeField] private TextMeshProUGUI roomNameText;
        [SerializeField] private TextMeshProUGUI playerCountText;
        [SerializeField] private Button joinButton;

        public void Init(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            roomNameText.text = roomInfo.Name;
            var playerCountStr = "(" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + ")";
            playerCountText.text = playerCountStr;
            joinButton.interactable = roomInfo.PlayerCount < roomInfo.MaxPlayers;
            joinButton.onClick.AddListener(OnClickJoinButton);
        }
        
        public void OnClickJoinButton()
        {
            PhotonNetwork.JoinRoom(_roomInfo.Name);
        }
    }
}