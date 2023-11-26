using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˾��� ���۽�ƼUI ���� ���� �� �ְ�
public class PopupStart : BasePopup
{
    public BaseAlpha back;

    protected override void Start()
    {
        back.OpenAlpha();
        Invoke("StartSetting", 0.3f);
    }

    public void StartSetting()
    {
        this.OpenAction();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    //�ݾ��������� ���� �ִϸ��̼��� �ٸ��� ������ 
    //�α��νÿ��� ���� �� ����
    public override void OnClose()
    {
        base.OnClose();

        //��ȯ �α���â �߱�
        OnBoardingManager.Instance.completeLoginBoxEmpty.GetComponent<BasePopup>().OpenAction();
        //back.CloseAlpha();
    }
}
