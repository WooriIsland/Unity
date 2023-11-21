using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_LHS : MonoBehaviour
{

    public static SoundManager_LHS instance;

    public enum EBgm
    {
        BGM_GAME,
        BGM_CUSTOM,
    }

    public enum ESfx
    {
        SFX_BUTTON
    }

    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    [SerializeField] AudioSource audioBgm;
    [SerializeField] AudioSource audioSfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(EBgm bgmIdx)
    {
        audioBgm.clip = bgms[(int)bgmIdx];

        audioBgm.Play();
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }

    public void PlaySFX(ESfx esfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx]);
    }
}
