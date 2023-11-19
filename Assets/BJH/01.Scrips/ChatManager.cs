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



    void Start()
    {
        isChatRoomActive = false;
        alert.SetActive(false);
        chatBG.SetActive(false);

        clickMove = myPlayer.GetComponentInChildren<PlayerMove>();

        // �ؽ�Ʈ�� �ۼ��ϰ� ���͸� ������ ȣ��Ǵ� �Լ� ���
        chatInput.onSubmit.AddListener(OnSubmit);

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

        // �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.Keypad4))
        {
            print(photonView.Owner.NickName);
        }
    }

    // ä��â���� ���͸� ������ ����Ǵ� �Լ�

    void OnSubmit(string text)
    {
        if(text.Length == 0)
        {
            return;
        }

        // chatInput�� �޾ƿ� text�� photon chat�� ����ؼ� ����
        chatInput.text = text;
        chatClient.PublishMessage(chatChannelNames[0], text);

        // chatInput ���� �ʱ�ȭ
        chatInput.text = "";

        // chatInput ������ ���õ� ���·�
        chatInput.ActivateInputField();

        if (text.Contains("���"))
        {
            print("����̸� ȣ���߽��ϴ�.");
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

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string aiJsonData = JsonUtility.ToJson(chatInfo, true);
        print(aiJsonData);

        //AI�� ä���� �Ѵ�!
        OnGetPost(aiJsonData);
    }

    // ����� ��� ��Ʈ ��������Ʈ
    IEnumerator CoKkamangWatingMent()
    {
        yield return new WaitForSeconds(2f);

        photonView.RPC("PunSendKkamangChat", RpcTarget.All, "�˰ڴٳ�, ��ø� ��ٷ������!");

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

        if(chatBotResponse.task == "���" || chatBotResponse.answer.Length <= 0 || chatBotResponse.answer == "No Response")
        {
            return;
        }

        photonView.RPC(nameof(PunSendKkamangChat), RpcTarget.All, chatBotResponse.answer);
        //// ������ ���ӿ�����Ʈ���� ChatItem ������Ʈ �����´�.
        //PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        //// ����, ���θ� �����ϰ�
        //item.SetText(chatBotResponse.answer, Color.black);

        //// ������ ������Ʈ���� SetText �Լ� ����
        //item.SetText("����� : " + chatBotResponse.answer, Color.black);


        // ����ȭ
        //chatClient.PublishMessage(chatChannelNames[0], chatBotResponse.answer); // ä�� ������ �Լ�



        //downloadHandler�� �޾ƿ� Json���� ������ �����ϱ�
        //2.FromJson���� ���� �ٲ��ֱ�
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);

        //-----------------ê�� �ֱ�--------------

        //if (aiPhotoData.results.body.response.Trim() == "") return;
    }

    [PunRPC]
    public void PunSendKkamangChat(string chat)
    {
        print("����� ä�� PunRpc");

        // ����ȯ
        // ä�ÿ� result.text����ϱ�
        int currChannelIdx = 0; // �ӽ�

        // chatItem ������ (scrollView -> content �� �ڽ����� ���)
        GameObject go = Instantiate(black, rtContent.transform);
        print("����� ä�� ����");

        AreaScript area = go.GetComponent<AreaScript>();

        // ���δ� �ִ� 600, ���δ� boxRect�� ���� ��������
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = chat;

        area.userNameText.text = "�����";

        // �ؽ�Ʈ�� ���� ������ �ؽ�Ʈ�� ũ�� �ڽ��� �۰�.. �̷� �� �־
        // ������(?)
        Fit(area.boxRect);


        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭,
        // �� ���� �Ʒ��� �������� ���� �ٷ� �� ũ�⸦ ���ο� ����
        float x = area.textRect.sizeDelta.x + 42;
        float y = area.textRect.sizeDelta.y;

        if (y > 49) // �ؽ�Ʈ�� 3�� �̻�
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
        print("Chat ����");
    }

    // ��ư�� ������ ä���� ���۵�
    public void OnClickSendBtn()
    {
        if(chatInput.text.Length == 0)
        {
            return;
        }

        string text = chatInput.text;
        int currChannelIdx = 0; // �ӽ�
        chatClient.PublishMessage(chatChannelNames[0], text);

        // inputChat ���� �ʱ�ȭ
        chatInput.text = "";

        // inputChat ������ ���õ� ���·�
        chatInput.ActivateInputField();
    }

    // ���� �ʱ� ����
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
    
    // ������ ���� ����
    void Connect()
    {
        chatClient = new ChatClient(this);

        // ä���� �� NickName ����
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName);

        // �ʱ⼳���� �̿��ؼ� ä�ü����� ���� �õ�
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    // ä���� ������ �Լ�
    void CreateChat(string sender, string text, Color color)
    {
        GameObject go;
        AreaScript area;

        // ���� �����Ŷ��?
        if (sender == PhotonNetwork.NickName)
        {
            print("���� ����");
            go = Instantiate(yellow, rtContent);
            area = go.GetComponent<AreaScript>();
        }
        else
        {
            print("��밡 ����");
            go = Instantiate(white, rtContent);
            area = go.GetComponent<AreaScript>();
            area.userNameText.text = sender;

            // ����� ������ �̹��� ��������
            photonView.RPC("setProfile", RpcTarget.All);
        }


        // ���δ� �ִ� 600, ���δ� boxRect�� ���� ��������
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = text;

        // �ؽ�Ʈ�� ���� ������ �ؽ�Ʈ�� ũ�� �ڽ��� �۰�.. �̷� �� �־
        // ������(?)
        Fit(area.boxRect);


        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭,
        // �� ���� �Ʒ��� �������� ���� �ٷ� �� ũ�⸦ ���ο� ����
        float x = area.textRect.sizeDelta.x + 42;
        float y = area.textRect.sizeDelta.y;

        if (y > 49) // �ؽ�Ʈ�� 3�� �̻�
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

        // �ð�
        //DateTime t = DateTime.Now;
        //area.time = t.ToString("yyyy-MM-dd-HH-dd");
        //area.user = sender;

        //// ���� ���� �׻� ���ο� �ð� ����
        //int hour = t.Hour;
        //if (t.Hour == 0)
        //{
        //    hour = 12;
        //}
        //else if (t.Hour > 12)
        //{
        //    hour -= 12;
        //}
        //area.timeText.text = (t.Hour > 12 ? "����" : "����") + hour + " : " + t.Minute.ToString("D2");


        // ���� �Ͱ� ��¥�� �ٸ��� ��¥���� ���̱�
        //if (lastArea != null && lastArea.time.Substring(0, 10) != area.time.Substring(0, 10))
        //{
        //    Transform curDataArea = Instantiate(date).transform;
        //    curDataArea.SetParent(rtContent.transform, false);
        //    curDataArea.SetSiblingIndex(curDataArea.GetSiblingIndex() - 1);

        //    string week = "";
        //    switch (t.DayOfWeek)
        //    {
        //        case DayOfWeek.Sunday:
        //            week = "��";
        //            break;
        //        case DayOfWeek.Monday:
        //            week = "��";
        //            break;
        //        case DayOfWeek.Tuesday:
        //            week = "ȭ";
        //            break;
        //        case DayOfWeek.Wednesday:
        //            week = "��";
        //            break;
        //        case DayOfWeek.Thursday:
        //            week = "��";
        //            break;
        //        case DayOfWeek.Friday:
        //            week = "��";
        //            break;
        //        case DayOfWeek.Saturday:
        //            week = "��";
        //            break;
        //    }
        //    curDataArea.GetComponent<AreaScript>().dataText.text = t.Year + "�� " + t.Month + "�� " + t.Day + "�� " + week + "����";

        //}


        // ��ũ�ѹٰ� ���� �ö� ���¿��� �� �޽����� ������ �� �Ʒ��� ������ ����
        //if (!isSend && !isBottom)
        //{
        //    return;
        //}
        Invoke("ScrollDelay", 0.03f);



        // chatItem ������ (scrollView -> content �� �ڽ����� ���)
        //GameObject go = Instantiate(chatItemFactory, trContent);

        // ������ ���ӿ�����Ʈ���� ChatItem ������Ʈ �����´�.
        //PhotonChatItem item = go.GetComponent<PhotonChatItem>();

        // ����, ���θ� �����ϰ�
        //item.SetText(message, color);

        // ������ ������Ʈ���� SetText �Լ� ����
        //item.SetText(sender + " : " + message, color);
    }

    // ������ ä�ùڽ� ����
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
    //    if(isChatRoomActive) // true�� �� ������? ��, ä�÷��� ������
    //    {
    //        //clickMove.canMove = true;

    //        isChatRoomActive = false;
    //        chatScrollView.SetActive(isChatRoomActive);

    //        isChatExcept = false;
    //        chatExcept.SetActive(isChatExcept);
    //    }

    //    else if(!isChatRoomActive) // false�� �� ������? ��, ä�÷��� ������
    //    {
    //        //clickMove.canMove = false;

    //        isChatRoomActive = true;
    //        chatScrollView.SetActive(isChatRoomActive);

    //        isChatExcept = true;
    //        chatExcept.SetActive(isChatExcept);
    //    }
    //}

    //// chatRoom�� ����Ǵ� �߿�
    //// ����� Ŭ���ϸ� chatRoom�� ��Ȱ��ȭ�ȴ�.
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
        //    // ä�÷��� �����ִ� ���¿��� �� ui�� �����ϸ� ä�÷��� �������.
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
        print("**** ä�� ���� ���� ���� ****");
        // ä�� �߰�
        if (chatChannelNames.Count > 0)
        {
            chatClient.Subscribe(chatChannelNames.ToArray());
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
    //public RectTransform scrollView;
    //public RectTransform rtContent;
    //IEnumerator AutoScrollBottom()
    //{
    //    yield return 0;


    //    // ���� chat item�� scroll view���� Ŀ����
    //    if (rtContent.sizeDelta.y > scrollView.sizeDelta.y)
    //    {
    //        // ���������� ���۵� ä���� scroll view �ٴڿ� ��Ҵٸ�?
    //        if (prevContentH - scrollView.sizeDelta.y <= scrollView.anchoredPosition.y) // position : 3D������ �Ǻ� ��ġ, anchoredPosition�� ���� �ν����� â�� ������ x, y���� �������
    //        {
    //            // content�� y���� �缳���Ѵ�.
    //            rtContent.anchoredPosition = new Vector2(0, rtContent.sizeDelta.y - scrollView.sizeDelta.y);
    //        }

    //        // content�� y���� ���� ���۵� ä���� y����ŭ ������Ų��.
    //    }
    //}
}
