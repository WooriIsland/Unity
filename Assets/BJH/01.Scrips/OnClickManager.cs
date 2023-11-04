using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class OnClickManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 다음 씬으로 이동
    public void OnClickNextScene()
    {
        SceneManager.LoadScene(3);
    }
}
