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

    #region Core
    [SerializeField] ConnectionManager _connection = new ConnectionManager();
    [SerializeField] ChatManager _chatManager = new ChatManager();

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

    public static ChatManager Chat
    {
        get
        {
            if(Instance._chatManager == null)
            {
                GameObject go = GameObject.Find("ChatManager");
                ConnectionManager cm = go.GetComponent<ConnectionManager>();
                Instance._connection = cm;
            }
            return Instance._chatManager;
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
}
