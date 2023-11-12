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
        print("토글 누름");

        if (toggle != null)
        {
            Button[] buttons = toggle.GetComponentsInChildren<Button>();

            toggle.onValueChanged.AddListener((boolOn) =>
            {
                if (boolOn == true)
                {
                    print("토글 누름");

                    foreach(Button b in buttons)
                    {
                        b.interactable = true;
                    }
                    
                }

                else
                {
                    print("토글 안누름");

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
