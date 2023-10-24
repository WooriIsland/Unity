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

}
