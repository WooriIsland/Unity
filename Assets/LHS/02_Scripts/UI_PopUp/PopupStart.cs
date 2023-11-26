using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//팝업과 오퍼시티UI 같이 나올 수 있게
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

    //닫았을때마다 각자 애니메이션이 다르기 때문에 
    //로그인시에만 닫을 수 있음
    public override void OnClose()
    {
        base.OnClose();

        //지환 로그인창 뜨기
        OnBoardingManager.Instance.completeLoginBoxEmpty.GetComponent<BasePopup>().OpenAction();
        //back.CloseAlpha();
    }
}
