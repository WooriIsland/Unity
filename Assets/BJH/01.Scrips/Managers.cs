using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 변지환
// Manager 스크립트를 관리하는 스크립트
// 프로퍼티를 통해 접근
public class Managers : MonoBehaviour
{
    static Managers _instance;
    static Managers Instance { get { return _instance; } }

    #region Core



    #endregion

    private void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if(go == null)
        {
            // go.name = "@Managers";
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }
        DontDestroyOnLoad(go);
        _instance = go.GetComponent<Managers>();
    }
}
