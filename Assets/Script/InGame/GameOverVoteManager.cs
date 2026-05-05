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
        Exit = 2,
        Next = 3
    }

    private enum VoteContext
    {
        Death = 1,
        LevelComplete = 2
    }

    public static GameOverVoteManager Instance { get; private set; }

    [Header("Scene")]
    [SerializeField] private string lobbySceneName = "MenuScene";
    [SerializeField] private string nextLevelSceneName = "";

    [Header("Canvas UI")]
    [SerializeField] private GameObject votePanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitLobbyButton;
    [SerializeField] private Button settingsButton;

    [Header("Settings UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button closeSettingsButton;
    [SerializeField] private Slider volumeSlider;

    private readonly Dictionary<int, VoteChoice> votes = new Dictionary<int, VoteChoice>();
    private VoteContext currentContext = VoteContext.Death;
    private bool voteStarted;
    private bool decisionApplied;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !voteStarted)
        {
            ToggleSettingsPanel();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        AutoAssignTextReferences();
        AutoAssignButtonReferences();
        HookButtons();
        SetupVolumeSlider();
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
            RaiseEventToAll(ShowVoteEventCode, new object[] { (int)VoteContext.Death, "A player died." });
        }
        else
        {
            ShowVotePanel(VoteContext.Death, "A player died.");
        }
    }

    public void ShowLevelCompleteVote()
    {
        if (voteStarted)
        {
            return;
        }

        if (votePanel == null)
        {
            Debug.LogError("[GameOverVoteManager] votePanel is not assigned. Assign your level complete vote UI panel.");
            return;
        }

        if (PhotonNetwork.InRoom)
        {
            RaiseEventToAll(ShowVoteEventCode, new object[] { (int)VoteContext.LevelComplete, "Level complete!" });
        }
        else
        {
            ShowVotePanel(VoteContext.LevelComplete, "Level complete!");
        }
    }

    public void VoteNextLevel()
    {
        SubmitLocalVote(VoteChoice.Next);
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

    private void ToggleSettingsPanel()
    {
        if (settingsPanel == null)
        {
            Debug.LogWarning("[GameOverVoteManager] Settings Panel is not assigned. Esc settings cannot open.");
            return;
        }

        bool shouldOpen = !settingsPanel.activeSelf;
        settingsPanel.SetActive(shouldOpen);
        if (votePanel != null && !voteStarted)
        {
            votePanel.SetActive(false);
        }
    }

    public void CloseEscapeMenu()
    {
        if (settingsPanel != null && !voteStarted)
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

        if (currentContext != VoteContext.LevelComplete)
        {
            SetButtonsInteractable(false);
        }

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

    private void ShowVotePanel(VoteContext context, string reason)
    {
        if (voteStarted)
        {
            return;
        }

        voteStarted = true;
        decisionApplied = false;
        currentContext = context;
        votes.Clear();
        AutoAssignTextReferences();
        AutoAssignButtonReferences();
        CamaraMovement.PauseAllCameras();

        votePanel.SetActive(true);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (titleText != null)
        {
            titleText.text = context == VoteContext.LevelComplete ? "Level Complete" : "Game Over";
        }

        if (context == VoteContext.LevelComplete)
        {
            SetStatus(reason + "\nVote for next level, restart, or lobby.");
        }
        else
        {
            SetStatus(reason + "\nVote to restart or return to lobby.");
        }

        SetButtonVisible(nextLevelButton, context == VoteContext.LevelComplete);
        SetButtonVisible(restartButton, true);
        SetButtonVisible(exitLobbyButton, true);
        SetButtonVisible(settingsButton, true);
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

        if (currentContext == VoteContext.LevelComplete)
        {
            int nextVotes = 0;
            int restartVotes = 0;
            int exitVotes = 0;

            foreach (VoteChoice vote in votes.Values)
            {
                if (vote == VoteChoice.Next)
                {
                    nextVotes++;
                }
                else if (vote == VoteChoice.Continue)
                {
                    restartVotes++;
                }
                else if (vote == VoteChoice.Exit)
                {
                    exitVotes++;
                }
            }

            if (exitVotes == expectedVotes)
            {
                ApplyDecisionToAll(VoteChoice.Exit);
                return;
            }

            if (nextVotes == expectedVotes)
            {
                ApplyDecisionToAll(VoteChoice.Next);
                return;
            }

            if (restartVotes == expectedVotes)
            {
                ApplyDecisionToAll(VoteChoice.Continue);
                return;
            }

            BroadcastStatus("Votes are split. Both players must choose the same option.");
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
            LoadLobbyScene();
            return;
        }

        if (decision == VoteChoice.Next)
        {
            LoadNextLevel();
            return;
        }

        RestartCurrentLevel();
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case ShowVoteEventCode:
                object[] showData = (object[])photonEvent.CustomData;
                ShowVotePanel((VoteContext)(int)showData[0], (string)showData[1]);
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

    private void LoadLobbyScene()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(lobbySceneName);
            }

            return;
        }

        SceneManager.LoadScene(lobbySceneName);
    }

    private void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevelSceneName))
        {
            Debug.LogWarning("[GameOverVoteManager] Next Level Scene Name is empty. Restarting current level instead.");
            RestartCurrentLevel();
            return;
        }

        SetStatus("Loading next level...");
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(nextLevelSceneName);
            }
        }
        else
        {
            SceneManager.LoadScene(nextLevelSceneName);
        }
    }

    private void RestartCurrentLevel()
    {
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

    private void HookButtons()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(VoteRestart);
            restartButton.onClick.AddListener(VoteRestart);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveListener(VoteNextLevel);
            nextLevelButton.onClick.AddListener(VoteNextLevel);
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

    private void SetupVolumeSlider()
    {
        if (volumeSlider == null)
        {
            return;
        }

        volumeSlider.SetValueWithoutNotify(AudioListener.volume);
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
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

    private void SetButtonVisible(Button button, bool visible)
    {
        if (button != null)
        {
            button.gameObject.SetActive(visible);
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (restartButton != null)
        {
            restartButton.interactable = interactable;
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = interactable;
        }

        if (exitLobbyButton != null)
        {
            exitLobbyButton.interactable = interactable;
        }
    }

    private void SetStatus(string message)
    {
        if (statusText == null)
        {
            AutoAssignTextReferences();
        }

        if (statusText != null)
        {
            statusText.text = message;
        }
        else
        {
            Debug.LogWarning("[GameOverVoteManager] Status Text is not assigned.");
        }
    }

    private string ChoiceToText(VoteChoice choice)
    {
        if (choice == VoteChoice.Next)
        {
            return "Next Level";
        }

        return choice == VoteChoice.Continue ? "Restart" : "Exit";
    }

    private void AutoAssignTextReferences()
    {
        if (votePanel == null || (titleText != null && statusText != null))
        {
            return;
        }

        TMP_Text[] texts = votePanel.GetComponentsInChildren<TMP_Text>(true);
        foreach (TMP_Text text in texts)
        {
            string lowerName = text.name.ToLower();
            string lowerText = text.text.ToLower();

            if (titleText == null && (lowerName.Contains("title") || lowerText.Contains("game over")))
            {
                titleText = text;
                continue;
            }

            if (statusText == null &&
                (lowerName.Contains("status") || lowerName.Contains("vote") || lowerText.Contains("new text")))
            {
                statusText = text;
            }
        }
    }

    private void AutoAssignButtonReferences()
    {
        if (votePanel == null || nextLevelButton != null)
        {
            return;
        }

        Button[] buttons = votePanel.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            TMP_Text label = button.GetComponentInChildren<TMP_Text>(true);
            string buttonName = button.name.ToLower();
            string labelText = label != null ? label.text.ToLower() : "";

            if (buttonName.Contains("next") || labelText.Contains("next level"))
            {
                nextLevelButton = button;
                Debug.Log("[GameOverVoteManager] Auto-assigned Next Level Button: " + button.name);
                return;
            }
        }
    }
}
