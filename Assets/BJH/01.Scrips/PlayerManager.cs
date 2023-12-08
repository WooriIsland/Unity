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

            // 접속한것으로 셋팅  
            PlayerStateManager.instance.ChangeOffLine(photonView.Owner.NickName, false);

        }



        // animaotr 변수 가져오기
        animator = playerMove.animator;



        // 입장하면 GameManager의 userList에 자신의 photonview ID를 추가
        GameManager.instance.userList.Add(photonView.ViewID);
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


    private void Update()
    {
        // 애니메이션 테스트
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < animator.Length; i++)
            {
                if (animator[i].gameObject.activeSelf == true)
                {
                    animator[i].SetTrigger("Punch");
                    break;
                }
            }
        }



        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = aniCam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;


        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.transform.gameObject.CompareTag("Player"))
        //        {
        //            // 나
        //            if (hit.transform.gameObject.GetComponent<PhotonView>().IsMine == true)
        //            {
        //                print("나를 클릭했으니 춤추자");
        //                for (int i = 0; i < animator.Length; i++)
        //                {
        //                    if (animator[i].gameObject.activeSelf == false)
        //                    {
        //                        continue;
        //                    }
        //                    aniTemp = i;

        //                    photonView.RPC("PunDance", RpcTarget.AllBuffered);


        //                    //일단 주석 앨범부분에서 소리 날 수 도 있기 때문에
        //                    //SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.Glitter);

        //                }
        //            }
        //            // 내가 아니면
        //            else
        //            {
        //                // 인사하기
        //                for (int i = 0; i < animator.Length; i++)
        //                {
        //                    if (animator[i].gameObject.activeSelf == false)
        //                    {
        //                        continue;
        //                    }

        //                    print("상대를 클릭했으니까 인사하자");
        //                    aniTemp = i;

        //                    photonView.RPC("PunHello", RpcTarget.AllBuffered);
        //                }

        //            }
        //        }
        //    }
        //}


    }

    // 플레이어 카메라와 플레이어 상태를 껐다 켜는 함수
    /*    public void OnOff()
        {
            state = !state;

            foreach (GameObject go in players)
            {
                go.SetActive(state);
            }

            camera.gameObject.SetActive(state);
        }*/

    public void StartHello()
    {
        // 내 캐릭터의 photonView ID를 RPC에 넘겨
        string id = photonView.ViewID.ToString();
        photonView.RPC("PunAnimation", RpcTarget.All, id,"Hello");
    }

    public void StartPunch()
    {
        // 내 캐릭터의 photonView ID를 RPC에 넘겨
        string id = photonView.ViewID.ToString();
        photonView.RPC("PunAnimation", RpcTarget.All, id, "Punch");
    }


    // 애니메이션 RPC
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
                                go2.transform.GetChild(0).transform.GetChild(i).GetComponent<Animator>().SetTrigger(trigger); // canvas 객체를 참조하려고 하는건지 모르겠으나 rpc는 잘 됨
                                print(go2.transform.GetChild(0)); // Player List
                                print(go2.transform.GetChild(0).transform.GetChild(i)); // 플레이어가 선택한 게임 오브젝트가 잘 들어옴
                                SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);
                                break;
                            case "Punch":
                                go2.transform.GetChild(0).transform.GetChild(i).GetComponent<Animator>().SetTrigger(trigger); // canvas 객체를 참조하려고 하는건지 모르겠으나 rpc는 잘 됨
                                SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow); // 펀치로 변경해야됨
                                break;
                        }
                    }
                }
            }
        }
    }




    public void SelectModel(string characterName)
    {
        // rpc 함수로 캐릭터를 생성
        photonView.RPC(nameof(RpcSelectModel), RpcTarget.AllBuffered, characterName);
    }

    [PunRPC]
    void RpcSelectModel(string characterName)
    {
        // Player 프리팹 안에 들어있는 캐릭터 중
        foreach(Transform t in playerList.transform)
        {
            if(t.name == characterName)
            {
                t.gameObject.SetActive(true);
                break;
            }
        }

        // 들어오면서 캐릭터 채팅 프로필 정보 저장해주기
        ChatManager.Instance.dicAllPlayerProfile[nickName.text] = characterName;

        // 일반 맵
        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            // 입장한 플레이어 상태 업데이트
            PlayerStateManager.instance.JoinedPlayerStateUpdate(characterName);
            character = characterName;
            print("일반맵입니다.");

        }

        // 입장한 플레이어 리스트 업데이트
        //PlayerStateManager.instance.OnlinePlayers.Add(InfoManager.Instance.Character);
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

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        CallRpcLeftPlayer();

    }


    [PunRPC]
    public void PunHello()
    {
        animator[aniTemp].SetTrigger("Hello");
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);
    }


    [PunRPC]
    public void PunDance()
    {
        print("pun 진입");
        animator[aniTemp].SetTrigger("Dance");
    }



    // 애니메이션 실행 후 3초 뒤 애니메이션 끄기
    IEnumerator CoFalseAnimationTrigger(string triggerName, float time)
    {
        animator[aniTemp].SetTrigger(triggerName);

        yield return new WaitForSeconds(time);

        animator[aniTemp].ResetTrigger(triggerName);
    }


}
