using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickFileBrower()
    {
        //���� ����
        FileBrowser.SetFilters(true, new FileBrowser.Filter("�̹���", ".jpg", ".png"));
        // �� ��� .jpg Ȯ���ڸ� �⺻ ���ͷ� �����մϴ�.
        FileBrowser.SetDefaultFilter(".jpg");


    }
}
