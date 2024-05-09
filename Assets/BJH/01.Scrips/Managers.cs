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
    [SerializeField] ConnectionManager _connection = new ConnectionManager();

    public static ConnectionManager Connection
    {
        get 
        { 
            if(Instance._connection == null)
            {
                GameObject go = GameObject.Find("ConnectionManager");
                ConnectionManager cm = go.GetComponent<ConnectionManager>();
                Instance._connection = cm;
            }
            return Instance._connection; 
        }
    }




    #endregion

    private void Start()
    {
        // 초기화
        Init();

        // 씬이 바뀌더라도 삭제되지 않도록
        DontDestroyOnLoad(gameObject);
    }

    private void Init()
    {
        // 인스턴스가 비어있으면
        if(_instance == null)
        {
            // managers 컴포넌트를 대입
            _instance = gameObject.GetComponent<Managers>();
        }
    }
}
