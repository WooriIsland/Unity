using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // 현재 선택된 캐릭터 idx
    public int currentCharacterIdx;

    // 현재 선택된 캐릭터 이름
    public string currentCharacterName;

    // 인스턴스
    public static CharacterManager _characterManager;

    private void Awake()
    {
        if(_characterManager == null)
        {
            _characterManager = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // 선택된 플레이어가 있다면?
        if(currentCharacterIdx != null)
        {
            // 플레이어 인덱스 저장
            PlayerPrefs.SetInt("CurrentCharacterIdx", currentCharacterIdx);
        }
    }


}
