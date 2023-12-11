using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class VideoMap : MonoBehaviour
{
    public VideoPlayer video1;
    public GameObject Image;
    public CanvasGroup canvasGroup;

    public GameObject eventUI;

    void Start()
    {
        Invoke("OffMapOne", 2f);
    }

    
    void Update()
    {
        
    }

    public void OnMapOne()
    {
        //rawImage.gameObject.GetComponent<RawImage>().enabled = true;

        Invoke("Deley", 0.2f);
    }

    public void Deley()
    {
        video1.Play();
        print(video1.time);
        print(video1.isPlaying);
        print(video1.isLooping);
    }

    public void OffMapOne()
    {
        //Image.SetActive(false);

        canvasGroup = GetComponent<CanvasGroup>();
        var v = canvasGroup.DOFade(0, 0.4f);
        v.onComplete = OnClose;


    }
    public virtual void OnClose()
    {
        //eventUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
