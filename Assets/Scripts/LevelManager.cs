using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<Enemy> bosslist = new List<Enemy>();

    [ContextMenu("Win")]
    public void Win()
    {
        SceneManager.LoadScene(2);
    }

    public void Update()
    {
        var allbosses = true;
        foreach(Enemy enemy in bosslist)
        {
            if(enemy == null) continue;
            allbosses = false;
        }

        if(allbosses)
        {
            Win();
        }
    }
}
