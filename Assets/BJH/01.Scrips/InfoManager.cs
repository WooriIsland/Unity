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




    // �湮 �ϰ���� �� ���� �ӽ÷� ����
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

    // �ӽ�
    // ������ �� �� ���� ����Ͽ� Ư�� ���� � ����� �ִ��� ����
    public Dictionary<int, List<string>> dicIslandMembers;


    // �ӽ�
    // �г��� : ������ ĳ���� �̸�
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
        // islandId�� 2�� ������ ���̿� ����, �ɻ�����1, 2, 3�� ����־��.
        dicIslandMembers = new Dictionary<int, List<string>>();
        List<string> list = new List<string>();
        list.Add("����");
        list.Add("����");
        list.Add("�ɻ�����1");
        list.Add("�ɻ�����2");
        list.Add("�ɻ�����3");

        dicIslandMembers[2] = list;

        // player state �ӽ� ����
        dicMemberCharacter["����"] = "m_10";
        dicMemberCharacter["����"] = "f_3";
        dicMemberCharacter["�ɻ�����1"] = "f_5";
        dicMemberCharacter["�ɻ�����2"] = "m_8";
        dicMemberCharacter["�ɻ�����3"] = "f_7";
        dicMemberCharacter["�����"] = "�����";

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
