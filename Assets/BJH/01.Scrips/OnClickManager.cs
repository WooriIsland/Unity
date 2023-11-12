using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class OnClickManager : MonoBehaviour
{
    // 다음 씬으로 이동
    public void OnClickNextScene()
    {
        SceneManager.LoadScene(3);
    }
}
