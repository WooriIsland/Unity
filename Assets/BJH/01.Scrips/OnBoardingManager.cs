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

    // ���� �� ������
    // �̸���
    public TextMeshProUGUI emailInput;
    string email;


    private void Start()
    {
        signUpPage.SetActive(false);
        sighUpCheckPage.SetActive(false);

        
        if (PlayerPrefs.GetString("email").Length > 0)
        {
            string savedEmail = PlayerPrefs.GetString("email");

            // �� �ȵǴ°��� �𸣰���;;
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
       
        // ���� �ڵ带 �Է��ϴ� ������ �̵�
        SceneManager.LoadScene(1);
    }

}
