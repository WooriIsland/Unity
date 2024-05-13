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
    //static T GetComponentFromName<T>(string name) where T : Component
    //{
    //    GameObject go = GameObject.Find(name);

    //    if (go == null)
    //    {
    //        Debug.Log("ã������ ���ӿ�����Ʈ�� �����ϴ�.");
    //        return null;
    //    }

    //    T component = go.GetComponent<T>();

    //    if(component == null)
    //    {
    //        Debug.Log("ã������ ������Ʈ�� �����ϴ�.");
    //        return null;
    //    }

    //    return component;
    //}

}
