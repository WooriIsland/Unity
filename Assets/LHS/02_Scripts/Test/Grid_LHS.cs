using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils; //��ƿ��Ƽ ���

// �׸��� Ŭ���� 
public class Grid_LHS
{
    //�ʺ�
    int width;
    //����
    int height;
    //�� ũ��
    float cellSize;
    //2������ ������ �迭
    int[,] gridArray;
    TextMesh[,] debugTextArray;
    Vector3 originPosition;

    //�ʺ�� ���̸� ����� ������ , ���� ��ġ
    public Grid_LHS(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        //��ü �迭�� ����ϴ� ����Ŭ (������ ��ȯ)
        //arr.GetLength(0) ���� ���� / arr.GetLength(1) ���� ����
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            { 
                Debug.Log(x + "," + y);

                //�� ���� ũ�� (Ȯ�ο�) -> ĭ �߰��� ���ڸ� ���ֱ�
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 30, Color.white, TextAnchor.MiddleCenter);

                //������ �Ʒ����� ����
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.green, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 100f);
            }
        }
        
        //�׵θ�
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        //SetValue(2, 1, 56);
    }

    //x�� y�� ��ġ�� ��ȯ�ϴ� �Լ� Vector3 ��ȯ
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    //���콺 Ŭ��
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    // �� �Է�
    public void SetValue(int x, int y, int value)
    {
        //���� ���� ó���ϱ� ����
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    //���콺 Ŭ��
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
