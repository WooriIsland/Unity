using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    [SerializeField] float scaleFactor = 1.15f;
    private Vector3 originalScale;

    protected override void Start()
    {
        base.Start();

        originalScale = transform.localScale;
    }

    void Update()
    {

    }

    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
        print("OnMove");
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        print("OnPointerDown");
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        print("OnPointerEnter");

        //transform.localScale = originalScale * scaleFactor;

        transform.DOScale(0.9f, 1f).SetEase(Ease.OutBack);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        print("OnPointerExit");

        //transform.localScale = originalScale;

        transform.DOScale(1f, 1f).SetEase(Ease.OutBack);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        print("OnPointerUp");
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        print("OnSelect");

        transform.DOScale(0.9f, 1f).SetEase(Ease.OutBounce);
        //SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_BUTTON);
    }
}
