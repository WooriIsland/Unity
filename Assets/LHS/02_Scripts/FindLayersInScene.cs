using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLayersInScene : MonoBehaviour
{
    void Start()
    {
        // ���� ���� �ִ� ��� GameObject ��������
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // GameObject�� ���̾� ���
            Debug.Log(obj.name + "�� ���̾�: " + LayerMask.LayerToName(obj.layer));
        }
    }
}
