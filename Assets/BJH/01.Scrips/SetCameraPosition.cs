using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraPosition : MonoBehaviour
{
    public Transform trPlayer;
    public Transform trCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        trCam.transform.position = trPlayer.transform.position + new Vector3(0, 2.5f, -2.5f); // ī�޶�� ������ �÷��̾�Լ� ���� �Ÿ��� ����
    }
}
