/*using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAlpha : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public void CloseAlpha()
    {
        var v = canvasGroup.DOFade(0, 0.4f);
        v.onComplete = OnClose;
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OpenAlpha()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);

        var v = canvasGroup.DOFade(1, 0.4f);
        v.onComplete = OnOpen;
    }

    public virtual void OnOpen()
    {

    }
}*/
