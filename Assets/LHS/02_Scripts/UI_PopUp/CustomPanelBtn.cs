using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomPanelBtn : Button
{
    /*public GameObject[] Btn;

    public GameObject[] panel;

    public void ClickBtn(int num)
    {
        for(int i = 0; i < panel.Length; i++)
        {
            if (i == num)
            {
                panel[i].SetActive(true);
                Btn[i].SetActive(false);
            }

            panel[i].SetActive(false);
        }
    }*/

    //public num
    /*public GameObject bar;
    public GameObject panel;*/
    public int num;

    protected override void Start()
    {
        base.Start();

        //색 안변하게 하기 위해
        //transition = Transition.None;
    }

    void Update()
    {

    }

    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
        print("OnMove");
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        print("OnPointerDown");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        print("OnPointerEnter");

        /*var v = transform.DOScale(0.9f, 0.8f).SetEase(Ease.OutBack);
        v.onComplete = () => {
            print("트윈 끝!");
        };*/
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        print("OnPointerExit");

        //transform.DOScale(1f, 0.8f).SetEase(Ease.OutBack);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        print("OnPointerUp");

        //transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        print("OnSelect");

        //transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutBounce);
        //bar.SetActive(true);
       //panel.SetActive(true);
        CustomPanelM.instance.ClickBtn(num);

        //나머지 애들 꺼주기
    }
}
