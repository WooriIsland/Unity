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

    // instance�� ����ؼ� chat client�� ����Ѵ�.
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

        // �ؽ�Ʈ�� �ۼ��ϰ� ���͸� ������ ȣ��Ǵ� �Լ� ���
        inputField.onSubmit.AddListener(OnSubmit);

        // photon chat �ʱ� ����
        PhotonChatSetting();

        // �ʱ� ������ �������� photon chat ����
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

    // ä��â���� ���͸� ������ ����Ǵ� �Լ�

    float prevContentH; // ���ο� ä���� �߰��Ǳ� ���� content�� H���� ����
    void OnSubmit(string text)
    {
        prevContentH = content.sizeDelta.y;

        text = inputField.text;
        int currChannelIdx = 0; // �ӽ�

        chatClient.PublishMessage(channelNames[currChannelIdx], text); // ä�� ������ �Լ�

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

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string aiJsonData = JsonUtility.ToJson(chatInfo, true);
        print(aiJsonData);

        //AI�� ä���� �Ѵ�!
        OnGetPost(aiJsonData);

        // inputChat ���� �ʱ�ȭ
        inputField.text = "";

        // inputChat ������ ���õ� ���·�
        inputField.ActivateInputField();
    }

    //Ai
    // ���� ���� �� -> ê�� ������ ����
    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
    public void OnGetPost(string s)
    {
        string url = "http://221.163.19.218:1221/api/chatbot/conversation";

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester_LHS requester = new HttpRequester_LHS();

        requester.SetUrl(RequestType.POST, url, false);
        requester.body = s; // json data
        requester.isJson = true;
        requester.isChat = false; // �̰� ����

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager_LHS.instance.SendRequest(requester);
    }

    public ChatBotResponse chatBotResponse;
    void OnGetPostComplete(DownloadHandler result)
    {
        print("Chat ����");

        //HttpAiPhotoData aiPhotoData = new HttpAiPhotoData();
        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);

        
        print(result.text);

        

        chatBotResponse = new ChatBotResponse();
        chatBotResponse = JsonUtility.FromJson<ChatBotResponse>(result.text);

        if(chatBotResponse.task == "���")
        {
            return;
        }


        // ����ȯ
        // ä�ÿ� result.text����ϱ�
        int currChannelIdx = 0; // �ӽ�

        // chatItem ������ (scrollView -> content �� �ڽ����� ���)
        GameObject go = Instantiate(chatItemFactory, trContent);

        // ������ ���ӿ�����Ʈ���� ChatItem ������Ʈ �����´�.
        PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        // ����, ���θ� �����ϰ�
        item.SetText(chatBotResponse.answer, Color.black);

        // ������ ������Ʈ���� SetText �Լ� ����
        item.SetText("����� : " + chatBotResponse.answer, Color.black);


        // ����ȭ
        chatClient.PublishMessage(channelNames[currChannelIdx], chatBotResponse.answer); // ä�� ������ �Լ�



        //downloadHandler�� �޾ƿ� Json���� ������ �����ϱ�
        //2.FromJson���� ���� �ٲ��ֱ�
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);

        //-----------------ê�� �ֱ�--------------

        //if (aiPhotoData.results.body.response.Trim() == "") return;
    }

    void OnGetPostFailed()
    {
        print("Chat ����");
    }

    public void OnClickSendBtn()
    {

        print(nameof(OnClickSendBtn));
        string text = inputField.text;
        int currChannelIdx = 0; // �ӽ�
        chatClient.PublishMessage(channelNames[currChannelIdx], text);

        // inputChat ���� �ʱ�ȭ
        inputField.text = "";

        // inputChat ������ ���õ� ���·�
        inputField.ActivateInputField();

        StartCoroutine(AutoScrollBottom());
    }

    void PhotonChatSetting()
    {
        //���� ������ �����ͼ� ChatAppSettings �� ��������.
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

        // ä���� �� NickName ����
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName);

        // �ʱ⼳���� �̿��ؼ� ä�ü����� ���� �õ�
        chatClient.ConnectUsingSettings(chatAppSettings);
    }


    // ä���� ���� �������� ����
    public GameObject chatItemFactory;
    public Transform trContent;
    void CreateChat(string sender, string message, Color color)
    {
        // chatItem ������ (scrollView -> content �� �ڽ����� ���)
        GameObject go = Instantiate(chatItemFactory, trContent);
        
        // ������ ���ӿ�����Ʈ���� ChatItem ������Ʈ �����´�.
        PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        // ����, ���θ� �����ϰ�
        item.SetText(message, color);

        // ������ ������Ʈ���� SetText �Լ� ����
        item.SetText(sender + " : " + message, color);
    }



#if PC
    public void OnClickChatBtn()
    {
        if(isChatRoomActive) // true�� �� ������? ��, ä�÷��� ������
        {
            //clickMove.canMove = true;

            isChatRoomActive = false;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = false;
            chatExcept.SetActive(isChatExcept);
        }

        else if(!isChatRoomActive) // false�� �� ������? ��, ä�÷��� ������
        {
            //clickMove.canMove = false;

            isChatRoomActive = true;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = true;
            chatExcept.SetActive(isChatExcept);
        }
    }

    // chatRoom�� ����Ǵ� �߿�
    // ����� Ŭ���ϸ� chatRoom�� ��Ȱ��ȭ�ȴ�.
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
        // ä�÷��� �����ִ� ���¿��� �� ui�� �����ϸ� ä�÷��� �������.
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
        print("**** ä�� ���� ���� ���� ****");
        // ä�� �߰�
        if (channelNames.Count > 0)
        {
            chatClient.Subscribe(channelNames.ToArray());
        }

        // ���� ���¸� �¶������� �Ѵ�.
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
            print("**** ä�� [" + channels[i] + "] �߰� ����");
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

    // scroll view�� chatitem�� �������� �ڵ����� ��ũ���� �ֽ� chat���� �����ش�.
    public RectTransform scrollView;
    public RectTransform content;
    IEnumerator AutoScrollBottom()
    {
        yield return 0;


        // ���� chat item�� scroll view���� Ŀ����
        if (content.sizeDelta.y > scrollView.sizeDelta.y)
        {
            // ���������� ���۵� ä���� scroll view �ٴڿ� ��Ҵٸ�?
            if (prevContentH - scrollView.sizeDelta.y <= scrollView.anchoredPosition.y) // position : 3D������ �Ǻ� ��ġ, anchoredPosition�� ���� �ν����� â�� ������ x, y���� �������
            {
                // content�� y���� �缳���Ѵ�.
                content.anchoredPosition = new Vector2(0, content.sizeDelta.y - scrollView.sizeDelta.y);
            }

            // content�� y���� ���� ���۵� ä���� y����ŭ ������Ų��.
        }
    }
}
