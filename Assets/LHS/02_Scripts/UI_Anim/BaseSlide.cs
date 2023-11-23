using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlide : MonoBehaviour
{
    //오브젝트들을 담고 순서대로 슬라이드해서 올라온다
    //0.2초 간격으로
    public GameObject[] slideObjUp;
    public GameObject[] slideObjDown;

    public float UpSaveSet;
    public float DownSaveSet;

    private bool isCoroutineRunning = false;

    public void OpenAction()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        if(!isCoroutineRunning)
        {
            StartCoroutine(UpAction());
        }
    }

    IEnumerator UpAction()
    {
        isCoroutineRunning = true;

        slideObjDown[0].transform.DOLocalMoveY(UpSaveSet, 0.7f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObjDown[1].transform.DOLocalMoveY(UpSaveSet, 0.6f).SetEase(Ease.OutBack);

        slideObjUp[0].transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObjUp[1].transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObjUp[2].transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

        isCoroutineRunning = false;
    }

    public void CloseAction()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        if (!isCoroutineRunning)
        {
            StartCoroutine(DownAction());
        }
    }

    IEnumerator DownAction()
    {
        isCoroutineRunning = true;

        slideObjUp[0].transform.DOLocalMoveY(DownSaveSet, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObjUp[1].transform.DOLocalMoveY(DownSaveSet, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObjUp[2].transform.DOLocalMoveY(DownSaveSet, 0.5f).SetEase(Ease.OutBack);               
        
        slideObjDown[0].transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutBack);             

        yield return new WaitForSeconds(0.05f);

        slideObjDown[1].transform.DOLocalMoveY(0, 0.7f).SetEase(Ease.OutBack);

        isCoroutineRunning = false;
    }
}
