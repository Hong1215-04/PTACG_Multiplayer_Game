using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Bootstrap Init...");

        // Load the MenuScene to start the game
        SceneManager.LoadScene("MenuScene");
    }
}