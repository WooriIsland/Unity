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
using System.Text;
using System.IO;

public class ChatBotResponse
{
    public string answer;
    public string task;
    public string data;
    public string island_id;
}

public class ChatManager : MonoBehaviourPun, IPointerDownHandler, IChatClientListener
{
    // Chat UI, Player Move
    public Button chatBtn;
    public GameObject chatRoom;
    public GameObject chatExcept;
    bool isChatRoomActive = true;
    bool isChatExcept = true;

    public GameObject myPlayer;
    PlayerMove clickMove;


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

    public List<string> channelNames;
    public InputField inputField;

    void Start()
    {
        isChatRoomActive = false;
        isChatExcept = false;
        chatRoom.SetActive(isChatRoomActive);
        chatExcept.SetActive(isChatExcept);

        clickMove = myPlayer.GetComponentInChildren<PlayerMove>();

        // 텍스트를 작성하고 엔터를 쳤을때 호출되는 함수 등록
        inputField.onSubmit.AddListener(OnSubmit);

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
    }

    // 채팅창에서 엔터를 누르면 실행되는 함수

    float prevContentH; // 새로운 채팅이 추가되기 전의 content의 H값을 저장
    void OnSubmit(string text)
    {
        prevContentH = content.sizeDelta.y;

        text = inputField.text;
        int currChannelIdx = 0; // 임시

        chatClient.PublishMessage(channelNames[currChannelIdx], text); // 채팅 보내는 함수

        // ---------------------------------------------------------------------------------

        ChatInfo chatInfo = new ChatInfo();

        string island_id = "family123";
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

        // inputChat 내용 초기화
        inputField.text = "";

        // inputChat 강제로 선택된 상태로
        inputField.ActivateInputField();
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

        HttpManager_LHS.instance.SendRequest(requester);
    }

    public ChatBotResponse chatBotResponse;
    void OnGetPostComplete(DownloadHandler result)
    {
        print("Chat 성공");

        //HttpAiPhotoData aiPhotoData = new HttpAiPhotoData();
        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);

        
        print(result.text);

        

        chatBotResponse = new ChatBotResponse();
        chatBotResponse = JsonUtility.FromJson<ChatBotResponse>(result.text);

        if(chatBotResponse.task == "대기")
        {
            return;
        }


        // 변지환
        // 채팅에 result.text출력하기
        int currChannelIdx = 0; // 임시

        // chatItem 생성함 (scrollView -> content 의 자식으로 등록)
        GameObject go = Instantiate(chatItemFactory, trContent);

        // 생성된 게임오브젝트에서 ChatItem 컴포넌트 가져온다.
        PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        // 가로, 세로를 세팅하고
        item.SetText(chatBotResponse.answer, Color.black);

        // 가져온 컴포넌트에서 SetText 함수 실행
        item.SetText("까망이 : " + chatBotResponse.answer, Color.black);


        // 동기화
        chatClient.PublishMessage(channelNames[currChannelIdx], chatBotResponse.answer); // 채팅 보내는 함수



        //downloadHandler에 받아온 Json형식 데이터 가공하기
        //2.FromJson으로 형식 바꿔주기
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);

        //-----------------챗봇 넣기--------------

        //if (aiPhotoData.results.body.response.Trim() == "") return;
    }

    void OnGetPostFailed()
    {
        print("Chat 실패");
    }

    public void OnClickSendBtn()
    {

        print(nameof(OnClickSendBtn));
        string text = inputField.text;
        int currChannelIdx = 0; // 임시
        chatClient.PublishMessage(channelNames[currChannelIdx], text);

        // inputChat 내용 초기화
        inputField.text = "";

        // inputChat 강제로 선택된 상태로
        inputField.ActivateInputField();

        StartCoroutine(AutoScrollBottom());
    }

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



    void Connect()
    {
        chatClient = new ChatClient(this);

        // 채팅할 때 NickName 설정
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName);

        // 초기설정을 이용해서 채팅서버에 연결 시도
        chatClient.ConnectUsingSettings(chatAppSettings);
    }


    // 채팅을 만들어서 컨텐츠에 삽입
    public GameObject chatItemFactory;
    public Transform trContent;
    void CreateChat(string sender, string message, Color color)
    {
        // chatItem 생성함 (scrollView -> content 의 자식으로 등록)
        GameObject go = Instantiate(chatItemFactory, trContent);
        
        // 생성된 게임오브젝트에서 ChatItem 컴포넌트 가져온다.
        PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        // 가로, 세로를 세팅하고
        item.SetText(message, color);

        // 가져온 컴포넌트에서 SetText 함수 실행
        item.SetText(sender + " : " + message, color);
    }



#if PC
    public void OnClickChatBtn()
    {
        if(isChatRoomActive) // true일 때 누르면? 즉, 채팅룸이 꺼지면
        {
            //clickMove.canMove = true;

            isChatRoomActive = false;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = false;
            chatExcept.SetActive(isChatExcept);
        }

        else if(!isChatRoomActive) // false일 때 누르면? 즉, 채팅룸이 켜지면
        {
            //clickMove.canMove = false;

            isChatRoomActive = true;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = true;
            chatExcept.SetActive(isChatExcept);
        }
    }

    // chatRoom이 실행되는 중에
    // 배경을 클릭하면 chatRoom이 비활성화된다.
    private void OnMouseDown()
    {
        if (isChatRoomActive)
        {
            isChatRoomActive = false;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = false;
            chatExcept.SetActive(isChatExcept);
        }
    }
#endif

    public void OnPointerDown(PointerEventData eventData)
    {
        // 채팅룸이 열려있는 상태에서 빈 ui를 선택하면 채팅룸이 사라진다.
        if(isChatRoomActive == true && EventSystem.current.IsPointerOverGameObject(eventData.pointerId)) {
            chatRoom.SetActive(false);
            isChatRoomActive = false;
        }
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
        if (channelNames.Count > 0)
        {
            chatClient.Subscribe(channelNames.ToArray());
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
    public RectTransform scrollView;
    public RectTransform content;
    IEnumerator AutoScrollBottom()
    {
        yield return 0;


        // 만약 chat item이 scroll view보다 커지면
        if (content.sizeDelta.y > scrollView.sizeDelta.y)
        {
            // 마지막으로 전송된 채팅이 scroll view 바닥에 닿았다면?
            if (prevContentH - scrollView.sizeDelta.y <= scrollView.anchoredPosition.y) // position : 3D세상의 피봇 위치, anchoredPosition이 실제 인스펙터 창에 나오는 x, y값이 들어있음
            {
                // content의 y값을 재설정한다.
                content.anchoredPosition = new Vector2(0, content.sizeDelta.y - scrollView.sizeDelta.y);
            }

            // content의 y값을 새로 전송된 채팅의 y값만큼 증가시킨다.
        }
    }
}
