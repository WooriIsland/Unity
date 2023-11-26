using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaBlinking : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Start()
    {
        StartBlinking();
    }

    public void StartBlinking()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
