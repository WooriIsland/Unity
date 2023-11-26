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
    public GameObject completeLoginBoxEmpty, checkBox, CompleteSignUpBox;

    public Button nextBtn;

    public GameObject faileLoginBox;

    public GameObject sighUpCheckPage;

    public GameObject signInBox, signupBox;

    public LoginHttp loginHttp;

    public GameObject authEmailBoxEmpty, authEmailBox;

    // 저장 할 데이터
    // 이메일
    string email;

    private static OnBoardingManager instance;

    ConnectionManager03 cm3;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public static OnBoardingManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Start()
    {
        nextBtn.interactable = false;
        CompleteSignUpBox.SetActive(false);

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

#if UNITY_EDITOR

        if(Input.GetKeyDown(KeyCode.F2))
        {
            id.text = "jong@gmail.com";
            pw.text = "wjd123";
            OnClick_NextBtn();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            id.text = "hyeri@gmail.com";
            pw.text = "gP1234";
            OnClick_NextBtn();
        }

#endif
    }


    public void OnClick_GoToFamilyCodeScene()
    {
        SceneManager.LoadScene(2);
    }

    // 다시 입력
    public void OnClick_Rewrite()
    {
        faileLoginBox.SetActive(false);
    }


    // 회원가입 버튼 클릭
    public void OnClick_SignUP()
    {
        var info = signupBox.GetComponent<OnBoardingInfo>();

        loginHttp.SignUp(info.email.text, info.password.text, info.nickname.text);
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

    // 회원이 있으면 로그인
    // 없다면 회원가입 or 재입력
    public void OnClick_NextBtn()
    {
        // 임시
        //loginHttp.OnGetRequest_Test();

        loginHttp.TryLogin(id.text, pw.text);
    }

    // 이메일 인증 버튼
    public void OnClick_CheckEmail()
    {
        authEmailBoxEmpty.SetActive(true);
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

    // 이메인 인증 버튼
    public void OnClick_AuthEmail()
    {
        // 이메일로 코드 전송됨
        string email = signupBox.GetComponent<OnBoardingInfo>().email.text;
        print(email);
        loginHttp.SendAuthEmail(email);

        // 인증 코드 입력하는 UI 생성
        authEmailBoxEmpty.SetActive(true);
    }

    public void OnClick_AuthEmailCheck()
    {
        string code = authEmailBox.GetComponent<OnBoardingInfo>().code.text;
        loginHttp.AuthEmailCheck(code);
    }



}
