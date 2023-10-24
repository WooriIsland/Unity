using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ʈ ��ũ���� ����
//ScriptableObject -> Ŭ���� �ν��Ͻ��ʹ� ������ �뷮�� �����͸� �����ϴ� �� ����� �� �ִ� ������ �����̳�
//�ֿ� ��� ��� �� �ϳ� - ���� �纻�� �����Ǵ� ���� ����, ������Ʈ�� �޸� ����� ����
//MonoBehaviour ��ũ��Ʈ�� ������� �ʴ� �����͸� �����ϴ� �������� �ִ� ������Ʈ�� ��� ����
[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject
{
    //������Ʈ ����Ʈ
    public List<ObjectData> objectsData;
}

[Serializable] //using System �ʿ�
public class ObjectData
{
    //���⿡ ������ �����͸� �ٸ� Ŭ������ �������ϰ� �ϰ� ����.
    [field : SerializeField]
    public string Name { get; private set; }
    [field : SerializeField]
    public int ID { get; private set; }
    [field : SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field : SerializeField]
    public GameObject Prefab { get; private set; }
}
