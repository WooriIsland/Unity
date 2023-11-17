using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void OnClick_StartScene()
    {
        SceneManager.LoadScene(1);
    }
}
