using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;

public class OnBoardingManager : MonoBehaviour
{
    public void OnClickLogin()
    {
        // 가족 코드를 입력하는 씬으로 이동
        SceneManager.LoadScene(1);
    }

}
