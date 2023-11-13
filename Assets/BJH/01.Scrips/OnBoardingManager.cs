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

    // ���� �� ������
    // �̸���
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
            print("���� ���� ������ ��� �����߽��ϴ�.");
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

    public void OnClickNextBtn()
    {
        // ������ ����Ͽ� ���̵� �����ϴ��� Ȯ��

        // ���̵� �����Ѵٸ�?
        // �α��� �Ϸ� ȭ��
        string savedEmail = PlayerPrefs.GetString("email");
        if (string.IsNullOrEmpty(savedEmail) || savedEmail == id.text)
        {
            completeLoginBoxEmpty.SetActive(true);
        }
        else
        {
            // ���̵� �������� �ʴ´ٸ�?
            // ���Է�, ȸ������ â�� ���
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
