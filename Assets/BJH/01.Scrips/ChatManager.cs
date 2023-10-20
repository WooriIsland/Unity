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

public class ChatManager : MonoBehaviour, IPointerDownHandler, IChatClientListener
{
    // Chat UI, Player Move
    public Button chatBtn;
    public GameObject chatRoom;
    public GameObject chatExcept;
    bool isChatRoomActive = true;
    bool isChatExcept = true;

    public GameObject myPlayer;
    MovePlayer clickMove;


    // Photon Chat
    ChatAppSettings chatAppSettings;
    ChatClient chatClient;


    public List<string> channelNames;
    public InputField inputField;

    void Start()
    {
        isChatRoomActive = false;
        isChatExcept = false;
        chatRoom.SetActive(isChatRoomActive);
        chatExcept.SetActive(isChatExcept);

        clickMove = myPlayer.GetComponentInChildren<MovePlayer>();

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
    void OnSubmit(string text)
    {
        print(nameof(OnSubmit));

        text = inputField.text;
        int currChannelIdx = 0; // 임시



        chatClient.PublishMessage(channelNames[currChannelIdx], text);

        // inputChat 내용 초기화
        inputField.text = "";

        // inputChat 강제로 선택된 상태로
        inputField.ActivateInputField();
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
            clickMove.canMove = true;

            isChatRoomActive = false;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = false;
            chatExcept.SetActive(isChatExcept);
        }
        else if(!isChatRoomActive) // false일 때 누르면? 즉, 채팅룸이 켜지면
        {
            clickMove.canMove = false;

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
}
