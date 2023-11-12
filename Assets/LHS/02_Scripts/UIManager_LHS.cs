using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_LHS : MonoBehaviour
{
    bool isUIState;

    private void Start()
    {
        Toggle toggle = gameObject.GetComponent<Toggle>();
        print("��� ����");

        if (toggle != null)
        {
            Button[] buttons = toggle.GetComponentsInChildren<Button>();

            toggle.onValueChanged.AddListener((boolOn) =>
            {
                if (boolOn == true)
                {
                    print("��� ����");

                    foreach(Button b in buttons)
                    {
                        b.interactable = true;
                    }
                    
                }

                else
                {
                    print("��� �ȴ���");

                    foreach (Button b in buttons)
                    {
                        b.interactable = false;
                    }
                }
            });
        }
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
