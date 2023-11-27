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
        BGM_INTRO,
        BGM_XMAS
    }

    public enum ESfx
    {
        SFX_BUTTONON,
        SFX_BUTTONOFF,
        SFX_BtnSearch,
        SFX_BtnAdd,
        SFX_LodingCat,
        SFX_Hellow,
        Glitter,
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

    public void PlayBGM(int bgmIdx)
    {
        audioBgm.clip = bgms[bgmIdx];
        audioBgm.Play();

        //2번인데
        /*if (bgmIdx == 2)
        {
            print("같은소리이다");
        }

        else
        {
            audioBgm.clip = bgms[bgmIdx];
            audioBgm.Play();
        }*/
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
