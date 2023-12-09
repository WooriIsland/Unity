using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//오브젝트 추가 제거하는 동작을 담당하는 스크립트
//포톤
public class ObjectPlacer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>();

    //public GameObject vfx;
    //배치될오브젝트와 벡터의 위치
    public int PlaceObject(GameObject prefab, Vector3 vector3)
    {
        //설치
        //GameObject newObject = Instantiate(prefab);
        //newObject.transform.position = vector3;
        //photonView.RPC("RpcShowObjectSetting", RpcTarget.All, prefab, vector3);

        GameObject newObject = PhotonNetwork.Instantiate(prefab.name, vector3, Quaternion.identity);

        ObjSetting objSetting = newObject.GetComponentInChildren<ObjSetting>();
        objSetting.previewObj.gameObject.SetActive(false);
        objSetting.baseObj.gameObject.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);

        photonView.RPC("RpcShow", RpcTarget.All, vector3);
        //그리드 위치를 다시 World로 변환 
        placedGameObjects.Add(newObject);

        //인덱스를 반환해야함
        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    { 
        //인덱스가 목록에 없음을 의미 + 조건
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

        print("설치 RPC로");
        ObjSetting objSetting = newObject.GetComponentInChildren<ObjSetting>();
        objSetting.previewObj.gameObject.SetActive(false);
        objSetting.baseObj.gameObject.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);

        //그리드 위치를 다시 World로 변환 
        placedGameObjects.Add(newObject);
    }
    public GameObject ImpactFactory;

    [PunRPC]
    void RpcShow(Vector3 point)
    {
        GameObject Impact = Instantiate(ImpactFactory);
        Impact.transform.position = point;
        print("실행");
    }
}
