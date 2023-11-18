using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GPSSetting : MonoBehaviour
{
    public TextMeshProUGUI name;

    private void Update()
    {
        if(GPSManager.instance.TargetName == null)
        {
            name.text = "위치 없음";
        }
        else
        {
            name.text = GPSManager.instance.TargetName;
        }
    }
}
