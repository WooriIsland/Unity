using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyButton02 : Button
{
    protected override void Start()
    {
        base.Start();

        //색 안변하게 하기 위해
        transition = Transition.None;
    }

    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
        //print("OnMove");
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        //print("OnPointerDown");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //print("OnPointerEnter");

        var v = transform.DOScale(0.98f, 0.5f).SetEase(Ease.OutBack);
        v.onComplete = () => {
            //print("트윈 끝!");
        };
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        //print("OnPointerExit");

        transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);
        base.OnSelect(eventData);
    }
}
