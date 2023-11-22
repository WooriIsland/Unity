using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    BasePopup nextPopup;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    // ���� ��
    public void CloseAction()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONOFF);

        //TweenCallback v
        var v = transform.DOScale(0, 0.4f).SetEase(Ease.InBack);
        v.onComplete = OnClose;
    }

    public void CloseAction(BasePopup next)
    {
        nextPopup = next;
        CloseAction();
    }

    // �� ������
    public virtual void OnClose()
    {
        // ���࿡ ���� �˾��� �ִٸ� �� �˾� ����
        if(nextPopup != null )
        {
            nextPopup.OpenAction();
            nextPopup = null;
        }
        // �� �ڽ� ���ֱ�
        gameObject.SetActive(false);
    }

    // �� ��
    public void OpenAction()
    {
        // 0���� ���� �� �ֵ���
        gameObject.transform.localScale = Vector3.zero;
        // �� �ڽ� Ű��
        gameObject.SetActive(true);

        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        var v = transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        v.onComplete = OnOpen;
    }

    public virtual void OnOpen()
    {

    }
}
