using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 0 : 배경음악
    // 1 : 건축모드 음악
    public AudioClip[] audioClips;

    public AudioSource audioSource;

    private static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        audioSource.clip = audioClips[0];
    }

    // 배경음악 플레이
    private void PlayBGM()
    {
        audioSource.clip = audioClips[0];
    }

    // 섬꾸미기 음악 플레이
    private void PlayEditBGM()
    {
        audioSource.clip = audioClips[1];
    }

}
