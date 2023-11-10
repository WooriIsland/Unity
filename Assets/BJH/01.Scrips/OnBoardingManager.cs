using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnBoardingManager : MonoBehaviour
{
    public Button loginBtn;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public GameObject signUpPage;
    public GameObject sighUpCheckPage;

    // 저장 할 데이터
    // 이메일
    string email;

    ConnectionManager03 cm3;

    private void Start()
    {
        loginBtn.interactable = false;
        signUpPage.SetActive(false);
        sighUpCheckPage.SetActive(false);

        cm3 = GameObject.Find("ConnectionManager03").GetComponent<ConnectionManager03>();
        
        if (PlayerPrefs.GetString("email").Length > 0)
        {
            print(PlayerPrefs.GetString("email"));
            string savedEmail = PlayerPrefs.GetString("email");

            emailInput.text = savedEmail;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("로컬 저장 정보를 모두 삭제했습니다.");
            PlayerPrefs.DeleteAll();
        }

        if ((emailInput.text.Length > 0))
        {
            passwordInput.onValueChanged.AddListener((string s) =>
            {
                if (s.Length > 0)
                {
                    loginBtn.interactable = true;
                }
                else
                {
                    loginBtn.interactable = false;
                }
            });
        }
        else
        {
            emailInput.onValueChanged.AddListener((string s) =>
            {
                if (s.Length > 0)
                {
                    passwordInput.onValueChanged.AddListener((string s) =>
                    {
                        if (s.Length > 0)
                        {
                            loginBtn.interactable = true;
                        }
                        else
                        {
                            loginBtn.interactable = false;

                        }
                    });
                }
                else
                {
                    loginBtn.interactable = false;
                }
            });
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
            cm3.OnClickConnect();
            
        }
        else
        {
            // 만약 저장된 가족코드가 없으면
            // 가족 코드를 입력하는 씬으로 이동
            SceneManager.LoadScene(1);

        }


    }

}
