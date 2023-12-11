using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    // 모든 플에이어
    //public GameObject[] players;

    // 나의 카메라
    public Camera camera;
    public Camera roomCam;
    public bool isMine;
    //private bool state = true;

    // 프리팹의 닉네임
    //public TMP_Text nickName;
    public GameObject nickNameFiled;
    public TMP_Text nickName;

    public GameObject playerList;



    // 애니메이션 : Hello
    public Camera aniCam;
    public PlayerMove playerMove;
    Animator[] animator;
    int aniTemp;
    public bool isAni = true;
    //public TMP_Text nickName;


    //public GameObject[] models;

    // state를 위해서 charactername 저장해주기
    public string character;


    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 닉네임 설정
        nickName.text = photonView.Owner.NickName; // connection manager의 join room에서 설정해줌

        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // 내 캐릭터가 아니면
        if(!photonView.IsMine)
        {
            // 카메라 끄기
            camera.enabled = false;
            roomCam.enabled = false;
            isMine = false;            
        }
        // 내 캐릭터면
        else
        {
            // 카메라 켜기
            isMine = true;

            // 닉네임 끄기
            nickNameFiled.SetActive(false);
            //nickName.enabled = false;
        }


        // 일반 맵
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            // 캐릭터가 변경되면?
            if(InfoManager.Instance.Character != InfoManager.Instance.dicMemberCharacter[photonView.Owner.NickName]) 
            {
                string name = PhotonNetwork.NickName;
                string character = InfoManager.Instance.Character;
                photonView.RPC("ChangeCharacter", RpcTarget.AllBuffered, name, character);
            }
            // 접속한것으로 셋팅  
            PlayerStateManager.instance.ChangeOffLine(photonView.Owner.NickName, false);

            // SettingUI 설정
            SettingUI();
        }

        // animaotr 변수 가져오기
        animator = playerMove.animator;

        // 입장하면 GameManager의 userList에 자신의 photonview ID를 추가
        GameManager.instance.userList.Add(photonView.ViewID);
        //PlayerStateManager.instance.plzUpdate = true;



    }

    private void Update()
    {

        TouchKkamang();
    }

    [PunRPC]
    public void ChangeCharacter(string name, string character)
    {
        InfoManager.Instance.dicMemberCharacter[name] = character;
        print($"Photon의 닉네임 : {PhotonNetwork.NickName}, 저장된 dic은 : {InfoManager.Instance.dicMemberCharacter[name]}");
        PlayerStateManager.instance.plzUpdate = true;

    }

    private void OnDestroy()
    {
        // 일반 맵
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            // 접속한것으로 셋팅  
            PlayerStateManager.instance.ChangeOffLine(photonView.Owner.NickName, true);
        }
    }

    // 인사 애니메이션
    public void StartHello()
    {
        // 내 캐릭터의 photonView ID를 RPC에 넘겨
        string id = photonView.ViewID.ToString();
        photonView.RPC("PunAnimation", RpcTarget.All, id,"Hello");
    }

    // 펀치 애니메이션
    public void StartPunch()
    {
        // 내 캐릭터의 photonView ID를 RPC에 넘겨
        string id = photonView.ViewID.ToString();
        photonView.RPC("PunAnimation", RpcTarget.All, id, "Punch");
    }

    // 댄스 애니메이션
    public void StartDance()
    {
        // 내 캐릭터의 photonView ID를 RPC에 넘겨
        string id = photonView.ViewID.ToString();
        photonView.RPC("PunAnimation", RpcTarget.All, id, "Dance");
    }

    // 애니메이션 동기화
    [PunRPC]
    public void PunAnimation(string id, string trigger)
    {
        print("RPC진입");

        // 애니메이션을 해야 할 photonView id랑 애니메이션 이름이 들어옴
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");

        // id와 동일한 photonView id를 가진 플레이어를 찾기
        foreach (GameObject go2 in go)
        {
            if (go2.GetComponent<PhotonView>().ViewID == int.Parse(id))
            {
                for (int i = 0; i < go2.transform.GetChild(0).transform.childCount; i++)
                {
                    if(go2.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf == true && go2.transform.GetChild(0).transform.GetChild(i).gameObject.name != "Canvas")
                    {
                        switch (trigger)
                        {
                            case "Hello":
                                go2.transform.GetChild(0).transform.GetChild(i).GetComponent<Animator>().SetTrigger(trigger);
                                print(go2.transform.GetChild(0)); // Player List
                                print(go2.transform.GetChild(0).transform.GetChild(i)); // 플레이어가 선택한 게임 오브젝트가 잘 들어옴
                                SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);
                                break;
                            case "Punch":
                                go2.transform.GetChild(0).transform.GetChild(i).GetComponent<Animator>().SetTrigger(trigger);
                                SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Wow);
                                break;
                            case "Dance":
                                go2.transform.GetChild(0).transform.GetChild(i).GetComponent<Animator>().SetTrigger(trigger);
                                SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Dance);

                                break;
                        }
                    }
                }
            }
        }
    }









    // 캐릭터 선택
    public void SelectModel(string characterName)
    {
        photonView.RPC(nameof(RpcSelectModel), RpcTarget.AllBuffered, characterName);
    }

    // 캐릭터 선택 동기화
    [PunRPC]
    void RpcSelectModel(string characterName) // f_1
    {
        // 선택한 캐릭터 프리팹에서 활성화
        foreach(Transform t in playerList.transform)
        {
            if(t.name == characterName)
            {
                t.gameObject.SetActive(true);
                break;
            }
        }

        // 들어오면서 캐릭터 채팅 프로필 정보 저장해주기
        // 닉네임 : 캐릭터 이름
        // 지환 : f_13
        ChatManager.Instance.dicAllPlayerProfile[nickName.text] = characterName;

        // 만약 일반맵에 접속했고
        //if(SceneManager.GetActiveScene().buildIndex == 5)
        //{
        //    // 입장객 닉네임이 정이 or 혜리면
        //    if(InfoManager.Instance.NickName == "정이" || InfoManager.Instance.NickName == "혜리")
        //    {
        //        // 입장한 플레이어 상태 무지개 색으로 업데이트!
        //        PlayerStateManager.instance.JoinedPlayerStateUpdate(characterName);
        //        character = characterName;
        //    }
        //}
    }











    // 플레이어가 나가면 자동으로 실행
    // 플레이어의 접속 상태를 "꺼짐"으로 변경
    [PunRPC]
    public void RpcLeftPlayer(string name)
    {
        PlayerStateManager.instance.LeavePlayerStateUpdate(InfoManager.Instance.Character);
        print($"떠난 플레이어의 캐릭터 {name} 삭제");
    }
    
    
    public void CallRpcLeftPlayer()
    {
        if(InfoManager.Instance.visitType == "Island02")
        {
            // 플레이어 접속 상태 꺼짐으로 변경
            photonView.RPC(nameof(RpcLeftPlayer), RpcTarget.AllBuffered, InfoManager.Instance.Character);
        }
    }

    // 방 나가기
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        CallRpcLeftPlayer();

    }














    // setting ui
    public void SettingUI()
    {
        SettingUIInfo info = GameManager.instance.settingUIInfo;
        info.nickName.text = PhotonNetwork.NickName;
        
        Texture2D picture = Resources.Load<Texture2D>("Member/" + InfoManager.Instance.dicMemberCharacter[PhotonNetwork.NickName]);
        info.profileImg.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));

        info.familyCode.text = InfoManager.Instance.FamilyCode; // infomanager에서 명시
    }











    // 까망이 터치하면 묘~ 하고 울기
    public void TouchKkamang()
    {
        // 클릭하면
        if (Input.GetMouseButtonDown(0))
        {
            if(Camera.main == null)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 선택한 플레이어 정보를 CharacterManager의 CurrentCharacterName에 저장한다.
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "Kkamang")
                {
                    SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Meow);
                }
            }
        }
    }
}
