using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // ���� ���õ� ĳ���� idx
    public int currentCharacterIdx;

    // ���� ���õ� ĳ���� �̸�
    public string currentCharacterName;

    // �ν��Ͻ�
    public static CharacterManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // ���õ� �÷��̾ �ִٸ�?
        if(currentCharacterIdx != null)
        {
            // �÷��̾� �ε��� ����
            PlayerPrefs.SetInt("CurrentCharacterIdx", currentCharacterIdx);
        }

        if(currentCharacterName != null)
        {
            Managers.Info.Character = currentCharacterName;
        }
    }


}
