using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;

public class OnBoardingManager : MonoBehaviour
{
    public void OnClickLogin()
    {
        // ���� �ڵ带 �Է��ϴ� ������ �̵�
        SceneManager.LoadScene(1);
    }

}
