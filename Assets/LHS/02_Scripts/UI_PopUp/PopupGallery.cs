using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGallery : BasePopup
{
    public bool isAutoClose;
    public LoadGallery loadGallery;
    public BasePopup faceSuccess;

    public void OnClickGallery()
    {
        loadGallery.OnClickImageLoad(OnCompleteLoad);
    }

    void OnCompleteLoad(string path)
    {
        if(isAutoClose)
        {
            CloseAction(faceSuccess);
        }
        
        loadGallery.StartCoroutine(loadGallery.LoadImage(path));
    }
}
