using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUISlide : MonoBehaviour
{
    //오브젝트들을 담고 순서대로 슬라이드해서 올라온다
    //0.2초 간격으로
    //반복문으로 변경!
    public GameObject[] slideObj;

    public float SaveSet;

    private bool isCoroutineRunning = false;

    public void OpenAction()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(UpAction());
        }
    }

    //반복문 쓰면 되지 않을까?
    IEnumerator UpAction()
    {
        isCoroutineRunning = true;

        for(int i = 0; i < slideObj.Length; i++)
        {
            slideObj[i].transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.05f);

            if(i == slideObj.Length-1)
            {
                isCoroutineRunning = false;
            }
        }
    }

    public void CloseAction()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(DownAction());
        }
    }

    IEnumerator DownAction()
    {
        isCoroutineRunning = true;

        for (int i = 0; i < slideObj.Length; i++)
        {
            slideObj[i].transform.DOLocalMoveY(SaveSet, 0.5f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.05f);

            if (i == slideObj.Length -1)
            {
                isCoroutineRunning = false;
            }
        }
    }
}
