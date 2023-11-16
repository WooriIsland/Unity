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

    // ���� �� ������
    // �̸���
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


        // �ӽ�
        // ���� Ư�� �̸����� �ԷµǸ�, �÷��̾� �г��� �ڵ����� ����
        if(id.text == "jeong@gmail.com")
        {
            PlayerPrefs.SetString("NickName", "����");
            print("�г����� ���̷� �����߽��ϴ�.");
        }

        if(id.text == "hyeri@gmail.com")
        {
            PlayerPrefs.SetString("NickName", "����");
        }
    }


    public void OnClick_GoToFamilyCodeScene()
    {
        SceneManager.LoadScene(1);
    }

    // ���� ����
    public void OnClickStartBG()
    {
        startBG.SetActive(false);
    }

    // �ٽ� �Է�
    public void OnClick_Rewrite()
    {
        faileLoginBox.SetActive(false);
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



    public void OnClickNextSceneBtn()
    {
        email = id.text;
        PlayerPrefs.SetString("email", email);
        print(PlayerPrefs.GetString("email").Length);
       


        if(PlayerPrefs.GetString("FamilyCode").Length > 0)
        {
            // ������?
            // �ٷ� �濡 �����ϱ�
            cm3.ConnectRequest();
            
        }
        else
        {
            // ���� ����� �����ڵ尡 ������
            // ���� �ڵ带 �Է��ϴ� ������ �̵�
            SceneManager.LoadScene(1);

        }


    }

    // �α����� ���� ȸ�������� ���� �Ǻ����ִ� �Լ�
    public void OnClick_NextBtn()
    {
        // ������ ����Ͽ� ���̵� �����ϴ��� Ȯ��
        loginHttp.TryLogin(id.text, pw.text);
    }

    // �̸��� ���� ��ư
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



}
