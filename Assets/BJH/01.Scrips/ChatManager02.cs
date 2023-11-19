using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ChatManager02 : MonoBehaviour
{
    public GameObject yellowArea, whiteArea, dateArea;
    public RectTransform contentRect;
    public Scrollbar scrollBar;
    AreaScript lastArea;

    public void Chat(bool isSend, string text, string user, Texture picure)
    {
        if (text.Trim() == "") return;
        bool isBottom = scrollBar.value <= 0.001f;

        print(text);
    }
}
