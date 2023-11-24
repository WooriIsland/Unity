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

    [SerializeField]
    private string islandType, islandName, islandIntroduce;

    [SerializeField]
    private bool secret;

    [SerializeField]
    private string islandCode;


    // 방문 하고싶은 섬 정보 임시로 저장
    [SerializeField]
    public string visit;


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
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

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

    public string IslandType { get => islandType; set => islandType = value; }
    public string IslandName { get => islandName; set => islandName = value; }
    public string IslandIntroduce { get => islandIntroduce; set => islandIntroduce = value; }
    public bool Secret { get => secret; set => secret = value; }
    public string IslandCode { get => islandCode; set => islandCode = value; }

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
