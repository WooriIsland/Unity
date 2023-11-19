using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Net.NetworkInformation;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;
using UnityEngine.Networking;
using System;
using TMPro;
using System.Text;
using System.IO;
using UnityEngine.UIElements;

public class ChatBotResponse
{
    public string answer;
    public string task;
    public string data;
    public string island_id;
}

public class ChatManager : MonoBehaviourPun, IPointerDownHandler, IChatClientListener
{
    // chat
    public GameObject chatBG, yellow, white, black, date;
    public RectTransform rtContent;
    public TMP_InputField chatInput;
    public Scrollbar scrollbar;
    public GameObject alert;

    // chat rooms
    public List<string> chatChannelNames;

    // player
    public GameObject myPlayer;
    PlayerMove clickMove;

    // bool
    bool isChatRoomActive = false;

    // Photon Chat
    ChatAppSettings chatAppSettings;
    ChatClient chatClient;


    // instance를 사용해서 chat client를 사용한다.
    private static ChatManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public static ChatManager Instance
    {
        get
        {
            return instance;
        }
    }



    void Start()
    {
        isChatRoomActive = false;
        alert.SetActive(false);
        chatBG.SetActive(false);

        clickMove = myPlayer.GetComponentInChildren<PlayerMove>();

        // 텍스트를 작성하고 엔터를 쳤을때 호출되는 함수 등록
        chatInput.onSubmit.AddListener(OnSubmit);

        // photon chat 초기 설정
        PhotonChatSetting();

        // 초기 설정을 바탕으로 photon chat 입장
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }

