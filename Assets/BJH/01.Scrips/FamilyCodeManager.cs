using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class FamilyCodeManager : MonoBehaviour
{
    public TextMeshProUGUI inputNickName;
    public TextMeshProUGUI inputFamilyCode;

    public string nickName;
    public string familyCode;

    private void Start()
    {

    }

    public void OnClickJoinBtn()
    {
        nickName = inputNickName.text;
        familyCode = inputFamilyCode.text;

        PlayerPrefs.SetString("NickName", nickName);
        PlayerPrefs.SetString("FamilyCode", familyCode);

        SceneManager.LoadScene(2);
    }
}
