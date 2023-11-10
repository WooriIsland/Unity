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

    // ���� �� ������
    // �̸���
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
            print("���� ���� ������ ��� �����߽��ϴ�.");
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
            // ������?
            // �ٷ� �濡 �����ϱ�
            cm3.OnClickConnect();
            
        }
        else
        {
            // ���� ����� �����ڵ尡 ������
            // ���� �ڵ带 �Է��ϴ� ������ �̵�
            SceneManager.LoadScene(1);

        }


    }

}
