using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnBoardingManager : MonoBehaviour
{
    public GameObject signUpPage;
    public GameObject sighUpCheckPage;

    // 저장 할 데이터
    // 이메일
    public TextMeshProUGUI emailInput;
    string email;


    private void Start()
    {
        signUpPage.SetActive(false);
        sighUpCheckPage.SetActive(false);

        
        if (PlayerPrefs.GetString("email").Length > 0)
        {
            string savedEmail = PlayerPrefs.GetString("email");

            // 왜 안되는건지 모르겠음;;
            emailInput.text = savedEmail;
            emailInput.SetText(savedEmail);
            print(emailInput.text);
        }
    }

    public void OnClickSignUpBtn()
    {
        signUpPage.SetActive(true);
    }

    public void OnClickCompleteSignUpBtn()
    {
        sighUpCheckPage.SetActive(true);
    }

    public void OnClickCompleteSignUpCheckBtn()
    {
        signUpPage.SetActive(false);
        sighUpCheckPage.SetActive(false);
    }

    public void OnClickLogin()
    {
        email = emailInput.text;
        PlayerPrefs.SetString("email", email);
        print(PlayerPrefs.GetString("email").Length);
       


        if(PlayerPrefs.GetString("FamilyCode").Length > 0)
        {
            // 있으면?
            // 바로 방에 연결하기
            ConnectionManager03.instance.OnClickConnect();
        }
        else
        {
            // 만약 저장된 가족코드가 없으면
            // 가족 코드를 입력하는 씬으로 이동
            SceneManager.LoadScene(1);

        }


    }

}
