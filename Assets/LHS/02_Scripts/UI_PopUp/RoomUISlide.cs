using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUISlide : MonoBehaviour
{
    //������Ʈ���� ��� ������� �����̵��ؼ� �ö�´�
    //0.2�� ��������
    public GameObject[] slideObj;

    public float SaveSet;

    private bool isCoroutineRunning = false;

    public void OpenAction()
    {
        //SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        if (!isCoroutineRunning)
        {
            StartCoroutine(UpAction());
        }
    }

    IEnumerator UpAction()
    {
        isCoroutineRunning = true;

        slideObj[0].transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObj[1].transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObj[2].transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

        isCoroutineRunning = false;
    }

    public void CloseAction()
    {
        //SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        if (!isCoroutineRunning)
        {
            StartCoroutine(DownAction());
        }
    }

    IEnumerator DownAction()
    {
        isCoroutineRunning = true;

        slideObj[0].transform.DOLocalMoveY(SaveSet, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObj[1].transform.DOLocalMoveY(SaveSet, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.05f);

        slideObj[2].transform.DOLocalMoveY(SaveSet, 0.5f).SetEase(Ease.OutBack);

        isCoroutineRunning = false;
    }
}
