using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClose : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("CoAutoClose");
    }

    IEnumerator CoAutoClose()
    {
        print("�ڷ�ƾ ����");
        yield return new WaitForSeconds(2.5f);

        if (gameObject.name.ToString() == "CompleteLoginBoxEmpty")
        {
            // �α��� �Ϸ� �ڽ� ������Ʈ���? �ڵ����� �κ� ����
            gameObject.SetActive(false);
            ConnectionManager.Instance.OnClickConnect();
        }

    }
}
