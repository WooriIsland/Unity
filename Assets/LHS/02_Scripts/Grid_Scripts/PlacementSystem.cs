using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {

        //��ġ ����
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();

        //(����) �ڽ��� ������Ʈ�� renderer �� �����´�.
        //previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //������ �� ��ü �����Ϳ� ID�� �Ҵ������� UI���� ȣ��
    public void StartPlacement(int ID)
    {
        //�����ϴ� ��ũ��Ʈ �����ؾ��� ������ ���� ��
        StopPlacement();

        gridVisualization.SetActive(true);
        //�� ��ġ ���� ������ ȣ���
        //FindIndex �޼��带 Ȱ���Ͽ� ����Ʈ ������ ���������� �����ϴ� ��ü�� �ε����� ã�� ��� (��ȯ)
        //FindIndex ���ǰ� ��ġ�ϴ� �����͸� ã�� ���ϸ� -1�� ��ȯ
        /*selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        //��ü�� ���� ��
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        //�ð��� ǥ��
        //[����]gridVisualization.SetActive(true);
        //[����]cellIndicator.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab,
                                             database.objectsData[selectedObjectIndex].Size);*/

        buildingState = new PlacementState(ID,grid,preview, database,floorData,furnitureData,objectPlacer);
        //��ġ�� ��ġ�� �̸� ���� ǥ��
        //Ŭ����
        inputManager.OnClicked += PlaceStructure;
        //�����
        inputManager.OnExit += StopPlacement; 
    }

    //���� ���� ���
    public void StartRemoving()
    {
        //���� ��ġ ȣ��
        StopPlacement();
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
        if(inputManager.IsPointerOverUI())
        {
            return;
        }


        //���콺 ��ġ�� �׸��� ��ġ����� if Ȯ��
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //���� ��ġ�� Cell��ġ�� ��ȯ
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
        #region OnAction ���� �����
        /*//���콺 Ŀ�� �׷���
        //mouseIndicator.transform.position = mousePosition;

        //��ġ�� ��ȿ���� Ȯ��
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        //�����̶��
        if(placementValidity == false)
        {
            return;
        }

        //��ġ
        //���� ��� 
        //source.Play();

        //������ ������Ʈ�� index��ȣ�� �˾ƾ��Ѵ�. 
        //������ , �׸��� ���� ��ġ�� ����
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        //[ObjectPlacer�� ����]�� ��ġ�� ���ӿ�����Ʈ �ν��Ͻ�ȭ �ϱ� -> �����ؾ� �Ѵ�. 
        *//*GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        //�׸��� ��ġ�� �ٽ� World�� ��ȯ 
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);*//*

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID,
            index); //placedGameObjects.Count -1 -> index�� ����

        //��� ���� ��ġ�� ������ ���� �޼ҵ忡 ������ ��ġ�� �� �̸� ������Ʈ ��ġ�� ȣ���ϰ� �׸��� ���� ���� �׸��� ��ġ�� ȣ���ϵ��� �����ϴ� ��
        //��ġ�ߴٸ� ��ȿ���� �ʱ� ������ false�� ���� -> �̹� ��ü�� ��ġ�Ǿ��ֱ� ������
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);*/
        #endregion
    }

    //������ ���� �˻� ��ġ��ȿ���� �����ϴ� ��
/*    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //�׸��� �����͸� ����, ������ �����͸� ����
        //������ �ε����� 0�̸� ���� �����͸� ��ȯ�� ����
        //������ ID�� �ƴ� �ε��� ������ ������ ���� DB
        //�ٴ� ��ü�� �� ������ �������̳� �ٸ� ��Ҹ� �����ؾ� ��
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    } */

    //������ ��ü ��ġ�� �� �� ������
    //�� ���� �ȵ�?
    public void StopPlacement()
    {
        if(buildingState == null)
        {
            return;
        }

        //selectedObjectIndex = -1;

        gridVisualization.SetActive(false);

        //�׸��忡�� �̸����⸦ �����ϴ� ���� ȣ��
        //(����)cellIndicator.SetActive(false);
        //preview.StopShowingPreview();
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
        if(buildingState == null) //(selectedObjectIndex < 0)
        {
            return; //�ƹ��͵� �̵��ϰ� ���� ����
        }

        //���� ��ġ�� ������
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //���� ��ġ�� Cell��ġ�� ��ȯ
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //������ ������ ��ġ�� �׸��� ��ġ�� �ٸ��ٸ�
        if(lastDetectedPosition != gridPosition)
        {
            //�ر׸��� - UpdateState����
            //��ġ�� ��ȿ���� Ȯ��
            /*bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            //��ü�� ��ġ�� �� �ִ� ���ΰ� ǥ��
            //(����)previewRenderer.material.color = placementValidity ? Color.white : Color.red;

            //���콺 Ŀ�� �׷���
            mouseIndicator.transform.position = mousePosition;

            //��ġ�Ϸ��� ��ü�� �̸����⸦ ǥ���ؾ��ϴ� �������
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);

            //�׸��� ��ġ�� �ٽ� World�� ��ȯ -> �׸��� �����ִ� �� PrieviewSystem���� �Ұ��� 
            //(����)cellIndicator.transform.position = grid.CellToWorld(gridPosition);*/

            buildingState.UpdateState(gridPosition);

            //������ ��ġ�� �׸��� ��ġ�� �����ϰ� ������
            lastDetectedPosition = gridPosition;
        }
    }
}
