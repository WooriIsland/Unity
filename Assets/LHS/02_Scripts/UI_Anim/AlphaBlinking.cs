using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//±ôºýÀÌ´Â ¸ð¼Ç
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

        canvasGroup.DOFade(0, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }
}
