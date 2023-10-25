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
        //필터 설정
        FileBrowser.SetFilters(true, new FileBrowser.Filter("이미지", ".jpg", ".png"));
        // 이 경우 .jpg 확장자를 기본 필터로 설정합니다.
        FileBrowser.SetDefaultFilter(".jpg");


    }
}
