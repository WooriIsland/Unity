using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager_LHS : MonoBehaviour
{
    public CustomButton[] buttons;
    bool isUIState;

    private void Start()
    {
        UnityEngine.UI.Toggle toggle = gameObject.GetComponent<UnityEngine.UI.Toggle>();

        print("토글 누름");

        if (toggle != null)
        {

            toggle.onValueChanged.AddListener((boolOn) =>
            {
                if (boolOn == true)
                {
                    print("토글 누름");
                    var v = transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
                    SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnAdd);

                    v.onComplete = OnCheck;

                    foreach (CustomButton b in buttons)
                    {
                        b.interactable = true;
                    }
                    
                }

                else
                {
                    var v = transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
                    SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnAdd);

                    v.onComplete = OnCheck;

                    print("토글 안누름");

                    foreach (CustomButton b in buttons)
                    {
                        b.interactable = false;
                    }
                }
            });
        }
    }

    private void OnCheck()
    {
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnInteractionUI(GameObject objUI)
    {
        //클릭했을 때 값이 true 면 false / false 면 true
        isUIState = !isUIState;

        objUI.SetActive(isUIState);
    }

    public void OnBtnNext(GameObject nextBtn)
    {
        //자기 자신의 오브젝트 꺼지고
        //myBtn.SetActive(false);
        //다음 오브젝트 켜지게 하기
        nextBtn.SetActive(true);
    }

    public void OnBtnBack(GameObject myBtn)
    {
        //자기 자신의 오브젝트 꺼지고
        myBtn.SetActive(false);
    }

}
