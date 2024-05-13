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

    #region Game



    #endregion

    #region Core
    [SerializeField] ConnectionManager _connection = new ConnectionManager();
    [SerializeField] ChatManager _chat = new ChatManager();
    [SerializeField] InfoManager _info = new InfoManager();
    [SerializeField] LobbyManager _lobby = new LobbyManager();
    [SerializeField] HttpManager _http = new HttpManager();


    public static ConnectionManager Connection
    {
        get
        {
            if (Instance._connection == null)
            {
                ConnectionManager component = GetComponent<ConnectionManager>(nameof(ConnectionManager));
                Instance._connection = component;
            }
            return Instance._connection;
        }
    }

    public static ChatManager Chat
    {
        get
        {
            if (Instance._chat == null)
            {
                ChatManager component = GetComponent<ChatManager>(nameof(ChatManager));
                Instance._chat = component;
            }
            return Instance._chat;
        }
    }

    public static InfoManager Info
    {
        get
        {
            if(Instance._info == null)
            {
                InfoManager component = GetComponent<InfoManager>(nameof(InfoManager));
                Instance._info = component;
            }
            return Instance._info;
        }
    }
    public static LobbyManager Lobby
    {
        get
        {
            if (Instance._lobby == null)
            {
                LobbyManager component = GetComponent<LobbyManager>(nameof(LobbyManager));
                Instance._lobby = component;
            }
            return Instance._lobby;
        }
    }

    public static HttpManager Http
    {
        get
        {
            if(Instance._http == null)
            {
                HttpManager component = GetComponent<HttpManager>(nameof(HttpManager));
                Instance._http = component;
            }
            return Instance._http;
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
    static T GetComponent<T>(string name) where T : Component
    {
        GameObject go = GameObject.Find(name);
        
        if(go != null)
        {
            // 최상단에 추가된 컴포넌트 1개를 리턴
            return go.GetComponent<T>();
        }
        return null;
    }
}
