using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ġ�� ��ü�� ���� �����͸� ������ ��� �ʿ�
public class GridData
{
    //��ųʸ� Ű, ��(��ġ ������)
    //C#9�� ���� �ش� ��ü�� ������ �� �ݺ����� �ʰ� new�� �Է��Ͽ� �� ���� �Է��� �� �� ������
    //��ġ �����͸� ȣ���ϰų� ��ġ�� ��ü�� ��ġ�� �� �� ����
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    //��ü �߰��� ȣ�� (�׸��� ��ġ, ������, ID, object �ε���)
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {

        //List<Vector3Int> positionToOccupy = new List<Vector3Int>();
        //�׸��� ��ġ , ������ (��ġ���)
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        //�����Ϳ� �����ϸ� ���⿡ ID�� ������ ��ġ ���� 
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);

        foreach(var pos in positionToOccupy)
        {
            //��ġ�� ��ü�� �׸��� ���ԵǾ� �ִ��� Ȯ��
            if(placedObjects.ContainsKey(pos))
            {
                //�� ���� �߻�
                Debug.Log("�̹� �ڸ��� �ֽ��ϴ�.");
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }

            //������ ó�� , ��ġ �����Ͱ� �����Ƿ�
            placedObjects[pos] = data;
        }
    }

    //��ġ ���
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        //��ȸ��
        //������ �Ÿ��� ���� ��
        //��ü�� ȸ���Ϸ��� ���� �Ʒ� �𼭸����� ��ü�� ��ġ�ؾ���
        for(int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {   
                //x , z ������ ������
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
                Debug.Log("��ġ ���ϱ�");
            }
        }
        return returnVal;
    }

    //��ġ�� ��ġ �������� Ȯ��
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        //Vector3 �� ��ġ ����� ���� (�׸��� ��ġ�� ��ü�� ũ�⸦ �����ϴ� ��ġ�� ���)
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        //������ ��ġ�� �Ͻ� ����
        foreach (var pos in positionToOccupy)
        {
            //Ű�� ���ԵǾ� �ְ� �ش� ��ġ �� �ϳ��� �ִٸ�
            if(placedObjects.ContainsKey(pos))
            {
                Debug.Log("��ġ�Ұ���");
                return false;
            }
            //��ġ�� �� �ִٸ� true;
        }

        Debug.Log("��ġ����");
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        //��ġ�� ��ü�� false�� ���ԵǾ��ִ��� Ȯ��
        if(placedObjects.ContainsKey(gridPosition) == false)
        {
            return -1;
        }
        //�׷��� ������ ��ü�� ��ȯ�� ����
        return placedObjects[gridPosition].PlacedObjectIndex;
    }


    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach(var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

//������ ������ �����ϴ� ������
//������ �����ϴ� ��?
public class PlacementData
{
    //��ġ ����� �ʿ� : ��ü�� �����ϴ� ��ġ
    public List<Vector3Int> occupiedPositions;

    //ID -> �����͸� �����ϰ� �ε��� �� ����
    public int ID { get; private set; }
    //��� ��ü �ε��� -> ��ü ���� �ý��۸��� �� ����
    public int PlacedObjectIndex { get; private set; }

    //������ ����
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
