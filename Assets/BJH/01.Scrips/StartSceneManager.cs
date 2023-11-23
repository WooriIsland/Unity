using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void Start()
    {
        //�º��� BGM
        SoundManager_LHS.instance.PlayBGM(SoundManager_LHS.EBgm.BGM_INTRO);
    }

    public void OnClick_StartScene()
    {
        //�ʹ� �ٷ� �ٲ��� 1�ʵڿ� ���� �� �� �ְ�
        Invoke("LoadScene", 0.2f);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
