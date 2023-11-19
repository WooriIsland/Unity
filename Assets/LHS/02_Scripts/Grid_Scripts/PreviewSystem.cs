using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//미리보기 시스템
public class PreviewSystem : MonoBehaviour
{
    //배치할 오브젝트를 그리드 위에 볼 수 있게 
    //잔디 평면이나 그리드 또는 셀 표시기를 통과하지 않는다.
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    //미리보기 객체
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    //인스턴스 미리보기
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    void Start()
    {
        //새로 생성될 메터리얼은 미리보기 메티리얼을 생성한다.
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        //비활성화 시켜준다.
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //시작시 프리팹
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreavie(previewObject);
        //표시기의 미리보기를 준비
        PrepareCursor(size);
        //커서를 활성화한다.
        cellIndicator.SetActive(true);
    }

    //개체의 크기에 따른 바닥 구현
    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x * 3, 1, size.y *3);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    //미리보기의 메터리얼의 색을 바꿔줄 것
    //문제점! 같은 메터리얼 색을 썼을 시 
    private void PreparePreavie(GameObject previewObject)
    {
        //렌더러 배열로 만들어주고 
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

        //프리뷰메터리얼로 바꿔주기
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

    //표시 중지
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        
        //파괴할 미리보기도 삭제
        if(previewObject != null)
        {
            //오브젝트 삭제
            Destroy(previewObject);
        }
    }

    //위치 메소드
    public void UpdatePosition(Vector3 position, bool validity)
    {
        //미리보기 개체가 null이 아니라면
        if(previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    //동일한 색상을 만들기
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
        //위치 점들을 전달
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    public void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
         
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
