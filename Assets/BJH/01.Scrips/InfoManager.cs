using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    private string familyCode;

    private static InfoManager instance;

    public static InfoManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public string FamilyCode
    {
        get
        {
            return familyCode;
        }

        set
        {
            familyCode = value;
        }
    }


}
