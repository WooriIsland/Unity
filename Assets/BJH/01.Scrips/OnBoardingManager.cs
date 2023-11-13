using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnBoardingManager : MonoBehaviour
{
    public TMP_InputField id, pw;
    public GameObject startBG, completeLoginBoxEmpty, checkBox;

    public Button loginBtn;

    public GameObject signUpPage;
    public GameObject sighUpCheckPage;

    // 저장 할 데이터
    // 이메일
    string email;

    ConnectionManager03 cm3;
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        loginBtn.interactable = false;

        cm3 = GameObject.Find("ConnectionManager03").GetComponent<ConnectionManager03>();
        
        if (PlayerPrefs.GetString("email").Length > 0)
        {
            string savedEmail = PlayerPrefs.GetString("email");

            id.text = savedEmail;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("로컬 저장 정보를 모두 삭제했습니다.");
            PlayerPrefs.DeleteAll();
        }

        

        if ((id.text.Length > 0))
        {
            pw.onValueChanged.AddListener((string s) =>
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
            pw.onValueChanged.AddListener((string s) =>
            {
                if (s.Length > 0)
                {
                    pw.onValueChanged.AddListener((string s) =>
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

    public void OnClickStartBG()
    {
        startBG.SetActive(false);
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

    public void OnClickNextSceneBtn()
    {
        email = id.text;
        PlayerPrefs.SetString("email", email);
        print(PlayerPrefs.GetString("email").Length);
       


        if(PlayerPrefs.GetString("FamilyCode").Length > 0)
        {
            // 있으면?
            // 바로 방에 연결하기
            cm3.ConnectRequest();
            
        }
        else
        {
            // 만약 저장된 가족코드가 없으면
            // 가족 코드를 입력하는 씬으로 이동
            SceneManager.LoadScene(1);

        }


    }

    // 로그인을 할지 회원가입을 할지 판별해주는 함수

    public void OnClickNextBtn()
    {
        // 서버와 통신하여 아이디가 존재하는지 확인

        // 아이디가 존재한다면?
        // 로그인 완료 화면
        string savedEmail = PlayerPrefs.GetString("email");
        if (string.IsNullOrEmpty(savedEmail) || savedEmail == id.text)
        {
            completeLoginBoxEmpty.SetActive(true);
        }
        else
        {
            // 아이디가 존재하지 않는다면?
            // 재입력, 회원가입 창을 띄움
            checkBox.SetActive(true);
        }




    }

    public void OnClickCloseBtn()
    {
        if(checkBox.active)
        {
            checkBox.SetActive(false);
        }

        if(completeLoginBoxEmpty.active)
        {
            completeLoginBoxEmpty.SetActive(false);
        }
    }

    public void OnClickRewirteBtn()
    {
        completeLoginBoxEmpty.SetActive(false);
    }

    public void OnClickSignUpBtn()
    {

    }

}
