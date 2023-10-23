using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//배치상태머신
public class PlacementState : IBuildingState
{
    //선택된 개체
    private int selectedObjectIndex = -1;
    //ID
    int ID;
    //그리드
    Grid grid;
    GridLayout gridLayout;
    //미리보기 시스템
    PreviewSystem previewSystem;
    //데이터베이스
    ObjectDatabaseSO database;
    //그리드 데이터
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    //생성자 -> 우리가 플레이할 ID가져옴
    public PlacementState(int iD,
                          Grid grid,
                          GridLayout gridLayout,
                          PreviewSystem previewSystem,
                          ObjectDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.gridLayout = gridLayout;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        //※ 배치 상태 내에서 호출됨 StartPlacement()
        //FindIndex 메서드를 활용하여 리스트 내에서 측정조건을 충족하는 객체의 인덱스를 찾는 방법 (반환)
        //FindIndex 조건과 일치하는 데이터를 찾지 못하면 -1를 반환
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        //개체가 없을 시
        /*if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }*/

        //개체 데이터 목록에서 ID를 인덱스로 찾지 못하면 찾기 인덱스가 -1를 반환
        //조건 변경
        //찾았을시
        if (selectedObjectIndex > -1)
        {
            //[삭제]시각적 표시
            //gridVisualization.SetActive(true);
            //cellIndicator.SetActive(true);
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab,
                                                 database.objectsData[selectedObjectIndex].Size);
        }

        //찾지 못했을 시
        else
        {
            Debug.Log($"찾지 못했다{iD}");
        }
    }

    //최종상태에서 객체 배치를 추가
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    //마우스 버튼을 누를때마다 발생하는 Action
    //장소 구조 방법 내부의 배치 시스템과 동일 PlaceStructure()
    public void OnAction(Vector3Int gridPosition)
    {
        //배치가 유효한지 확인
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        //거짓이라면
        if (placementValidity == false)
        {
            return;
        }

        //배치
        //사운드 재생 
        //source.Play();

        //선택한 오브젝트의 index번호를 알아야한다. 
        //프리팹 , 그리드 월드 위치도 전달objecrPlacer
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, gridLayout.CellToWorld(gridPosition));

        //[ObjectPlacer로 변경]그 위치에 게임오브젝트 인스턴스화 하기 -> 저장해야 한다. 
        /*GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        //그리드 위치를 다시 World로 변환 
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);*/

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID,
            index); //placedGameObjects.Count -1 -> index로 변경

        //모든 것이 배치된 마지막 구조 메소드에 구조를 배치할 때 미리 업데이트 위치를 호출하고 그리드 셀을 월드 그리드 위치로 호출하도록 보장하는 것
        //배치했다면 유효하지 않기 때문에 false로 변경 -> 이미 개체가 배치되어있기 때문에
        previewSystem.UpdatePosition(gridLayout.CellToWorld(gridPosition), false);
    }

    //마지막 중지 검사 배치유효성을 구현하는 것
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //그리드 데이터를 정의, 선택한 데이터를 정의
        //선택한 인덱스가 0이면 가구 데이터를 반환할 예정
        //실제의 ID가 아닌 인덱스 임으로 안전을 위해 DB
        //바닥 개체가 더 많으면 열거형이나 다른 요소를 구현해야 함
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    //마지막으로 감지된 위치가 그리드 위치에서 다른 경우 배치 유효성을 계산하고 미리보기를 업데이트 -> 마지막 위치 업데이트
    public void UpdateState(Vector3Int gridPosition)
    {
        //※그리기
        //배치가 유효한지 확인
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        //개체를 배치할 수 있는 여부가 표시
        //(삭제)previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        //[삭제]마우스 커서 그려줌 -> 필요없음
        //mouseIndicator.transform.position = mousePosition;

        //배치하려는 개체의 미리보기를 표시해야하는 변경사항
        previewSystem.UpdatePosition(gridLayout.CellToWorld(gridPosition), placementValidity);
    }
}
