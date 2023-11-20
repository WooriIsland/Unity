using UnityEngine;

//��ġ �ý����� ȣ��
public class PlacementSystem : MonoBehaviour
{
    //���콺 ǥ��
    //[SerializeField]
    //private GameObject mouseIndicator; //cellIndicator -> PreviewSystem���� ���

    [SerializeField]
    private InputManager inputManager;
    //�׸��� -> ���� ���� �̵��Ҷ� �̵��� -> ���� �׸��带 ���� �׷��شٸ� ?
    [SerializeField]
    private Grid grid;

    //��ġ ������Ʈ
    [SerializeField]
    private ObjectDatabaseSO database;
    //�����ͺ��̽����� ��ü�� �������� ���� ��.
    //private int selectedObjectIndex = -1;

    //�׸��� �ð�ȭ
    [SerializeField]
    private GameObject gridVisualization;

    //����
    [SerializeField]
    private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField]
    private UnityEngine.AudioSource source;

    //�ٴ� , ���� Data
    private GridData floorData, furnitureData;

    //(����) �̸����� ������
    //private Renderer previewRenderer;

    //���� ��ü�� ���� ����� ���� -> ObjectPlacer��ũ���ͷ� �̵�
    //private List<GameObject> placedGameObjects = new();

    [SerializeField]
    private PreviewSystem preview;

    //������ ��ġ 
    //��ġ ���� �� ��ġ �̸����⸦ �����ϴ� ��
    //��ġ�� ������ �� �� ���� �缳��
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    [SerializeField]
    private GameObject planeUI;

    private void Start()
    {
        //��ġ ����
        StopPlacement();

        //�츮�� �̸� ��ġ�س��´ٰ� ġ�� new�� �ϸ� �ȵ�
        floorData = new GridData();
        furnitureData = new GridData();

        //(����) �ڽ��� ������Ʈ�� renderer �� �����´�.
        //previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //������ �� ��ü �����Ϳ� ID�� �Ҵ������� UI���� ȣ��
    public void StartPlacement(int ID)
    {
        print("���� ������ �ϱ�");
        planeUI.SetActive(false);

        print("1 : " + ID + "��ġ ����");
        //�����ϴ� ��ũ��Ʈ �����ؾ��� ������ ���� ��
        StopPlacement();

        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID, grid, preview, database, floorData, furnitureData, objectPlacer);

        //��ġ�� ��ġ�� �̸� ���� ǥ��
        //Ŭ����
        inputManager.OnClicked += PlaceStructure;
        //�����
        inputManager.OnExit += StopPlacement;
    }

    //���� ���� ��� ���� �ϴ� �κ��̿���
    public void StartRemoving()
    {
        //���� ��ġ ȣ��
        StopPlacement();

        planeUI.SetActive(false);

        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer);

        //Ŭ����
        inputManager.OnClicked += PlaceStructure;
        //����� 
        inputManager.OnExit += StopPlacement;
    }

    //�׸��� ���� ���� �� ���콺 ��ư�� ������ �� ��ü�� �ν��Ͻ�ȭ�ϰ�
    //��ü�� ��ġ�� ��ġ
    private void PlaceStructure()
    {
        //������ ���� UI�� �ִ��� Ȯ���ؾ���
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        print("2 : �׸��� ��ġ ���Դϴ�");

        planeUI.SetActive(true);

        //���콺 ��ġ�� �׸��� ��ġ����� if Ȯ��
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //���� ��ġ�� Cell��ġ�� ��ȯ
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //�ý��� ��带 ���� ����� �� �ְ� ��
        //��ġ�ϴ� ��(�ν��Ͻ��ϴ°�)
        buildingState.OnAction(gridPosition);

        StopPlacement();
    }

    public void StopPlacement()
    {
        if (buildingState == null)
        {
            return;
        }

        print("2_1, 3 : ��ġ ����");
        //selectedObjectIndex = -1;

        gridVisualization.SetActive(false);

        //�׸��忡�� �̸����⸦ �����ϴ� ���� ȣ��
        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        //�����ϸ� �ٽ� zero�� �Ҵ�
        lastDetectedPosition = Vector3Int.zero;

        //buildingState�� null�� ����
        buildingState = null;
    }

    private void Update()
    {
        //��ġ ��忡 �ִٸ� �� ǥ�ñ�� ���콺 ǥ�ñ⸦ �̵��Ͽ� �׸��� ��ġ�� ����� ����
        //���⿡ ������ ����

        //��ġ��忡 ���� ������ ���� ������ (������ ��ü �ε����� 0���� ���� ���)
        if (buildingState == null) //(selectedObjectIndex < 0)
        {
            return; //�ƹ��͵� �̵��ϰ� ���� ����
        }

        //���� ��ġ�� ������
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        //���� ��ġ�� Cell��ġ�� ��ȯ
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //������ ������ ��ġ�� �׸��� ��ġ�� �ٸ��ٸ�
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);

            //������ ��ġ�� �׸��� ��ġ�� �����ϰ� ������
            lastDetectedPosition = gridPosition;
        }
    }
}
