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
        /*audioSource.clip = audioClips[0];
        audioSource.Play();*/

        SoundManager_LHS.instance.PlayBGM(SoundManager_LHS.EBgm.BGM_GAME);
    }

    // 배경음악 플레이
    public void PlayBGM()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();

    }

    // 섬꾸미기 음악 플레이
    public void PlayEditBGM()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();

    }

}
