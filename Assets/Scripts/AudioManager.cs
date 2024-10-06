using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTrack()
    {
        audioSource.Play();
    }
}
