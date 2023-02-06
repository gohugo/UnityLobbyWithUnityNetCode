using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {


    public static LobbyUI Instance { get; private set; }


    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private GameObject changePlayerNameUI;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private Button changeBunnyButton;
    [SerializeField] private Button changeCactoroButton;
    [SerializeField] private Button changeNinjaButton;
    [SerializeField] private Button changeOrcButton;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button changeGameModeButton;
    [SerializeField] private Button startGameButton;

    private bool gameStarted = false;


    private void Awake() {
        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        changeBunnyButton.onClick.AddListener(() => {
            LobbyManager.Instance.UpdatePlayerCharacter(CharacterType.Bunny);
        });
        changeCactoroButton.onClick.AddListener(() => {
            LobbyManager.Instance.UpdatePlayerCharacter(CharacterType.Cactoro);
        });
        changeNinjaButton.onClick.AddListener(() => {
            LobbyManager.Instance.UpdatePlayerCharacter(CharacterType.Ninja);
        });
        changeOrcButton.onClick.AddListener(() => {
            LobbyManager.Instance.UpdatePlayerCharacter(CharacterType.Orc);
        });

        leaveLobbyButton.onClick.AddListener(() => {
            LobbyManager.Instance.LeaveLobby();
        });

        changeGameModeButton.onClick.AddListener(() => {
            LobbyManager.Instance.ChangeGameMode();
        });
        startGameButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.StartGame();
            gameStarted = true;
            HideLobbyUIAfterGameStart();
        });
}

    private void Start() {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
        ClearLobby();
        if (gameStarted) { return; }
        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerSingleUI.UpdatePlayer(player);
        }

        // si un jour veux avoir Game Mode
        //changeGameModeButton.gameObject.SetActive(LobbyManager.Instance.IsLobbyHost());
        startGameButton.gameObject.SetActive(LobbyManager.Instance.IsLobbyHost());
        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;

        if(!gameObject.activeSelf && !gameStarted)
            Show();
    }

    private void ClearLobby() {
        if (!container)
            return;
        foreach (Transform child in container) {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide() {
        Debug.Log("LobbyUI Hide");
        gameObject.SetActive(false);
    }    
    public void HideLobbyUIAfterGameStart() {
        Debug.Log("LobbyUI HideLobbyUIAfterGameStart");
        gameObject.SetActive(false);
        changePlayerNameUI.SetActive(false);
    }

    private void Show() {
        Debug.Log("LobbyUI Show");
        gameObject.SetActive(true);
    }

}