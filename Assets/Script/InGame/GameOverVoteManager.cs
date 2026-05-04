using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverVoteManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte ShowVoteEventCode = 21;
    private const byte SubmitVoteEventCode = 22;
    private const byte StatusEventCode = 23;
    private const byte ApplyDecisionEventCode = 24;

    private enum VoteChoice
    {
        Continue = 1,
        Exit = 2
    }

    public static GameOverVoteManager Instance { get; private set; }

    [Header("Scene")]
    [SerializeField] private string lobbySceneName = "MenuScene";

    [Header("Canvas UI")]
    [SerializeField] private GameObject votePanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitLobbyButton;
    [SerializeField] private Button settingsButton;

    [Header("Settings UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button closeSettingsButton;

    private readonly Dictionary<int, VoteChoice> votes = new Dictionary<int, VoteChoice>();
    private bool voteStarted;
    private bool decisionApplied;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        HookButtons();
        HidePanels();
    }

    public static GameOverVoteManager FindInstance()
    {
        if (Instance != null)
        {
            return Instance;
        }

        GameOverVoteManager[] managers = Resources.FindObjectsOfTypeAll<GameOverVoteManager>();
        foreach (GameOverVoteManager manager in managers)
        {
            if (manager.gameObject.scene.IsValid())
            {
                Instance = manager;
                return manager;
            }
        }

        return null;
    }

    public void ShowDeathVote()
    {
        if (voteStarted)
        {
            return;
        }

        if (votePanel == null)
        {
            Debug.LogError("[GameOverVoteManager] votePanel is not assigned. Create your death Canvas UI, add GameOverVoteManager to it, then assign Vote Panel and buttons.");
            return;
        }

        if (PhotonNetwork.InRoom)
        {
            RaiseEventToAll(ShowVoteEventCode, "A player died.");
        }
        else
        {
            ShowVotePanel("A player died.");
        }
    }

    public void VoteRestart()
    {
        SubmitLocalVote(VoteChoice.Continue);
    }

    public void VoteExitLobby()
    {
        SubmitLocalVote(VoteChoice.Exit);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    private void SubmitLocalVote(VoteChoice choice)
    {
        if (!voteStarted || decisionApplied)
        {
            return;
        }

        SetButtonsInteractable(false);
        SetStatus($"Your vote: {ChoiceToText(choice)}. Waiting for other player...");

        int actorNumber = PhotonNetwork.InRoom ? PhotonNetwork.LocalPlayer.ActorNumber : 1;
        if (PhotonNetwork.InRoom)
        {
            RaiseEventToMaster(SubmitVoteEventCode, new object[] { actorNumber, (int)choice });
        }
        else
        {
            SubmitVote(actorNumber, (int)choice);
        }
    }

    private void ShowVotePanel(string reason)
    {
        if (voteStarted)
        {
            return;
        }

        voteStarted = true;
        decisionApplied = false;
        votes.Clear();

        votePanel.SetActive(true);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (titleText != null)
        {
            titleText.text = "Game Over";
        }

        SetStatus(reason + "\nVote to restart or return to lobby.");
        SetButtonsInteractable(true);
    }

    private void SubmitVote(int actorNumber, int voteValue)
    {
        if (!PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
        {
            return;
        }

        votes[actorNumber] = (VoteChoice)voteValue;

        int expectedVotes = PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.PlayerCount : 1;
        if (votes.Count < expectedVotes)
        {
            BroadcastStatus($"Votes: {votes.Count}/{expectedVotes}. Waiting...");
            return;
        }

        foreach (VoteChoice vote in votes.Values)
        {
            if (vote == VoteChoice.Exit)
            {
                ApplyDecisionToAll(VoteChoice.Exit);
                return;
            }
        }

        ApplyDecisionToAll(VoteChoice.Continue);
    }

    private void ApplyDecisionToAll(VoteChoice decision)
    {
        if (decisionApplied)
        {
            return;
        }

        decisionApplied = true;

        if (PhotonNetwork.InRoom)
        {
            RaiseEventToAll(ApplyDecisionEventCode, (int)decision);
        }
        else
        {
            ApplyDecision((int)decision);
        }
    }

    private void ApplyDecision(int decisionValue)
    {
        decisionApplied = true;
        SetButtonsInteractable(false);

        VoteChoice decision = (VoteChoice)decisionValue;
        if (decision == VoteChoice.Exit)
        {
            SetStatus("Returning to lobby...");
            StartCoroutine(LeaveRoomAndLoadLobby());
            return;
        }

        SetStatus("Restarting level...");
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case ShowVoteEventCode:
                ShowVotePanel((string)photonEvent.CustomData);
                break;
            case SubmitVoteEventCode:
                object[] voteData = (object[])photonEvent.CustomData;
                SubmitVote((int)voteData[0], (int)voteData[1]);
                break;
            case StatusEventCode:
                SetStatus((string)photonEvent.CustomData);
                break;
            case ApplyDecisionEventCode:
                ApplyDecision((int)photonEvent.CustomData);
                break;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient && voteStarted && !decisionApplied)
        {
            ApplyDecisionToAll(VoteChoice.Exit);
        }
    }

    private void BroadcastStatus(string message)
    {
        if (PhotonNetwork.InRoom)
        {
            RaiseEventToAll(StatusEventCode, message);
        }
        else
        {
            SetStatus(message);
        }
    }

    private void RaiseEventToAll(byte eventCode, object content)
    {
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(eventCode, content, options, SendOptions.SendReliable);
    }

    private void RaiseEventToMaster(byte eventCode, object content)
    {
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent(eventCode, content, options, SendOptions.SendReliable);
    }

    private IEnumerator LeaveRoomAndLoadLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            while (PhotonNetwork.InRoom)
            {
                yield return null;
            }
        }

        SceneManager.LoadScene(lobbySceneName);
    }

    private void HookButtons()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(VoteRestart);
            restartButton.onClick.AddListener(VoteRestart);
        }

        if (exitLobbyButton != null)
        {
            exitLobbyButton.onClick.RemoveListener(VoteExitLobby);
            exitLobbyButton.onClick.AddListener(VoteExitLobby);
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveListener(OpenSettings);
            settingsButton.onClick.AddListener(OpenSettings);
        }

        if (closeSettingsButton != null)
        {
            closeSettingsButton.onClick.RemoveListener(CloseSettings);
            closeSettingsButton.onClick.AddListener(CloseSettings);
        }
    }

    private void HidePanels()
    {
        if (votePanel != null)
        {
            votePanel.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (restartButton != null)
        {
            restartButton.interactable = interactable;
        }

        if (exitLobbyButton != null)
        {
            exitLobbyButton.interactable = interactable;
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private string ChoiceToText(VoteChoice choice)
    {
        return choice == VoteChoice.Continue ? "Restart" : "Exit";
    }
}
