using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public int idx;

    private void OnMouseUpAsButton()
    {
        print("ĳ���� Ŭ��");
        CharacterManager._characterManager.currentCharacterIdx = idx;
    }
}
