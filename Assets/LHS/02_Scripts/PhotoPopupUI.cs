using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPopupUI : MonoBehaviour
{
    public Button outBtn;
    public Button photoBtn;

    void Start()
    {
        outBtn.onClick.AddListener(() => OnOut());
       // photoBtn.onClick.AddListener(() => Photo());
    }

    void Update()
    {
        
    }

    void OnOut()
    {
        PhotoManager.instance.OnPhotoDwon();
    }

   public void Photo()
    {
        PhotoManager.instance.OnZoomCheck();
    }
}
