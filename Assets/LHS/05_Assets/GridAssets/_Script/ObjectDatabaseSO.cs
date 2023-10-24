using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트 스크립팅 가능
//ScriptableObject -> 클래스 인스턴스와는 별도로 대량의 데이터를 저장하는 데 사용할 수 있는 데이터 컨테이너
//주요 사용 사례 중 하나 - 값의 사본이 생성되는 것을 방지, 프로젝트의 메모리 사용을 줄임
//MonoBehaviour 스크립트에 변경되지 않는 데이터를 저장하는 프리팹이 있는 프로젝트의 경우 유용
[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject
{
    //오브젝트 리스트
    public List<ObjectData> objectsData;
}

[Serializable] //using System 필요
public class ObjectData
{
    //여기에 저장한 데이터를 다른 클래스가 수정못하게 하고 싶음.
    [field : SerializeField]
    public string Name { get; private set; }
    [field : SerializeField]
    public int ID { get; private set; }
    [field : SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field : SerializeField]
    public GameObject Prefab { get; private set; }
}
