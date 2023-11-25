using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomPanelM : MonoBehaviour
{
    public static CustomPanelM instance;

    public GameObject[] Btn;
    public GameObject[] BtnSelect;

    public GameObject[] panel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //다른 거 다꺼지게
    public void ClickBtn(int num)
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnAdd);

        for (int i = 0; i < panel.Length; i++)
        {
            if (i != num)
            {
                print(num);
                Btn[i].SetActive(false);
                BtnSelect[i].SetActive(false);
                panel[i].SetActive(false);
            }

            else
            {
                panel[i].SetActive(true);
                BtnSelect[i].SetActive(true);
                Btn[i].SetActive(true);
            }
        }
    }
}
