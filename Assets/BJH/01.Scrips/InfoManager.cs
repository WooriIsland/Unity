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

    [SerializeField]
    private string myIslandLike;
    public bool isMyIslandLike = false;

    [SerializeField]
    private string christmasIslandLike;
    public bool isChristmasIslandLike = true;


    [SerializeField]
    public int userId;




    // 방문 하고싶은 섬 정보 임시로 저장
    [SerializeField]
    public string visit;

    [SerializeField]
    public string visitType;

    [SerializeField]
    public int likeCnt = 42;


    public bool isLike;
    public string isIslandUniqueNumber;
    public int islandId;

    private static InfoManager instance;

    // 임시
    // 서버가 안 될 것을 대비하여 특정 섬에 어떤 멤버가 있는지 저장
    public Dictionary<int, List<string>> dicIslandMembers;


    // 임시
    // 닉네임 : 선택한 캐릭터 이름
    public Dictionary<string, string> dicMemberCharacter = new Dictionary<string, string>();

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

    private void Start()
    {
        // islandId가 2인 섬에는 정이와 혜리, 심사위원1, 2, 3이 살고있어요.
        dicIslandMembers = new Dictionary<int, List<string>>();
        List<string> list = new List<string>();
        list.Add("정이");
        list.Add("혜리");
        list.Add("심사위원1");
        list.Add("심사위원2");
        list.Add("심사위원3");

        dicIslandMembers[2] = list;

        // player state 임시 구현
        dicMemberCharacter["정이"] = "m_10";
        dicMemberCharacter["혜리"] = "f_3";
        dicMemberCharacter["심사위원1"] = "f_5";
        dicMemberCharacter["심사위원2"] = "m_8";
        dicMemberCharacter["심사위원3"] = "f_7";
        dicMemberCharacter["까망이"] = "까망이";

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

    public string MyIslandLike
    {
        get => myIslandLike;
        set => myIslandLike = value;
    }

    public string ChristmasIslandLike
    {
        get => christmasIslandLike;
        set => christmasIslandLike = value;
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
