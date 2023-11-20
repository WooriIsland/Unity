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

    [SerializeField]
    private GameObject planeUI;

    private void Start()
    {
        //배치 중지
        StopPlacement();

        //우리는 미리 설치해놓는다고 치면 new를 하면 안됨
        floorData = new GridData();
        furnitureData = new GridData();

        //(삭제) 자식의 오브젝트의 renderer 를 가져온다.
        //previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //생성한 각 개체 데이터에 ID를 할당했으며 UI에서 호출
    public void StartPlacement(int ID)
    {
        print("셋팅 꺼지게 하기");
        planeUI.SetActive(false);

        print("1 : " + ID + "설치 예정");
        //종료하는 스크립트 실행해야지 놓고나서 삭제 됨
        StopPlacement();

        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, objectPlacer);

        //배치할 위치에 미리 보기 표시
        //클릭시
        inputManager.OnClicked += PlaceStructure;
        //종료시
        inputManager.OnExit += StopPlacement;
    }

    //상태 제거 사용 삭제 하는 부분이예욥
    public void StartRemoving()
    {
        //중지 배치 호출
        StopPlacement();

        planeUI.SetActive(false);

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
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        print("2 : 그리드 설치 중입니다");

        planeUI.SetActive(true);

        //마우스 위치와 그리드 위치계산을 if 확인
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //월드 위치를 Cell위치로 변환
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //시스템 모드를 만들어서 사용할 수 있게 함
        //설치하는 곳(인스턴스하는곳)
        buildingState.OnAction(gridPosition);

        StopPlacement();
    }

    public void StopPlacement()
    {
        if (buildingState == null)
        {
            return;
        }

        print("2_1, 3 : 설치 중지");
        //selectedObjectIndex = -1;

        gridVisualization.SetActive(false);

        //그리드에서 미리보기를 제거하는 로직 호출
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
        if (buildingState == null) //(selectedObjectIndex < 0)
        {
            return; //아무것도 이동하고 싶지 않음
        }

        //선택 위치를 가져옴
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        //월드 위치를 Cell위치로 변환
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //마지막 감지된 위치는 그리드 위치와 다르다면
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);

            //마지막 위치는 그리드 위치와 동일하게 가져감
            lastDetectedPosition = gridPosition;
        }
    }
}
