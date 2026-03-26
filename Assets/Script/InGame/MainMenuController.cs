using UnityEngine;
using TMPro;
using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject lobbyPanel;
    public GameObject settingsPanel;
    public GameObject passwordPanel;
    public GameObject teamPanel;

    public TMP_InputField passwordInputField;
    public TMP_Text errorText;

    public LobbyManager lobbyManager;
    private PhotonManager photonManager;

    private bool isJoinMode = false;

    void Awake()
    {
        photonManager = PhotonManager.Instance;
        if (photonManager == null)
        {
            Debug.LogError("PhotonManager instance not found!");
        }
    }

    public void OnClickStart()
    {
        Debug.Log("OnClickStart called");

        if (menuPanel != null && lobbyPanel != null)
        {
            menuPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }

        if (photonManager != null)
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                photonManager.ConnectToServer();
            }
        }
        else
        {
            Debug.LogWarning("PhotonManager not assigned!");
        }

        passwordPanel?.SetActive(false);
        teamPanel?.SetActive(false);
    }

    public void OnClickCreateRoom()
    {
        Debug.Log("OnClickCreateRoom called");
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Not connected to Photon. Connecting first...");
            if (photonManager != null)
            {
                photonManager.ConnectToServer();
            }
            return;
        }

        isJoinMode = false;
        if (lobbyPanel != null)
            lobbyPanel.SetActive(false);
        if (passwordPanel != null)
            passwordPanel.SetActive(true);
    }

    public void OnClickJoinRoom()
    {
        Debug.Log("OnClickJoinRoom called");
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Not connected to Photon. Connecting first...");
            if (photonManager != null)
            {
                photonManager.ConnectToServer();
            }
            return;
        }

        isJoinMode = true;
        if (lobbyPanel != null)
            lobbyPanel.SetActive(false);
        if (passwordPanel != null)
            passwordPanel.SetActive(true);
    }

    public void OnPasswordInputChanged()
    {
        // Clear error message when user starts typing a new password
        if (errorText != null)
        {
            errorText.text = "";
        }
    }

    public void OnClickBackFromPassword()
    {
        Debug.Log("OnClickBackFromPassword called");
        if (passwordPanel != null)
        {
            passwordPanel.SetActive(false);
        }

        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
        }

        // Clear the password input when leaving the panel
        if (passwordInputField != null)
        {
            passwordInputField.text = "";
        }
    }

    public void OnClickSubmitPassword()
    {
        Debug.Log("OnClickSubmitPassword called");

        // Clear error message when attempting to submit
        if (errorText != null)
        {
            errorText.text = "";
        }

        if (passwordInputField == null)
        {
            Debug.LogError("passwordInputField not assigned");
            return;
        }

        var roomPassword = passwordInputField.text.Trim();
        Debug.Log("Room password: '" + roomPassword + "'");

        if (string.IsNullOrEmpty(roomPassword))
        {
            Debug.LogWarning("Please enter a room password");
            if (errorText != null)
            {
                errorText.text = "Please enter a password.";
            }
            return;
        }

        if (lobbyManager == null)
        {
            Debug.LogError("LobbyManager not assigned");
            return;
        }

        if (isJoinMode)
        {
            Debug.Log("Attempting to join room with password: " + roomPassword);
            lobbyManager.JoinRoomWithPassword(roomPassword);
        }
        else
        {
            Debug.Log("Attempting to create room with password: " + roomPassword);
            lobbyManager.CreateRoomWithPassword(roomPassword);
        }

        if (passwordPanel != null)
            passwordPanel.SetActive(false);
    }

    public void OnClickBack()
    {
        Debug.Log("OnClickBack called");
        if (menuPanel != null && lobbyPanel != null)
        {
            lobbyPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("menuPanel or lobbyPanel is not assigned in MainMenuController.");
        }
    }

    public void OnClickOpenSettings()
    {
        Debug.Log("OnClickOpenSettings called");
        if (settingsPanel != null && menuPanel != null)
        {
            settingsPanel.SetActive(true);
            menuPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("settingsPanel or menuPanel is not assigned. Please assign it in the Inspector.");
        }
    }

    public void OnClickCloseSettings()
    {
        Debug.Log("OnClickCloseSettings called");
        if (settingsPanel != null && menuPanel != null)
        {
            settingsPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("settingsPanel or menuPanel is not assigned. Please assign it in the Inspector.");
        }
    }

    // Use this method for Settings->Back button to avoid confusion with lobby Back.
    public void OnClickBackFromSettings()
    {
        Debug.Log("OnClickBackFromSettings called");
        OnClickCloseSettings();
    }

    public void ShowPasswordPanel()
    {
        if (passwordPanel != null)
        {
            passwordPanel.SetActive(true);
        }

        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(false);
        }

        if (errorText != null)
        {
            errorText.text = "";
        }
    }

    public void ShowPasswordPanelWithError(string errorMessage)
    {
        ShowPasswordPanel();
        if (errorText != null)
        {
            errorText.text = errorMessage;
        }
    }

    public void OnRoomJoined()
    {
        Debug.Log("MainMenuController: room joined, entering team panel");
        if (passwordPanel != null)
        {
            passwordPanel.SetActive(false);
            Debug.Log("PasswordPanel hidden");
        }

        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(false);
            Debug.Log("LobbyPanel hidden");
        }

        if (teamPanel != null)
        {
            teamPanel.SetActive(true);
            Debug.Log("TeamPanel shown");
        }
        else
        {
            Debug.LogError("teamPanel not assigned!");
        }
    }

    public void OnClickBackToLobby()
    {
        Debug.Log("OnClickBackToLobby called");

        if (teamPanel != null)
        {
            teamPanel.SetActive(false);
        }

        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
        }

        // Leave the room so that join/create works on next attempt
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Leaving current room before going back to lobby");
            PhotonNetwork.LeaveRoom();
        }

        if (passwordInputField != null)
        {
            passwordInputField.text = "";
        }

        if (errorText != null)
        {
            errorText.text = "";
        }
    }

    public void OnClickQuitGame()
    {
        Debug.Log("QuitGame triggered");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}