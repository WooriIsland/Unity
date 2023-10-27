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

    // chat �Ѱ��ϴ� ��ü
    ChatClient chatClient;

    // �⺻ ä�� ä�� ���
    public List<string> channelNames = new List<string>();

    // ���� ���õ� ä��
    int currChannelIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputChat.onSubmit.AddListener((string s) =>
        {
            chatClient.PublishMessage(channelNames[currChannelIdx], s);

            // ä���� ������ ����
            // inputChat �ʱ�ȭ
            inputChat.text = "";
            // inputChat ���� ����
            inputChat.ActivateInputField();
        });

        // photon chat �ʱ⼳��
        PhotonChatSetting();

        // ���ӽõ�
        Connect();
    }

    void PhotonChatSetting()
    {
        // photon ���� �����ͼ� chat app settings�� �����ϱ�
        AppSettings photonSettings = PhotonNetwork.PhotonServerSettings.AppSettings;

        // �� ������ ������ ChatAppSettings ����
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

        // ä���� �� �г��� ����
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues("�ڵ���");

        // �ʱ⼳���� �̿��ؼ� ä�ü����� ���� �õ�
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    void CreateChat(string sender, string message, Color color)
    {
        // chatItem ������ (scrollView -> content �� �ڽ����� ���)
        GameObject go = Instantiate(chatItemFactory, trContent);
        // ������ ���ӿ�����Ʈ���� ChatItem ������Ʈ �����´�.
        PhotonChatItem item = go.GetComponent<PhotonChatItem>();
        // ������ ������Ʈ���� SetText �Լ� ����
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
