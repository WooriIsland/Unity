using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    // right, left, ok ��ư
    public Button right, left, ok;

    // ray�� �� ī�޶�
    public Camera camera;

    // ���ư��� �ǵ���
    public Transform ground;

    // �ǵ��� ȸ����
    float y = 56f;
    float angle = 14f;

    // ray �¾Ҵ� ������Ʈ
    GameObject isHitObject;

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ������ �÷��̾� ������ CharacterManager�� CurrentCharacterName�� �����Ѵ�.
        if (Physics.Raycast(ray, out hit))
        {
            if(isHitObject != null && isHitObject != hit.collider.gameObject)
            {
                // ray�� �ɸ� ������Ʈ�� ����Ǿ����Ƿ�    
                // isHitObject�� Animator�� ����.
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
