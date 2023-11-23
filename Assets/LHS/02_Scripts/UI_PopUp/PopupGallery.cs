using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGallery : BasePopup
{
    public BasePopup faceSuccess;
    public bool isAutoClose;
    private LoadGallery loadGallery;

    public void Start()
    {
        loadGallery = LoadGallery.instance;
    }

    public void OnClickGallery()
    {
        loadGallery.OnClickImageLoad(OnCompleteLoad);
    }

    void OnCompleteLoad(string path)
    {
        if (isAutoClose)
        {
            CloseAction(faceSuccess);
        }

        loadGallery.StartCoroutine(loadGallery.LoadImage(path));
    }

    // 안면등록 성공 시 UI 뜨게
    public void OnClickFaceImageSave()
    {
        loadGallery.OnFaceImageSave();
    }
}
