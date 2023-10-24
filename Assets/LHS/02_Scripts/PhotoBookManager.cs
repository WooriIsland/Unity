using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBookManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PhotoBookUI;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPhotoBook()
    {
        PhotoBookUI.SetActive(true);
    }
}
