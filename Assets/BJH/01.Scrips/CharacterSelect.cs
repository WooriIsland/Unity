using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviourPun
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
                InfoManager.Instance.Character = hit.collider.name;
                ChatManager.Instance.dicAllPlayerProfile[photonView.Owner.NickName] = hit.collider.name;
                print($"���õ� �÷��̾� {InfoManager.Instance.Character}");

            }
        }
    }
}
