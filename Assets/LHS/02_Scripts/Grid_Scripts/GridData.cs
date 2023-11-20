using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//배치된 객체에 대한 데이터를 저장할 방법 필요
public class GridData
{
    //딕셔너리 키, 값(배치 데이터)
    //C#9을 지원 해당 객체를 생성할 때 반복하지 않고 new를 입력하여 더 적은 입력을 할 수 있으며
    //배치 데이터를 호출하거나 배치된 객체를 배치해 볼 수 있음
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    //객체 추가를 호출 (그리드 위치, 사이즈, ID, object 인덱스)
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {

        //List<Vector3Int> positionToOccupy = new List<Vector3Int>();
        //그리드 위치 , 사이즈 (위치계산)
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        //데이터와 동일하며 여기에 ID를 차지할 위치 전달 
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);

        foreach(var pos in positionToOccupy)
        {
            //배치된 객체의 항목이 포함되어 있는지 확인
            if(placedObjects.ContainsKey(pos))
            {
                //새 예외 발생
                Debug.Log("이미 자리에 있습니다.");
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }

            //사전에 처리 , 위치 데이터가 같으므로
            placedObjects[pos] = data;
        }
    }

    //위치 계산
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        //※회전
        //오프셋 거리를 얻어야 함
        //객체를 회전하려면 왼쪽 아래 모서리에서 객체를 배치해야함
        for(int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {   
                //x , z 값으로 더해줌
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
                Debug.Log("설치 더하기");
            }
        }
        return returnVal;
    }

    //위치에 배치 가능한지 확인
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        //Vector3 끝 위치 목록을 생성 (그리드 위치와 객체의 크기를 전달하는 위치를 계산)
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        //점유할 위치에 일시 중지
        foreach (var pos in positionToOccupy)
        {
            //키가 포함되어 있고 해당 위치 중 하나가 있다면
            if(placedObjects.ContainsKey(pos))
            {
                Debug.Log("배치불가능");
                return false;
            }
            //배치할 수 있다면 true;
        }

        Debug.Log("배치가능");
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        //배치된 객체에 false가 포함되어있는지 확인
        if(placedObjects.ContainsKey(gridPosition) == false)
        {
            return -1;
        }
        //그렇지 않으면 객체를 반환할 것임
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

//사전의 값으로 저장하는 데이터
//서버에 저장하는 값?
public class PlacementData
{
    //위치 목록이 필요 : 개체가 차지하는 위치
    public List<Vector3Int> occupiedPositions;

    //ID -> 데이터를 저장하고 로드할 때 유용
    public int ID { get; private set; }
    //장소 객체 인덱스 -> 개체 제거 시스템만들 때 유용
    public int PlacedObjectIndex { get; private set; }

    //생성자 생성
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
