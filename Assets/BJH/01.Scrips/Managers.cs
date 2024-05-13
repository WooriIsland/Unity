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
    [SerializeField] ChatManager _chatManager = new ChatManager();
    [SerializeField] InfoManager _info = new InfoManager();

    public static ConnectionManager Connection
    {
        get
        {
            return Instance._connection;
        }
    }

    public static ChatManager Chat
    {
        get
        {
            return Instance._chatManager;
        }
    }

    public static InfoManager Info
    {
        get
        {
            if(Instance._info == null)
            {
                GameObject go = GameObject.Find("InfoManager");
                if (go != null)
                {
                    Instance._info = go.GetComponent<InfoManager>();
                }
            }

            return Instance._info;
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

    // 게임오브젝트의 컴포넌트를 리턴하는 메서드
    // 하나의 게임오브젝트에 하나의 컴포넌트만 존재함을 전재로 하며
    // 게임오브젝트와 컴포넌트의 이름은 동일합니다.
    //static T GetComponentFromName<T>(string name) where T : Component
    //{
    //    GameObject go = GameObject.Find(name);

    //    if (go == null)
    //    {
    //        Debug.Log("찾으려는 게임오브젝트가 없습니다.");
    //        return null;
    //    }

    //    T component = go.GetComponent<T>();

    //    if(component == null)
    //    {
    //        Debug.Log("찾으려는 컴포넌트가 없습니다.");
    //        return null;
    //    }

    //    return component;
    //}

}
