using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlphaManager : MonoBehaviour
{
    public GameObject photo;

    //���� ������ �ִ� cavnas ������Ʈ�� �����ϰ� �;�
    public CanvasGroup canvasGroup;

    //������ �� ��������
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //���� �����ų� ������ �� �ε巴�� �ִϸ��̼��� ����� �;��
    public void ControlAlpha()
    {
        //���� ����������?
        //if (photo.activeSelf == true) canvasGroup.DOFade(1, 0.8f);
        //else canvasGroup.DOFade(0, 0.8f);

        if (photo.activeSelf == true)
        {
            canvasGroup.DOFade(1, 1.2f);
            //photo.transform.DOScale(1, 1f).SetEase(Ease.Linear);
        }
    }
}
