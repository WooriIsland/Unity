using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils; //유틸리티 사용

// 그리드 클래스 
public class Grid_LHS
{
    //너비
    int width;
    //높이
    int height;
    //셀 크기
    float cellSize;
    //2차원의 다차원 배열
    int[,] gridArray;
    TextMesh[,] debugTextArray;
    Vector3 originPosition;

    //너비와 높이를 만드는 생성자 , 원점 위치
    public Grid_LHS(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        //전체 배열을 통과하는 사이클 (다차원 순환)
        //arr.GetLength(0) 행의 개수 / arr.GetLength(1) 열의 개수
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            { 
                Debug.Log(x + "," + y);

                //각 셀의 크기 (확인용) -> 칸 중간에 숫자를 써주기
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 30, Color.white, TextAnchor.MiddleCenter);

                //오른쪽 아래에서 시작
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.green, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 100f);
            }
        }
        
        //테두리
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        //SetValue(2, 1, 56);
    }

    //x와 y를 위치로 변환하는 함수 Vector3 반환
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    //마우스 클릭
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    // 값 입력
    public void SetValue(int x, int y, int value)
    {
        //음수 값을 처리하기 위해
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    //마우스 클릭
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }

        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
