using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    //선택된 개체
    private int gameObjectIndex = -1;
    //그리드
    Grid grid;
    //미리보기 시스템
    PreviewSystem previewSystem;
    //그리드 데이터
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

        //특정셀에서 거짓인지 확인하고 개체를 배치할 수 없는지 확인
        if(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            //여기에 무언가가 있다는 의미
            //선택한 데이터를 동일하게 설정
            selectedData = furnitureData;
        }
        else if(floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }
        //찾지 못한 경우
        if(selectedData == null)
        {
            //sound
            //제거할 것이 없다고 사용자에게 알리기

        }

        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

            if(gameObjectIndex == -1)
            {
                return;
            }

            //그렇지 않은 경우는 삭제해야함
            soundFeedback.PlaySound(SoundType.Remove);
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        //Vector3 셀 위치 가져오기
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        //false -> 바닥타일을 실제로 확인
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        //검사 선택 반환에서 여기로 돌아갈 것인지 확인하고 가구 데이터를 전달
        //참이 아닐때
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
