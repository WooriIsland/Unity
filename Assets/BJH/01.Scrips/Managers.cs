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
    ConnectionManager _connection = new ConnectionManager();

    public static ConnectionManager Connection
    {
        get { return Instance._connection; }
    }


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
