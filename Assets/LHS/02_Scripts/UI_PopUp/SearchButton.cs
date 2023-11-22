using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SearchButton : Button
{
    public SoundManager_LHS.ESfx soundManger;

    protected override void Start()
    {
        base.Start();

        //�� �Ⱥ��ϰ� �ϱ� ����
        transition = Transition.None;
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

        var v = transform.DOScale(0.9f, 0.8f).SetEase(Ease.OutBack);
        v.onComplete = () => {
            print("Ʈ�� ��!");
        };
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        print("OnPointerExit");

        transform.DOScale(1f, 0.8f).SetEase(Ease.OutBack);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        print("OnPointerUp");

        transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        print("OnSelect");

        transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutBounce);
        SoundManager_LHS.instance.PlaySFX(soundManger);

    }
}