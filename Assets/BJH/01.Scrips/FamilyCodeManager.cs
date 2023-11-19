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
        ////���� �ӽ� ����(���ǹ����� ������)
        //nickName = "����" + num.ToString();
        familyCode = code;

        

        InfoManager.Instance.FamilyCode = code;
        print(InfoManager.Instance.FamilyCode);


        ////��ȯ ���� 
        ///*nickName = inputNickName.text;
        //byte[] a = System.Text.Encoding.UTF8.GetBytes(nickName);
        //familyCode = inputFamilyCode.text;*/

        //PlayerPrefs.SetString("NickName", nickName);
        PlayerPrefs.SetString("FamilyCode", familyCode);

        SceneManager.LoadScene(3);
    }
}
