using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGPSOff : BasePopup
{
    [Header("OnClose ����� �۵�")]
    public GameObject goCustomSet;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    //�ݾ��������� ���� �ִϸ��̼��� �ٸ��� ������ 
    public override void OnClose()
    {
        base.OnClose();

        if(goCustomSet != null)
        {
            //������ �� ����Ǿ�� �ϴ� �׼�
            goCustomSet.SetActive(true);
        }
    }
}
