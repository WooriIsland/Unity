using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject ob;
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            GameObject go = GameObject.Find("InfoManager");
            ob = go;
            Debug.Log(go.ToString());
        }
    }
}
