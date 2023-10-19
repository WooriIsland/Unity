using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing_LHS : MonoBehaviour
{
    private Grid_LHS grid;

    void Start()
    {
        grid = new Grid_LHS(4, 2, 10f, new Vector3(20, 0));
        //new Grid_LHS(2, 5, 5f, new Vector3(0, -20));
        new Grid_LHS(10, 10, 20f, new Vector3(-300f, -20));
    }

    private void Update()
    {
        //���� �ٲ�� ��
        if(Input.GetMouseButtonDown(0))
        {
            //���� ������ ��.
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
            //Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition
        }

        //�� �о� ��
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}
