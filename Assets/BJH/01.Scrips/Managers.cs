using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ȯ
// Manager ��ũ��Ʈ�� �����ϴ� ��ũ��Ʈ
// ������Ƽ�� ���� ����
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
        // �ʱ�ȭ
        Init();

        // ���� �ٲ���� �������� �ʵ���
        DontDestroyOnLoad(gameObject);
    }


    private void Init()
    {
        // �ν��Ͻ��� ���������
        if(_instance == null)
        {
            // managers ������Ʈ�� ����
            _instance = gameObject.GetComponent<Managers>();
        }
    }

    // ���ӿ�����Ʈ�� ������Ʈ�� �����ϴ� �޼���
    // �ϳ��� ���ӿ�����Ʈ�� �ϳ��� ������Ʈ�� �������� ����� �ϸ�
    // ���ӿ�����Ʈ�� ������Ʈ�� �̸��� �����մϴ�.
    static T GetComponent<T>(string name) where T : Component
    {
        GameObject go = GameObject.Find(name);
        
        if(go != null)
        {
            // �ֻ�ܿ� �߰��� ������Ʈ 1���� ����
            return go.GetComponent<T>();
        }
        return null;
    }
}
