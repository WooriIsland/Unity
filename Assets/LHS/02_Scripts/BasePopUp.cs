using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopUp : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public void CloseAction()
    {
        var v = transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
        v.onComplete = OnClose;
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OpenAction()
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        var v = transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        v.onComplete = OnOpen;
    }

    public virtual void OnOpen()
    {

    }
}
