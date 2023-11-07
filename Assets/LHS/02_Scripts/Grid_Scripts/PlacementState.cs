using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ġ���¸ӽ�
public class PlacementState : IBuildingState
{
    //���õ� ��ü
    private int selectedObjectIndex = -1;
    //ID
    int ID;
    //�׸���
    Grid grid;
    //�̸����� �ý���
    PreviewSystem previewSystem;
    //�����ͺ��̽�
    ObjectDatabaseSO database;
    //�׸��� ������
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    //������ -> �츮�� �÷����� ID������
    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        //�� ��ġ ���� ������ ȣ��� StartPlacement()
        //FindIndex �޼��带 Ȱ���Ͽ� ����Ʈ ������ ���������� �����ϴ� ��ü�� �ε����� ã�� ��� (��ȯ)
        //FindIndex ���ǰ� ��ġ�ϴ� �����͸� ã�� ���ϸ� -1�� ��ȯ
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        //��ü ������ ��Ͽ��� ID�� �ε����� ã�� ���ϸ� ã�� �ε����� -1�� ��ȯ
        //���� ����
        //ã������
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab,
                                                 database.objectsData[selectedObjectIndex].Size);
        }

        //ã�� ������ ��
        else
        {
            Debug.Log($"ã�� ���ߴ�{iD}");
        }
    }

    //�������¿��� ��ü ��ġ�� �߰�
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    //���콺 ��ư�� ���������� �߻��ϴ� Action
    //��� ���� ��� ������ ��ġ �ý��۰� ���� PlaceStructure()
    public void OnAction(Vector3Int gridPosition)
    {
        //��ġ�� ��ȿ���� Ȯ��
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        //�����̶��
        if (placementValidity == false)
        {
            return;
        }

        //��ġ
        //���� ��� 
        //source.Play();

        //������ ������Ʈ�� index��ȣ�� �˾ƾ��Ѵ�. 
        //������ , �׸��� ���� ��ġ�� ����objecrPlacer
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID,
            index); //placedGameObjects.Count -1 -> index�� ����

        //��� ���� ��ġ�� ������ ���� �޼ҵ忡 ������ ��ġ�� �� �̸� ������Ʈ ��ġ�� ȣ���ϰ� �׸��� ���� ���� �׸��� ��ġ�� ȣ���ϵ��� �����ϴ� ��
        //��ġ�ߴٸ� ��ȿ���� �ʱ� ������ false�� ���� -> �̹� ��ü�� ��ġ�Ǿ��ֱ� ������
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    //������ ���� �˻� ��ġ��ȿ���� �����ϴ� ��
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //�׸��� �����͸� ����, ������ �����͸� ����
        //������ �ε����� 0�̸� ���� �����͸� ��ȯ�� ����
        //������ ID�� �ƴ� �ε��� ������ ������ ���� DB
        //�ٴ� ��ü�� �� ������ �������̳� �ٸ� ��Ҹ� �����ؾ� ��
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    //���������� ������ ��ġ�� �׸��� ��ġ���� �ٸ� ��� ��ġ ��ȿ���� ����ϰ� �̸����⸦ ������Ʈ -> ������ ��ġ ������Ʈ
    public void UpdateState(Vector3Int gridPosition)
    {
        //�ر׸���
        //��ġ�� ��ȿ���� Ȯ��
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        //��ġ�Ϸ��� ��ü�� �̸����⸦ ǥ���ؾ��ϴ� �������
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
