using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_LHS : MonoBehaviour
{
    public CustomButton[] buttons;
    bool isUIState;

    private void Start()
    {
        Toggle toggle = gameObject.GetComponent<Toggle>();
        print("��� ����");

        if (toggle != null)
        {
            //Button[] buttons = toggle.GetComponentsInChildren<Button>();

            toggle.onValueChanged.AddListener((boolOn) =>
            {
                if (boolOn == true)
                {
                    print("��� ����");
                    var v = transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
                    SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnAdd);

                    v.onComplete = OnCheck;

                    foreach (Button b in buttons)
                    {
                        b.interactable = true;
                    }
                    
                }

                else
                {
                    var v = transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
                    SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnAdd);

                    v.onComplete = OnCheck;

                    print("��� �ȴ���");

                    foreach (Button b in buttons)
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
        //Ŭ������ �� ���� true �� false / false �� true
        isUIState = !isUIState;

        objUI.SetActive(isUIState);
    }

    public void OnBtnNext(GameObject nextBtn)
    {
        //�ڱ� �ڽ��� ������Ʈ ������
        //myBtn.SetActive(false);
        //���� ������Ʈ ������ �ϱ�
        nextBtn.SetActive(true);
    }

    public void OnBtnBack(GameObject myBtn)
    {
        //�ڱ� �ڽ��� ������Ʈ ������
        myBtn.SetActive(false);
    }

}
