using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float playerPower;
    public static float playerRateOfFire;
    public static int playerLevel;
    public static float playerShootDistance;
    public static float playerCurrentExperience;
    public static bool playerLoaded;
    public static float playerMaxHealth;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
