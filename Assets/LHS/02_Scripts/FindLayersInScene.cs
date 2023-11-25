using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLayersInScene : MonoBehaviour
{
    void Start()
    {
        // 현재 씬에 있는 모든 GameObject 가져오기
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // GameObject의 레이어 출력
            Debug.Log(obj.name + "의 레이어: " + LayerMask.LayerToName(obj.layer));
        }
    }
}
