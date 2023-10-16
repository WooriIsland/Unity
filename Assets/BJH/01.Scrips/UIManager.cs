using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameObject createFamilyCodeImg;

    // Start is called before the first frame update
    void Start()
    {
        createFamilyCodeImg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickCreateFamilyCod()
    {
        createFamilyCodeImg.SetActive(true);
    }

    public void OnClickCheckBtn()
    {
        createFamilyCodeImg.SetActive(false);

    }
}
