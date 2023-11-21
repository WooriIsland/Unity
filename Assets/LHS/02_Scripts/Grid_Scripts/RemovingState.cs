using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    //���õ� ��ü
    private int gameObjectIndex = -1;
    //�׸���
    Grid grid;
    //�̸����� �ý���
    PreviewSystem previewSystem;
    //�׸��� ������
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;
    SoundFeedback soundFeedback;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;

        previewSystem.StartShowingRemovePreview();
    }
     
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;

        //Ư�������� �������� Ȯ���ϰ� ��ü�� ��ġ�� �� ������ Ȯ��
        if(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            //���⿡ ���𰡰� �ִٴ� �ǹ�
            //������ �����͸� �����ϰ� ����
            selectedData = furnitureData;
        }
        else if(floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }
        //ã�� ���� ���
        if(selectedData == null)
        {
            //sound
            //������ ���� ���ٰ� ����ڿ��� �˸���

        }

        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

            if(gameObjectIndex == -1)
            {
                return;
            }

            //�׷��� ���� ���� �����ؾ���
            soundFeedback.PlaySound(SoundType.Remove);
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        //Vector3 �� ��ġ ��������
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        //false -> �ٴ�Ÿ���� ������ Ȯ��
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        //�˻� ���� ��ȯ���� ����� ���ư� ������ Ȯ���ϰ� ���� �����͸� ����
        //���� �ƴҶ�
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
