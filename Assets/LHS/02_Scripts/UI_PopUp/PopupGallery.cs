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

    // �ȸ��� ���� �� UI �߰�
    public void OnClickFaceImageSave()
    {
        loadGallery.OnFaceImageSave();
    }
}
