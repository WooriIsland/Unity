using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    // right, left, ok 버튼
    public Button right, left, ok;

    // ray를 쏠 카메라
    public Camera camera;

    // 돌아가는 판데기
    public Transform ground;

    // 판데기 회전값
    float y = 56f;
    float angle = 14f;

    // ray 맞았던 오브젝트
    GameObject isHitObject;

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 선택한 플레이어 정보를 CharacterManager의 CurrentCharacterName에 저장한다.
        if (Physics.Raycast(ray, out hit))
        {
            if(isHitObject != null && isHitObject != hit.collider.gameObject)
            {
                // ray에 걸린 오브젝트가 변경되었으므로    
                // isHitObject의 Animator를 끈다.
                isHitObject.GetComponent<Animator>().SetFloat("Speed", 0);
                isHitObject.GetComponent<Outline>().OutlineWidth = 0;
            }
            isHitObject = hit.collider.gameObject;
            hit.collider.gameObject.GetComponent<Animator>().SetFloat("Speed", 1);
            hit.collider.gameObject.GetComponent<Outline>().OutlineWidth = 20;
        }
    }



    public void OnClick_RightBtn()
    {
        y += angle;
        ground.rotation = Quaternion.Euler(0, y, 0);
    }

    public void OnClick_LeftBtn()
    {
        y -= angle;
        ground.rotation = Quaternion.Euler(0, y, 0);
    }

    public void OnClick_OKBtn()
    {
        InfoManager.Instance.Character = isHitObject.name;
        SceneManager.LoadScene(4);
    }

}
