using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public AudioManager audioManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        audioManager.PlayTrack();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
