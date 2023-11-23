using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoClick : MonoBehaviour
{
    //버튼을 클릭했을 때 해당오브젝트의 위치가 흔들리기
    public void ClickAction()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        //TweenCallback v
        //var v = transform.DOScale(1.2f, 0.4f).SetEase(Ease.InBack);
        var v = transform.DORotate(new Vector3(0, 0, -2), 0.3f).SetEase(Ease.OutQuad);
        v.onComplete = OnLeft;
        //var v = transform.DORotate(new Vector3(0, 0, 2), 0.3f).SetLoops(2, LoopType.Yoyo);
    }

    public void OnLeft()
    {
        var v2 = transform.DORotate(new Vector3(0, 0, 2), 0.3f).SetEase(Ease.OutQuad);
        v2.onComplete = OnBase;
    }

    public void OnBase()
    {
        transform.DORotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutQuad);
    }

}
