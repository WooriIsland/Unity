using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private void Update()
    {
        // Ŭ���ϸ�
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ������ �÷��̾� ������ CharacterManager�� CurrentCharacterName�� �����Ѵ�.
            if (Physics.Raycast(ray, out hit))
            {
                CharacterManager.instance.currentCharacterName = hit.collider.name;
            }
        }
    }
}
