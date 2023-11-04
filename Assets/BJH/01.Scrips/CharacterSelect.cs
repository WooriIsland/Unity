using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public int idx;

    private void OnMouseUpAsButton()
    {
        print("캐릭터 클릭");
        CharacterManager._characterManager.currentCharacterIdx = idx;
    }
}
