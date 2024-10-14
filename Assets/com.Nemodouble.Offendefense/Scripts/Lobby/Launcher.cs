using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.Nemodouble.Offendefense.Scripts.Lobby.Uis;
using com.Nemodouble.Offendefense.Scripts.Skills;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace com.Nemodouble.Offendefense.Scripts.Lobby
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject loginPanel;
        [SerializeField] private TMP_InputField playerNameInputField;
        [FormerlySerializedAs("loggingInPanel")] [SerializeField] private GameObject loggingInPopUp;
        
        [Space(10)]
        [SerializeField] private GameObject skillSelectPanel;
        [SerializeField] private GameObject matchingPopup;
        [SerializeField] private TextMeshProUGUI matchingText;
        
        public LocalizedString matchingStartText;
        public LocalizedString waitingForOpponentText;
        public LocalizedString matchingSuccessText;
        
        private bool chaingingLocale;
        private int localeIndex;
        
        private string _gameVersion = "0.0.1";

        private void Awake()
        {
            if (PhotonNetwork.IsConnected)
            {
                SetActivePanel(skillSelectPanel);
            }
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = _gameVersion;
            PhotonPeer.RegisterType(typeof(SkillResult), 1, SkillResult.Serialize, SkillResult.Deserialize);
        }

        #region Pun Callbacks

        public override void OnConnectedToMaster()
        {
            SetActivePanel(skillSelectPanel);
            loggingInPopUp.SetActive(false);
        }
        
        public override void OnJoinedRoom()
        {
            Hashtable props = new Hashtable() {{Offendefense.PLAYER_LOADED_LEVEL, false}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            
            matchingText.text = waitingForOpponentText.GetLocalizedString();

            if (PhotonNetwork.PlayerList.Length != 2) return;
            matchingText.text = matchingSuccessText.GetLocalizedString();
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("GameScene");
        }

        public override void OnLeftRoom()
        {
            matchingPopup.SetActive(false);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.PlayerList.Length != 2) return;
            matchingText.text = matchingSuccessText.GetLocalizedString();
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel("GameScene");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            matchingText.text = "Matching Failed";
        }

        #endregion
        
        #region UI Callbacks

        public void OnEnterButtonClicked()
        {
            if(playerNameInputField.text.IsNullOrWhitespace())
            {
                PhotonNetwork.LocalPlayer.NickName  = "Player" + UnityEngine.Random.Range(1000, 10000);
            }
            else
            {
                PhotonNetwork.LocalPlayer.NickName = playerNameInputField.text;
            }
            PhotonNetwork.ConnectUsingSettings();
            loggingInPopUp.SetActive(true);
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }

        public void OnBackToMainClicked()
        {
            SetActivePanel(loginPanel);
            PhotonNetwork.Disconnect();
        }
        public void OnJoinRandomRoomButtonClicked()
        {
            matchingPopup.SetActive(true);
            matchingText.text =  matchingStartText.GetLocalizedString();
            var roomName = PhotonNetwork.LocalPlayer.NickName + "'s Room";
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 2,
                IsOpen = true,
                IsVisible = true
            };
            PhotonNetwork.JoinRandomOrCreateRoom(roomName:roomName, roomOptions:roomOptions);
        }
        
        public void OnMatchCancelClicked()
        {
            matchingPopup.SetActive(false);
            PhotonNetwork.LeaveRoom();
        }
        
        public void OnTextChanged()
        {
            if (playerNameInputField.text.Contains("\n"))
            {
                playerNameInputField.text = playerNameInputField.text.Replace("\n", "");
                OnEnterButtonClicked();
            }
        }
        
        public async void ChangeLocale()
        {
            if (chaingingLocale) return;
            await SetLocale(localeIndex++ % LocalizationSettings.AvailableLocales.Locales.Count);
        }

        private async Task SetLocale(int localeIndex)
        {
            chaingingLocale = true;
            // 비동기적으로 초기화가 완료될 때까지 대기
            await LocalizationSettings.InitializationOperation.Task;
            // 선택된 로케일을 변경
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
            chaingingLocale = false;
        }
        
        #endregion
        
        private void SetActivePanel(GameObject activePanel)
        {
            loginPanel.SetActive(activePanel == loginPanel);
            skillSelectPanel.SetActive(activePanel == skillSelectPanel);
            loggingInPopUp.SetActive(false);
            matchingPopup.SetActive(false);
        }
    }
}
