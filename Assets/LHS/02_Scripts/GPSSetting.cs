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
            name.text = "��ġ ����";
        }
        else
        {
            name.text = GPSManager.instance.TargetName;
        }
    }
}
