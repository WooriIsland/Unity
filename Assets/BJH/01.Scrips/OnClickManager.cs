using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class OnClickManager : MonoBehaviour
{
    // ���� ������ �̵�
    public void OnClickNextScene()
    {
        SceneManager.LoadScene(3);
    }
}
