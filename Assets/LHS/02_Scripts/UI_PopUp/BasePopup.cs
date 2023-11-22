using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    // 닫을 때
    public void CloseAction()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONOFF);

        //TweenCallback v
        var v = transform.DOScale(0, 0.4f).SetEase(Ease.InBack);
        v.onComplete = OnClose;
    }

    // 다 닫으면
    public virtual void OnClose()
    {
        // 나 자신 꺼주기
        gameObject.SetActive(false);
    }

    // 열 때
    public void OpenAction()
    {
        // 0부터 켜질 수 있도록
        gameObject.transform.localScale = Vector3.zero;
        // 나 자신 키기
        gameObject.SetActive(true);

        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        var v = transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        v.onComplete = OnOpen;
    }

    public virtual void OnOpen()
    {

    }
}
