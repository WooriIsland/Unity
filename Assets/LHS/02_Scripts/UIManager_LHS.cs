using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_LHS : MonoBehaviour
{
    bool isUIState;

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
