using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 0 : ¹è°æÀ½¾Ç
    // 1 : °ÇÃà¸ðµå À½¾Ç
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

    // ¹è°æÀ½¾Ç ÇÃ·¹ÀÌ
    public void PlayBGM()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();

    }

    // ¼¶²Ù¹Ì±â À½¾Ç ÇÃ·¹ÀÌ
    public void PlayEditBGM()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();

    }

    //int n = 0;
    //float currTime = 0;
    //private void Update()
    //{
    //    n++;

    //    currTime += Time.deltaTime;
    //    if(currTime >= 1)
    //    {
    //        print("frame : " + n);
    //        n = 0;
    //        currTime = 0;
    //    }
    //}
}
