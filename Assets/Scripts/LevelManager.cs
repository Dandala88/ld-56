using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [ContextMenu("Win")]
    public void Win()
    {
        SceneManager.LoadScene(2);
    }
}
