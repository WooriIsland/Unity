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

    // ���� �� ������
    // �̸���
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

    // �ٽ� �Է�
    public void OnClick_Rewrite()
    {
        faileLoginBox.SetActive(false);
    }


    // ȸ������ ��ư Ŭ��
    public void OnClick_SignUP()
    {
        var info = signupBox.GetComponent<OnBoardingInfo>();

        loginHttp.SignUp(info.email.text, info.password.text, info.nickname.text);
    }

    // ȸ������ ��ư Ŭ��    
    public void OnClickCompleteSignUpBtn()
    {
        sighUpCheckPage.SetActive(true);

    }

    public void OnClickCompleteSignUpCheckBtn()
    {
        sighUpCheckPage.SetActive(false);


    }

    // ȸ���� ������ �α���
    // ���ٸ� ȸ������ or ���Է�
    public void OnClick_NextBtn()
    {
        // �ӽ�
        //loginHttp.OnGetRequest_Test();

        loginHttp.TryLogin(id.text, pw.text);
    }

    // �̸��� ���� ��ư
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


    // -------------------- ȸ������ -------------------- 

    // ȸ������ box Ȱ��ȭ
    public void OnClick_OpenSignUpBox()
    {
        faileLoginBox.SetActive(false);
        signupBox.SetActive(true);
        signupBox.GetComponent<OnBoardingInfo>().email.text = signInBox.GetComponent<OnBoardingInfo>().email.text;
    }

    // ȸ������ �Ϸ�
    public void onClick_CompleteSignUp()
    {
        signupBox.SetActive(false);

        // ȸ�����Խ� �Է��� �̸����� �ڵ����� email input field�� �Էµ�
        string email = signupBox.GetComponent<OnBoardingInfo>().email.text;
        signInBox.GetComponent<OnBoardingInfo>().email.text = email;
        signInBox.GetComponent<OnBoardingInfo>().password.text = "";
    }

    // �̸��� ���� ��ư
    public void OnClick_AuthEmail()
    {
        // �̸��Ϸ� �ڵ� ���۵�
        string email = signupBox.GetComponent<OnBoardingInfo>().email.text;
        print(email);
        loginHttp.SendAuthEmail(email);

        // ���� �ڵ� �Է��ϴ� UI ����
        authEmailBoxEmpty.SetActive(true);
    }

    public void OnClick_AuthEmailCheck()
    {
        string code = authEmailBox.GetComponent<OnBoardingInfo>().code.text;
        loginHttp.AuthEmailCheck(code);
    }



}
