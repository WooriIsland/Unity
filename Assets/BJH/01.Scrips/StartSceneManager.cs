using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void Start()
    {
        //온보딩 BGM
        SoundManager_LHS.instance.PlayBGM(SoundManager_LHS.EBgm.BGM_INTRO);
    }

    public void OnClick_StartScene()
    {
        //너무 바로 바껴서 1초뒤에 실행 될 수 있게
        Invoke("LoadScene", 0.2f);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
