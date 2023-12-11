using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    private void Start()
    {
        print("dddd");
        gameObject.GetComponent<BasePopup>().OpenAction();
        print("eeeee");

    }


}
