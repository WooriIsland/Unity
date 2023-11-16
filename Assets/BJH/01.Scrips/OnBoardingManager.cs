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

    public Button nextBtn;

    public GameObject faileLoginBox;

    public GameObject sighUpCheckPage;

    public GameObject signInBox, signupBox;

    public LoginHttp loginHttp;

    public GameObject authEmailBox;

    // 저장 할 데이터
    // 이메일
    string email;

    private static OnBoardingManager instance;

    public static OnBoardingManager _instance
    {
        get
        {
            return instance;
        }
    }

    ConnectionManager03 cm3;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        nextBtn.interactable = false;
        signupBox.SetActive(false);

        cm3 = GameObject.Find("ConnectionManager03").GetComponent<ConnectionManager03>();
    }

    private void Update()
    {
        if ((id.text.Length > 0))
        {
            pw.onValueChanged.AddListener((string s) =>
            {
                if (s.Length > 0)
                {
                    nextBtn.interactable = true;
                }
                else
                {
                    nextBtn.interactable = false;
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
                            nextBtn.interactable = true;
                        }
                        else
                        {
                            nextBtn.interactable = false;

                        }
                    });
                }
                else
                {
                    nextBtn.interactable = false;
                }
            });
        }


        // 임시
        // 만약 특정 이메일이 입력되면, 플레이어 닉네임 자동으로 지정
        if(id.text == "jeong@gmail.com")
        {
            PlayerPrefs.SetString("NickName", "정이");
            print("닉네임을 정이로 설정했습니다.");
        }

        if(id.text == "hyeri@gmail.com")
        {
            PlayerPrefs.SetString("NickName", "혜리");
        }
    }


    public void OnClick_GoToFamilyCodeScene()
    {
        SceneManager.LoadScene(1);
    }

    // 게임 시작
    public void OnClickStartBG()
    {
        startBG.SetActive(false);
    }

    // 다시 입력
    public void OnClick_Rewrite()
    {
        faileLoginBox.SetActive(false);
    }


    // 회원가입 버튼 클릭    
    public void OnClickCompleteSignUpBtn()
    {
        sighUpCheckPage.SetActive(true);
    }

    public void OnClickCompleteSignUpCheckBtn()
    {
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
    public void OnClick_NextBtn()
    {
        // 서버와 통신하여 아이디가 존재하는지 확인
        loginHttp.TryLogin(id.text, pw.text);
    }

    // 이메일 인증 버튼
    public void OnClick_CheckEmail()
    {
        authEmailBox.SetActive(true);
        authEmailBox.GetComponent<OnBoardingInfo>().authEmail.text = signupBox.GetComponent<OnBoardingInfo>().email.text;
    }

    public void OnClick_CloseBtn(GameObject go)
    {
        go.SetActive(false);

        //if(checkBox.activeSelf)
        //{
        //    checkBox.SetActive(false);
        //}

        //if(completeLoginBoxEmpty.activeSelf)
        //{
        //    completeLoginBoxEmpty.SetActive(false);
        //}
    }

    public void OnClickRewirteBtn()
    {
        completeLoginBoxEmpty.SetActive(false);
    }


    // -------------------- 회원가입 -------------------- 

    // 회원가입 box 활성화
    public void OnClick_OpenSignUpBox()
    {
        faileLoginBox.SetActive(false);
        signupBox.SetActive(true);
        signupBox.GetComponent<OnBoardingInfo>().email.text = signInBox.GetComponent<OnBoardingInfo>().email.text;
    }

    // 회원가입 완료
    public void onClick_CompleteSignUp()
    {
        signupBox.SetActive(false);

        // 회원가입시 입력한 이메일이 자동으로 email input field에 입력됨
        string email = signupBox.GetComponent<OnBoardingInfo>().email.text;
        signInBox.GetComponent<OnBoardingInfo>().email.text = email;
        signInBox.GetComponent<OnBoardingInfo>().password.text = "";
    }



}
