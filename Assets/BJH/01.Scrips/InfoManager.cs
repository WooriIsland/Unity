using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    [SerializeField]
    private string nickName;

    [SerializeField]
    private string familyCode;

    [SerializeField]
    private string character;

    //[SerializeField]
    //private string acessToken;

    //[SerializeField]
    //private string refreshToken;

    public string accessToken;
    public string refreshToken;


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

    public string NickName
    {
        get { return nickName; }
        set { nickName = value; }
    }

    public string Character
    {
        get { return character; }
        set { character = value; }
    }

    //public string AcessToken
    //{
    //    get { return AcessToken; }
    //    set { AcessToken = value; }
    //}

    //public string RefreshToken
    //{
    //    get { return refreshToken; }
    //    set { refreshToken = value; }
    //}

}
