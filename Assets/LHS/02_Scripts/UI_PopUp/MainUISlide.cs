using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUISlide : MonoBehaviour
{
    //������Ʈ���� ��� ������� �����̵��ؼ� �ö�´�
    //0.2�� ��������
    //�ݺ������� ����!
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

    //�ݺ��� ���� ���� ������?
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
