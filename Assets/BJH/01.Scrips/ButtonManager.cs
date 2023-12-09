using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void OnClick_Alert()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Alert);
    }
}
