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
        print("코루틴 시작");
        yield return new WaitForSeconds(2.5f);

        if (gameObject.name.ToString() == "CompleteLoginBoxEmpty")
        {
            // 로그인 완료 박스 오브젝트라면? 자동으로 로비에 연결
            gameObject.SetActive(false);
            ConnectionManager.Instance.OnClickConnect();
        }

    }
}
