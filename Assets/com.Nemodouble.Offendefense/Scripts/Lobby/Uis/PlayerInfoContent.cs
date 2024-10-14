using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts.Lobby.Uis
{
    public class PlayerInfoContent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerReadyText;

        private int ownerId;
        private bool isPlayerReady;

        private void Start()
        {
            Hashtable initialProps = new Hashtable() {{Offendefense.PLAYER_READY, isPlayerReady}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            
        }
        
        public void Init(Player player)
        {
            ownerId = player.ActorNumber;
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                playerNameText.text = "<color=#fbf236>" + player.NickName + "</color>";
            else
                playerNameText.text = player.NickName;
            if (!player.CustomProperties.TryGetValue(Offendefense.PLAYER_READY, out var isPlayerReady)) return;
            SetPlayerReady(player.IsMasterClient, (bool) isPlayerReady);
        }
        
        public void SetPlayerReady(bool isMasterClient, bool playerReady)
        {
            if(isMasterClient)
                playerReadyText.text = "Host";
            else if (playerReady)
                playerReadyText.text = "Ready";
            else
                playerReadyText.text = "Not Ready";
        }
    }
}