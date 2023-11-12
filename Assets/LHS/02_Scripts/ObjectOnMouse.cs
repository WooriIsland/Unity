using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnMouse : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        MoveFrame();
    }

    private void MoveFrame()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            print(hitInfo.transform.gameObject);

            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Frame"))
            {
                print("¾×ÀÚ´ê¾Ò´ç");
            }
        }
    }
}
