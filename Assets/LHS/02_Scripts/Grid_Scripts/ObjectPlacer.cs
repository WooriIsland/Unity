using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//������Ʈ �߰� �����ϴ� ������ ����ϴ� ��ũ��Ʈ
//����
public class ObjectPlacer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>();

    //��ġ�ɿ�����Ʈ�� ������ ��ġ
    public int PlaceObject(GameObject prefab, Vector3 vector3)
    {
        //��ġ
        //GameObject newObject = Instantiate(prefab);
        //newObject.transform.position = vector3;

        GameObject newObject = PhotonNetwork.Instantiate(prefab.name, vector3, Quaternion.identity);

        ObjSetting objSetting = newObject.GetComponentInChildren<ObjSetting>();
        objSetting.previewObj.gameObject.SetActive(false);
        objSetting.baseObj.gameObject.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);

        //�׸��� ��ġ�� �ٽ� World�� ��ȯ 
        placedGameObjects.Add(newObject);

        //�ε����� ��ȯ�ؾ���
        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    { 
        //�ε����� ��Ͽ� ������ �ǹ� + ����
        if(placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
        {
            return;
        }

        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
