using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�̸����� �ý���
public class PreviewSystem : MonoBehaviour
{
    //��ġ�� ������Ʈ�� �׸��� ���� �� �� �ְ� 
    //�ܵ� ����̳� �׸��� �Ǵ� �� ǥ�ñ⸦ ������� �ʴ´�.
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    //�̸����� ��ü
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    //�ν��Ͻ� �̸�����
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    void Start()
    {
        //���� ������ ���͸����� �̸����� ��Ƽ������ �����Ѵ�.
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        //��Ȱ��ȭ �����ش�.
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //���۽� ������
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreavie(previewObject);
        //ǥ�ñ��� �̸����⸦ �غ�
        PrepareCursor(size);
        //Ŀ���� Ȱ��ȭ�Ѵ�.
        cellIndicator.SetActive(true);
    }

    //��ü�� ũ�⿡ ���� �ٴ� ����
    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x * 3, 1, size.y *3);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    //�̸������� ���͸����� ���� �ٲ��� ��
    //������! ���� ���͸��� ���� ���� �� 
    private void PreparePreavie(GameObject previewObject)
    {
        //������ �迭�� ������ְ� 
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

        //��������͸���� �ٲ��ֱ�
        foreach(Renderer renderer in renderers)
        {
            Material[] mat = renderer.materials;
            for(int i = 0; i < mat.Length; i++)
            {
                mat[i] = previewMaterialInstance;
            }
            renderer.materials = mat;
        }
    }

    //ǥ�� ����
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        
        //�ı��� �̸����⵵ ����
        if(previewObject != null)
        {
            //������Ʈ ����
            Destroy(previewObject);
        }
    }

    //��ġ �޼ҵ�
    public void UpdatePosition(Vector3 position, bool validity)
    {
        //�̸����� ��ü�� null�� �ƴ϶��
        if(previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    //������ ������ �����
    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        //��ġ ������ ����
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    public void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
         
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
