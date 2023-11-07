using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    public InputField inputChat;

    public GameObject chatItemFactory;

    public Transform trContent;

    // photon chat setting
    ChatAppSettings chatAppSettings;

    // chat 총괄하는 객체
    ChatClient chatClient;

    // 기본 채팅 채널 목록
    public List<string> channelNames = new List<string>();

    // 현재 선택된 채널
    int currChannelIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputChat.onSubmit.AddListener((string s) =>
        {
            chatClient.PublishMessage(channelNames[currChannelIdx], s);

            // 채팅을 보내고 나서
            // inputChat 초기화
            inputChat.text = "";
            // inputChat 강제 선택
            inputChat.ActivateInputField();
        });

        // photon chat 초기설정
        PhotonChatSetting();

        // 접속시도
        Connect();
    }

    void PhotonChatSetting()
    {
        // photon 설정 가져와서 chat app settings에 설정하기
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

        // 채팅할 때 닉네임 설정
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues("박동식");

        // 초기설정을 이용해서 채팅서버에 연결 시도
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    void CreateChat(string sender, string message, Color color)
    {
        // chatItem 생성함 (scrollView -> content 의 자식으로 등록)
        GameObject go = Instantiate(chatItemFactory, trContent);
        // 생성된 게임오브젝트에서 ChatItem 컴포넌트 가져온다.
        PhotonChatItem item = go.GetComponent<PhotonChatItem>();
        // 가져온 컴포넌트에서 SetText 함수 실행
        item.SetText(sender + " : " + message, color);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
    }

    public void OnDisconnected()
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}
