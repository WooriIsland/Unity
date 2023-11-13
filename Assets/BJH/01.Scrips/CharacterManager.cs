using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int currentCharacterIdx;

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
        if(currentCharacterIdx != null)
        {
            PlayerPrefs.SetInt("CurrentCharacterIdx", currentCharacterIdx);
            //print(currentCharacterIdx);
        }
    }


}