        // 테스트
        if(Input.GetKeyDown(KeyCode.Keypad4))
        {
            print(photonView.Owner.NickName);
        }
    }

    // 채팅창에서 엔터를 누르면 실행되는 함수

    void OnSubmit(string text)
    {
        if(text.Length == 0)
        {
            return;
        }

        // chatInput에 받아온 text를 photon chat을 사용해서 전송
        chatInput.text = text;
        chatClient.PublishMessage(chatChannelNames[0], text);

        // chatInput 내용 초기화
        chatInput.text = "";

        // chatInput 강제로 선택된 상태로
        chatInput.ActivateInputField();

        if (text.Contains("까망"))
        {
            print("까망이를 호출했습니다.");
            StartCoroutine(CoKkamangWatingMent());
        }

        if (isChatRoomActive == false)
        {
            alert.SetActive(true);
        }

        // ---------------------------------------------------------------------------------

        ChatInfo chatInfo = new ChatInfo();

        string island_id = InfoManager.Instance.FamilyCode;
        string user_id = PhotonNetwork.NickName;

        DateTime currentTime = DateTime.Now;
        string datetiem = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

        chatInfo.island_id = island_id;
        chatInfo.user_id = user_id;
        chatInfo.content = text;
        chatInfo.datetiem = datetiem;

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(chatInfo, true);
        print(aiJsonData);

        //AI와 채팅을 한다!
        OnGetPost(aiJsonData);
    }

    // 까망이 대기 멘트 델리게이트
    IEnumerator CoKkamangWatingMent()
    {
        yield return new WaitForSeconds(2f);

        photonView.RPC("PunSendKkamangChat", RpcTarget.All, "알겠다냥, 잠시만 기다려보라냥!");

    }

    //Ai
    // 엔터 쳤을 때 -> 챗봇 보내는 내용
    // 서버에 게시물 조회 요청 -> HttpManager한테 알려주려고 함
    public void OnGetPost(string s)
    {
        string url = "http://221.163.19.218:1221/api/chatbot/conversation";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s; // json data
        requester.isJson = true;
        requester.isChat = false; // 이거 뭐지

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        print(requester);

        HttpManager_LHS.instance.SendRequest(requester);
    }

    public ChatBotResponse chatBotResponse;
    void OnGetPostComplete(DownloadHandler result)
    {
        //HttpAiPhotoData aiPhotoData = new HttpAiPhotoData();
        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);


        print(result.text);

        chatBotResponse = new ChatBotResponse();
        chatBotResponse = JsonUtility.FromJson<ChatBotResponse>(result.text);

        if(chatBotResponse.task == "대기" || chatBotResponse.answer.Length <= 0 || chatBotResponse.answer == "No Response")
        {
            return;
        }

        photonView.RPC(nameof(PunSendKkamangChat), RpcTarget.All, chatBotResponse.answer);
        //// 생성된 게임오브젝트에서 ChatItem 컴포넌트 가져온다.
        //PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        //// 가로, 세로를 세팅하고
        //item.SetText(chatBotResponse.answer, Color.black);

        //// 가져온 컴포넌트에서 SetText 함수 실행
        //item.SetText("까망이 : " + chatBotResponse.answer, Color.black);


        // 동기화
        //chatClient.PublishMessage(chatChannelNames[0], chatBotResponse.answer); // 채팅 보내는 함수



        //downloadHandler에 받아온 Json형식 데이터 가공하기
        //2.FromJson으로 형식 바꿔주기
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);

        //-----------------챗봇 넣기--------------

        //if (aiPhotoData.results.body.response.Trim() == "") return;
    }

    [PunRPC]
    public void PunSendKkamangChat(string chat)
    {
        print("까망이 채팅 PunRpc");

        // 변지환
        // 채팅에 result.text출력하기
        int currChannelIdx = 0; // 임시

        // chatItem 생성함 (scrollView -> content 의 자식으로 등록)
        GameObject go = Instantiate(black, rtContent.transform);
        print("까망이 채팅 생성");

        AreaScript area = go.GetComponent<AreaScript>();

        // 가로는 최대 600, 세로는 boxRect의 기존 사이즈대로
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = chat;

        area.userNameText.text = "까망이";

        // 텍스트의 엔터 때문에 텍스트는 크고 박스는 작고.. 이럴 수 있어서
        // 리빌딩(?)
        Fit(area.boxRect);


        // 두 줄 이상이면 크기를 줄여가면서,
        // 한 줄이 아래로 내려가는 시점 바로 전 크기를 가로에 대입
        float x = area.textRect.sizeDelta.x + 42;
        float y = area.textRect.sizeDelta.y;

        if (y > 49) // 텍스트가 3줄 이상
        {
            for (int i = 0; i < 200; i++)
            {
                area.boxRect.sizeDelta = new Vector2(x - i * 2, area.boxRect.sizeDelta.y);

                Fit(area.boxRect);

                if (area.boxRect.sizeDelta.x <= 100)
                {
                    break;
                }

                if (y != area.textRect.sizeDelta.y)
                {
                    area.boxRect.sizeDelta = new Vector2(x - (i * 2) + 2, y);
                    break;
                }
            }
        }
        else
        {
            area.boxRect.sizeDelta = new Vector2(x, y);
        }

        Invoke("ScrollDelay", 0.03f);
    }

    void OnGetPostFailed()
    {
        print("Chat 실패");
    }

    // 버튼을 누르면 채팅이 전송됨
    public void OnClickSendBtn()
    {
        if(chatInput.text.Length == 0)
        {
            return;
        }

        string text = chatInput.text;
        int currChannelIdx = 0; // 임시
        chatClient.PublishMessage(chatChannelNames[0], text);

        // inputChat 내용 초기화
        chatInput.text = "";

        // inputChat 강제로 선택된 상태로
        chatInput.ActivateInputField();
    }

    // 포톤 초기 설정
    void PhotonChatSetting()
    {
        //포톤 설정을 가져와서 ChatAppSettings 에 설정하자.
        AppSettings photonSettings = PhotonNetwork.PhotonServerSettings.AppSettings;

        // 위 설정을 가지고 ChatAppSettings 셋팅
        chatAppSettings = new ChatAppSettings();
        chatAppSettings.AppIdChat = photonSettings.AppIdChat;
        chatAppSettings.AppVersion = photonSettings.AppVersion;
        chatAppSettings.FixedRegion = photonSettings.FixedRegion;
        chatAppSettings.NetworkLogging = photonSettings.NetworkLogging;
        chatAppSettings.Protocol = photonSettings.Protocol;
        chatAppSettings.EnableProtocolFallback = photonSettings.EnableProtocolFallback;
        chatAppSettings.Server = photonSettings.Server;
        chatAppSettings.Port = (ushort)photonSettings.Port;
        chatAppSettings.ProxyServer = photonSettings.ProxyServer;
    }
    
    // 설정을 토대로 연결
    void Connect()
    {
        chatClient = new ChatClient(this);

        // 채팅할 때 NickName 설정
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName);

        // 초기설정을 이용해서 채팅서버에 연결 시도
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    // 채팅을 보내는 함수
    void CreateChat(string sender, string text, Color color)
    {
        GameObject go;
        AreaScript area;

        // 내가 보낸거라면?
        if (sender == PhotonNetwork.NickName)
        {
            print("내가 보냄");
            go = Instantiate(yellow, rtContent);
            area = go.GetComponent<AreaScript>();
        }
        else
        {
            print("상대가 보냄");
            go = Instantiate(white, rtContent);
            area = go.GetComponent<AreaScript>();
            area.userNameText.text = sender;

            // 상대의 프로필 이미지 가져오기
            photonView.RPC("setProfile", RpcTarget.All);
        }


        // 가로는 최대 600, 세로는 boxRect의 기존 사이즈대로
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = text;

        // 텍스트의 엔터 때문에 텍스트는 크고 박스는 작고.. 이럴 수 있어서
        // 리빌딩(?)
        Fit(area.boxRect);


        // 두 줄 이상이면 크기를 줄여가면서,
        // 한 줄이 아래로 내려가는 시점 바로 전 크기를 가로에 대입
        float x = area.textRect.sizeDelta.x + 42;
        float y = area.textRect.sizeDelta.y;

        if (y > 49) // 텍스트가 3줄 이상
        {
            for (int i = 0; i < 200; i++)
            {
                area.boxRect.sizeDelta = new Vector2(x - i * 2, area.boxRect.sizeDelta.y);

                Fit(area.boxRect);

                if (area.boxRect.sizeDelta.x <= 100)
                {
                    break;
                }

                if (y != area.textRect.sizeDelta.y)
                {
                    area.boxRect.sizeDelta = new Vector2(x - (i * 2) + 2, y);
                    break;
                }
            }
        }
        else
        {
            area.boxRect.sizeDelta = new Vector2(x, y);
        }

        area.timeText.text = DateTime.Now.ToString("HH:mm");

        // 시간
        //DateTime t = DateTime.Now;
        //area.time = t.ToString("yyyy-MM-dd-HH-dd");
        //area.user = sender;

        //// 현재 것은 항상 새로운 시간 대입
        //int hour = t.Hour;
        //if (t.Hour == 0)
        //{
        //    hour = 12;
        //}
        //else if (t.Hour > 12)
        //{
        //    hour -= 12;
        //}
        //area.timeText.text = (t.Hour > 12 ? "오후" : "오전") + hour + " : " + t.Minute.ToString("D2");


        // 이전 것과 날짜가 다르면 날짜영역 보이기
        //if (lastArea != null && lastArea.time.Substring(0, 10) != area.time.Substring(0, 10))
        //{
        //    Transform curDataArea = Instantiate(date).transform;
        //    curDataArea.SetParent(rtContent.transform, false);
        //    curDataArea.SetSiblingIndex(curDataArea.GetSiblingIndex() - 1);

        //    string week = "";
        //    switch (t.DayOfWeek)
        //    {
        //        case DayOfWeek.Sunday:
        //            week = "일";
        //            break;
        //        case DayOfWeek.Monday:
        //            week = "월";
        //            break;
        //        case DayOfWeek.Tuesday:
        //            week = "화";
        //            break;
        //        case DayOfWeek.Wednesday:
        //            week = "수";
        //            break;
        //        case DayOfWeek.Thursday:
        //            week = "목";
        //            break;
        //        case DayOfWeek.Friday:
        //            week = "금";
        //            break;
        //        case DayOfWeek.Saturday:
        //            week = "토";
        //            break;
        //    }
        //    curDataArea.GetComponent<AreaScript>().dataText.text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";

        //}


        // 스크롤바가 위로 올라간 상태에서 새 메시지를 받으면 맨 아래로 내리지 않음
        //if (!isSend && !isBottom)
        //{
        //    return;
        //}
        Invoke("ScrollDelay", 0.03f);



        // chatItem 생성함 (scrollView -> content 의 자식으로 등록)
        //GameObject go = Instantiate(chatItemFactory, trContent);

        // 생성된 게임오브젝트에서 ChatItem 컴포넌트 가져온다.
        //PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        // 가로, 세로를 세팅하고
        //item.SetText(message, color);

        // 가져온 컴포넌트에서 SetText 함수 실행
        //item.SetText(sender + " : " + message, color);
    }

    // 강제로 채팅박스 조정
    void Fit(RectTransform rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

    void ScrollDelay() => scrollbar.value = 0;

    [PunRPC]
    public void setProfile()
    {

    }

    public void OnclickCloseBtn()
    {
        isChatRoomActive = false;
        chatBG.SetActive(false);
    }

    public void OnClickChatBtn()
    {
        isChatRoomActive = true;
        chatBG.SetActive(true);

    }


#if PC
    //public void OnClickChatBtn()
    //{
    //    if(isChatRoomActive) // true일 때 누르면? 즉, 채팅룸이 꺼지면
    //    {
    //        //clickMove.canMove = true;

    //        isChatRoomActive = false;
    //        chatScrollView.SetActive(isChatRoomActive);

    //        isChatExcept = false;
    //        chatExcept.SetActive(isChatExcept);
    //    }

    //    else if(!isChatRoomActive) // false일 때 누르면? 즉, 채팅룸이 켜지면
    //    {
    //        //clickMove.canMove = false;

    //        isChatRoomActive = true;
    //        chatScrollView.SetActive(isChatRoomActive);

    //        isChatExcept = true;
    //        chatExcept.SetActive(isChatExcept);
    //    }
    //}

    //// chatRoom이 실행되는 중에
    //// 배경을 클릭하면 chatRoom이 비활성화된다.
    //private void OnMouseDown()
    //{
    //    if (isChatRoomActive)
    //    {
    //        isChatRoomActive = false;
    //        chatScrollView.SetActive(isChatRoomActive);

    //        isChatExcept = false;
    //        chatExcept.SetActive(isChatExcept);
    //    }
    //}
#endif

    public void OnPointerDown(PointerEventData eventData)
    {
        //    // 채팅룸이 열려있는 상태에서 빈 ui를 선택하면 채팅룸이 사라진다.
        //    if(isChatRoomActive == true && EventSystem.current.IsPointerOverGameObject(eventData.pointerId)) {
        //        chatScrollView.SetActive(false);
        //        isChatRoomActive = false;
        //    }
    }


public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnDisconnected()
    {
    }

    public void OnConnected()
    {
        print("**** 채팅 서버 접속 성공 ****");
        // 채널 추가
        if (chatChannelNames.Count > 0)
        {
            chatClient.Subscribe(chatChannelNames.ToArray());
        }

        // 나의 상태를 온라인으로 한다.
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            CreateChat(senders[i], messages[i].ToString(), Color.black);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            print("**** 채널 [" + channels[i] + "] 추가 성공");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    // scroll view에 chatitem이 많아지면 자동으로 스크롤을 최신 chat으로 내려준다.
    //public RectTransform scrollView;
    //public RectTransform rtContent;
    //IEnumerator AutoScrollBottom()
    //{
    //    yield return 0;


    //    // 만약 chat item이 scroll view보다 커지면
    //    if (rtContent.sizeDelta.y > scrollView.sizeDelta.y)
    //    {
    //        // 마지막으로 전송된 채팅이 scroll view 바닥에 닿았다면?
    //        if (prevContentH - scrollView.sizeDelta.y <= scrollView.anchoredPosition.y) // position : 3D세상의 피봇 위치, anchoredPosition이 실제 인스펙터 창에 나오는 x, y값이 들어있음
    //        {
    //            // content의 y값을 재설정한다.
    //            rtContent.anchoredPosition = new Vector2(0, rtContent.sizeDelta.y - scrollView.sizeDelta.y);
    //        }

    //        // content의 y값을 새로 전송된 채팅의 y값만큼 증가시킨다.
    //    }
    //}
}
