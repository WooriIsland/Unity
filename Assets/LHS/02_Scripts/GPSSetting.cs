using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GPSSetting : MonoBehaviour
{
    public TextMeshProUGUI name;

    private void Update()
    {
        name.text = GPSManager.instance.CurrentName;
    }
}
