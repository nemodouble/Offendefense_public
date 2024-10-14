using com.Nemodouble.Offendefense.Scripts.Skills;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts
{
    public class GameSceneTester : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject gameManagers;

        private void Awake()
        {
            gameManagers.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
            PhotonPeer.RegisterType(typeof(SkillResult), 1, SkillResult.Serialize, SkillResult.Deserialize);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public override void OnJoinedRoom()
        {
            if(PhotonNetwork.PlayerList.Length == 2)
                gameManagers.SetActive(true);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(PhotonNetwork.PlayerList.Length == 2)
                gameManagers.SetActive(true);
        }
    }
}