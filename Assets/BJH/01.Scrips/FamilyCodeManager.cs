using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class FamilyCodeManager : MonoBehaviour
{
    public TMP_InputField inputNickName;
    public TextMeshProUGUI inputFamilyCode;

    public string nickName;
    public string familyCode;

    public void OnClickJoinBtn(string code)
    {
        //int num = UnityEngine.Random.Range(0, 100);
        ////현숙 임시 구현(조건문으로 가야함)
        //nickName = "정이" + num.ToString();
        familyCode = code;

        

        InfoManager.Instance.FamilyCode = code;
        print(InfoManager.Instance.FamilyCode);


        ////지환 구현 
        ///*nickName = inputNickName.text;
        //byte[] a = System.Text.Encoding.UTF8.GetBytes(nickName);
        //familyCode = inputFamilyCode.text;*/

        //PlayerPrefs.SetString("NickName", nickName);
        PlayerPrefs.SetString("FamilyCode", familyCode);

        SceneManager.LoadScene(3);
    }
}
