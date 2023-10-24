using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ʈ �߰� �����ϴ� ������ ����ϴ� ��ũ��Ʈ
public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>();

    //��ġ�ɿ�����Ʈ�� ������ ��ġ
    public int PlaceObject(GameObject prefab, Vector3 vector3)
    {
        GameObject newObject = Instantiate(prefab);
        //�׸��� ��ġ�� �ٽ� World�� ��ȯ 
        newObject.transform.position = vector3;
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
