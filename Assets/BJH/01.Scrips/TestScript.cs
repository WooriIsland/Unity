using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    

    void Start()
    {
        List<string> list = new List<string>();
        list.Add("������ ��û��");
        list.Add("��ȯ�̵� ��������");

        AudioManager audioManager = AudioManager.Instance;
        Hashtable hash = new Hashtable();
        hash.Add("�����̹ٺ�", list);
        hash.Add(10, "�ϸ��ն�Ӥä���");

        List<string> list2 = (List<string>)hash["�����̹ٺ�"];
        print(list2[0]);
        print(list2[1]);

        print(hash[10].ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
