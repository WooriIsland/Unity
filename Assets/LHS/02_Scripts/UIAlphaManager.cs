using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlphaManager : MonoBehaviour
{
    public GameObject photo;

    //내가 가지고 있는 cavnas 컴포넌트를 접근하고 싶어
    public CanvasGroup canvasGroup;

    //시작할 때 가져오자
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //내가 켜지거나 꺼졌을 때 부드럽게 애니메이션을 만들고 싶어요
    public void ControlAlpha()
    {
        //내가 켜져있을까?
        //if (photo.activeSelf == true) canvasGroup.DOFade(1, 0.8f);
        //else canvasGroup.DOFade(0, 0.8f);

        if (photo.activeSelf == true)
        {
            canvasGroup.DOFade(1, 1.2f);
            //photo.transform.DOScale(1, 1f).SetEase(Ease.Linear);
        }
    }
}
