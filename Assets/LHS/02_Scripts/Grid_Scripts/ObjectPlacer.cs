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

    //public GameObject vfx;
    //��ġ�ɿ�����Ʈ�� ������ ��ġ
    public int PlaceObject(GameObject prefab, Vector3 vector3)
    {
        //��ġ
        //GameObject newObject = Instantiate(prefab);
        //newObject.transform.position = vector3;
        //photonView.RPC("RpcShowObjectSetting", RpcTarget.All, prefab, vector3);

        GameObject newObject = PhotonNetwork.Instantiate(prefab.name, vector3, Quaternion.identity);

        ObjSetting objSetting = newObject.GetComponentInChildren<ObjSetting>();
        objSetting.previewObj.gameObject.SetActive(false);
        objSetting.baseObj.gameObject.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);

        photonView.RPC("RpcShow", RpcTarget.All, vector3);
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

    [PunRPC]
    void RpcShowObjectSetting(GameObject prefab, Vector3 vector3)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = vector3;

        print("��ġ RPC��");
        ObjSetting objSetting = newObject.GetComponentInChildren<ObjSetting>();
        objSetting.previewObj.gameObject.SetActive(false);
        objSetting.baseObj.gameObject.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);

        //�׸��� ��ġ�� �ٽ� World�� ��ȯ 
        placedGameObjects.Add(newObject);
    }
    public GameObject ImpactFactory;

    [PunRPC]
    void RpcShow(Vector3 point)
    {
        GameObject Impact = Instantiate(ImpactFactory);
        Impact.transform.position = point;
        print("����");
    }
}
