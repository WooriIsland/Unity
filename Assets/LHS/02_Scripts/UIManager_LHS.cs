using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_LHS : MonoBehaviour
{
    bool isUIState;

    public void OnInteractionUI(GameObject objUI)
    {
        //Ŭ������ �� ���� true �� false / false �� true
        isUIState = !isUIState;

        objUI.SetActive(isUIState);
    }

}
