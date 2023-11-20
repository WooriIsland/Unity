using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private void Update()
    {
        // 클릭하면
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 선택한 플레이어 정보를 CharacterManager의 CurrentCharacterName에 저장한다.
            if (Physics.Raycast(ray, out hit))
            {
                CharacterManager.instance.currentCharacterName = hit.collider.name;
            }
        }
    }
}
