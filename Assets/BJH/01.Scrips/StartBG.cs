using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBG : MonoBehaviour
{
    // 클릭 여부
    bool isClick = false;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        isClick = true;
    }

    private void Update()
    {
        if(isClick)
        {
            gameObject.SetActive(false);
            isClick = false;
        }
        if(Input.touchCount > 0)
        {
            gameObject.SetActive(false);
        }
    }
}
