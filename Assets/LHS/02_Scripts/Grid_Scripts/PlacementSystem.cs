using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//배치 시스템을 호출
public class PlacementSystem : MonoBehaviour
{
    //마우스 표시
    //[SerializeField]
    //private GameObject mouseIndicator; //cellIndicator -> PreviewSystem에서 사용

    [SerializeField]
    private InputManager inputManager;
    //그리드 -> 다음 셀로 이동할때 이동함 -> 내가 그리드를 직접 그려준다면 ?
    [SerializeField]
    private Grid grid;

    //설치 오브젝트
    [SerializeField]
    private ObjectDatabaseSO database;
    //데이터베이스에서 개체를 선택하지 않은 것.
    //private int selectedObjectIndex = -1;

    //그리드 시각화
    [SerializeField]
    private GameObject gridVisualization;

    //사운드
    [SerializeField]
    private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField]
    private UnityEngine.AudioSource source;

    //바닥 , 가구 Data
    private GridData floorData, furnitureData;

    //(삭제) 미리보기 렌더링
    //private Renderer previewRenderer;

    //게임 개체의 개인 목록을 생성 -> ObjectPlacer스크립터로 이동
    //private List<GameObject> placedGameObjects = new();

    [SerializeField]
    private PreviewSystem preview;

    //마지막 위치 
    //배치 시작 시 배치 미리보기를 생성하는 곳
    //배치를 중지할 때 이 값을 재설정
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {

        //배치 중지
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();

        //(삭제) 자식의 오브젝트의 renderer 를 가져온다.
        //previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //생성한 각 개체 데이터에 ID를 할당했으며 UI에서 호출
    public void StartPlacement(int ID)
    {
        //종료하는 스크립트 실행해야지 놓고나서 삭제 됨
        StopPlacement();

        gridVisualization.SetActive(true);
        //※ 배치 상태 내에서 호출됨
        //FindIndex 메서드를 활용하여 리스트 내에서 측정조건을 충족하는 객체의 인덱스를 찾는 방법 (반환)
        //FindIndex 조건과 일치하는 데이터를 찾지 못하면 -1를 반환
        /*selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        //개체가 없을 시
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        //시각적 표시
        //[삭제]gridVisualization.SetActive(true);
        //[삭제]cellIndicator.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab,
                                             database.objectsData[selectedObjectIndex].Size);*/

        buildingState = new PlacementState(ID,grid,preview, database,floorData,furnitureData,objectPlacer);
        //배치할 위치에 미리 보기 표시
        //클릭시
        inputManager.OnClicked += PlaceStructure;
        //종료시
        inputManager.OnExit += StopPlacement; 
    }

    //상태 제거 사용
    public void StartRemoving()
    {
        //중지 배치 호출
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer);

        //클릭시
        inputManager.OnClicked += PlaceStructure;
        //종료시 
        inputManager.OnExit += StopPlacement;
    }

    //그리드 위에 있을 때 마우스 버튼을 누르면 새 객체를 인스턴스화하고
    //객체의 위치에 배치
    private void PlaceStructure()
    {
        //관리자 위에 UI가 있는지 확인해야함
        if(inputManager.IsPointerOverUI())
        {
            return;
        }


        //마우스 위치와 그리드 위치계산을 if 확인
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //월드 위치를 Cell위치로 변환
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
        #region OnAction 으로 변경됨
        /*//마우스 커서 그려줌
        //mouseIndicator.transform.position = mousePosition;

        //배치가 유효한지 확인
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        //거짓이라면
        if(placementValidity == false)
        {
            return;
        }

        //배치
        //사운드 재생 
        //source.Play();

        //선택한 오브젝트의 index번호를 알아야한다. 
        //프리팹 , 그리드 월드 위치도 전달
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        //[ObjectPlacer로 변경]그 위치에 게임오브젝트 인스턴스화 하기 -> 저장해야 한다. 
        *//*GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        //그리드 위치를 다시 World로 변환 
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);*//*

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID,
            index); //placedGameObjects.Count -1 -> index로 변경

        //모든 것이 배치된 마지막 구조 메소드에 구조를 배치할 때 미리 업데이트 위치를 호출하고 그리드 셀을 월드 그리드 위치로 호출하도록 보장하는 것
        //배치했다면 유효하지 않기 때문에 false로 변경 -> 이미 개체가 배치되어있기 때문에
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);*/
        #endregion
    }

    //마지막 중지 검사 배치유효성을 구현하는 것
/*    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //그리드 데이터를 정의, 선택한 데이터를 정의
        //선택한 인덱스가 0이면 가구 데이터를 반환할 예정
        //실제의 ID가 아닌 인덱스 임으로 안전을 위해 DB
        //바닥 개체가 더 많으면 열거형이나 다른 요소를 구현해야 함
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    } */

    //실제로 개체 배치를 할 수 없도록
    //왜 실행 안됨?
    public void StopPlacement()
    {
        if(buildingState == null)
        {
            return;
        }

        //selectedObjectIndex = -1;

        gridVisualization.SetActive(false);

        //그리드에서 미리보기를 제거하는 로직 호출
        //(삭제)cellIndicator.SetActive(false);
        //preview.StopShowingPreview();
        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        //종료하면 다시 zero에 할당
        lastDetectedPosition = Vector3Int.zero;

        //buildingState도 null로 변경
        buildingState = null;
    }

    private void Update()
    {
        //배치 모드에 있다면 셀 표시기와 마우스 표시기를 이동하여 그리드 위치를 계산한 직후
        //여기에 공간을 생성

        //배치모드에 있지 않을때 숨길 예정임 (선택한 개체 인덱스가 0보다 작은 경우)
        if(buildingState == null) //(selectedObjectIndex < 0)
        {
            return; //아무것도 이동하고 싶지 않음
        }

        //선택 위치를 가져옴
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //월드 위치를 Cell위치로 변환
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //마지막 감지된 위치는 그리드 위치와 다르다면
        if(lastDetectedPosition != gridPosition)
        {
            //※그리기 - UpdateState변경
            //배치가 유효한지 확인
            /*bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            //개체를 배치할 수 있는 여부가 표시
            //(삭제)previewRenderer.material.color = placementValidity ? Color.white : Color.red;

            //마우스 커서 그려줌
            mouseIndicator.transform.position = mousePosition;

            //배치하려는 개체의 미리보기를 표시해야하는 변경사항
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);

            //그리드 위치를 다시 World로 변환 -> 그리드 보여주는 거 PrieviewSystem에서 할것임 
            //(삭제)cellIndicator.transform.position = grid.CellToWorld(gridPosition);*/

            buildingState.UpdateState(gridPosition);

            //마지막 위치는 그리드 위치와 동일하게 가져감
            lastDetectedPosition = gridPosition;
        }
    }
}
