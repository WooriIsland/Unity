using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using System;
using TMPro;

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
    public GameObject _chatBG, _yellow, _white, _black, _date;
    public RectTransform _rtContent;
    public TMP_InputField _chatInput;
    public Scrollbar _scrollbar;

    // chat rooms
    public List<string> chatChannelNames;

    // Photon Chat
    ChatAppSettings chatAppSettings;
    ChatClient chatClient;


    PlayerMove clickMove;

    // bool
    bool isChatRoomActive = false;

    public GameObject message;



    // �ִϸ��̼�
    public GameObject particle;
    public Transform TrParticle;
    public GameObject createdParticle;
    public Transform kkamang;
    public Animator chatJump;



    // ������
    // ��� �÷��̾��� key : �г���, value : ĳ���� �̸�
    public Dictionary<string, string> dicAllPlayerProfile = new Dictionary<string, string>();


    void Start()
    {
        isChatRoomActive = false;
        // alert.SetActive(false);
        _chatBG.SetActive(false);

        // �ؽ�Ʈ�� �ۼ��ϰ� ���͸� ������ ȣ��Ǵ� �Լ� ���
        _chatInput.onSubmit.AddListener(OnSubmit);

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

    void OnSubmit(string text)
    {
        if (text.Length == 0)
        {
            return;
        }

        // chatInput�� �޾ƿ� text�� photon chat�� ����ؼ� ����
        _chatInput.text = text;

        chatClient.PublishMessage(chatChannelNames[0], text);

        // chatInput ���� �ʱ�ȭ
        _chatInput.text = "";

        // chatInput ������ ���õ� ���·�
        _chatInput.ActivateInputField();

        if (text.Contains("���"))
        {
            print("����̸� ȣ���߽��ϴ�.");
            StartCoroutine(CoKkamangWatingMent());
        }

        // ---------------------------------------------------------------------------------

        ChatInfo chatInfo = new ChatInfo();

        string island_id = Managers.Info.FamilyCode;
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
        yield return new WaitForSeconds(1.5f);

        photonView.RPC("PunSendKkamangChat", RpcTarget.All, "��ø� ��ٷ������!");

    }

    //Ai
    // ���� ���� �� -> ê�� ������ ����
    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
    public void OnGetPost(string s)
    {
        string url = "http://221.163.19.218:1221/api/chatbot/conversation";

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester(Define.RequestType.POST, Define.DataType.JSON, url, false);

        requester._body = s; // json data
        requester._onComplete = OnGetPostComplete;
        requester._onFailed = OnGetPostFailed;

        Managers.Http.SendRequest(requester);
    }

    public ChatBotResponse chatBotResponse;
    void OnGetPostComplete(DownloadHandler result)
    {
        //HttpAiPhotoData aiPhotoData = new HttpAiPhotoData();
        //aiPhotoData = JsonUtility.FromJson<HttpAiPhotoData>(result.text);


        print(result.text);

        chatBotResponse = new ChatBotResponse();
        chatBotResponse = JsonUtility.FromJson<ChatBotResponse>(result.text);

        if (chatBotResponse.task == "���" || chatBotResponse.answer.Length <= 0 || chatBotResponse.answer == "No Response")
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
        // ä�ÿ� result.text����ϱ�
        int currChannelIdx = 0; // �ӽ�

        // chatItem ������ (scrollView -> content �� �ڽ����� ���)
        GameObject go = Instantiate(_black, _rtContent.transform);
        print("����� ä�� ����");

        // ����̰� ä���� ���´ٴ� UI�� �����ϱ�
        StartCoroutine("CoKkamangMessageDelay");

        AreaScript area = go.GetComponent<AreaScript>();

        // ���δ� �ִ� 600, ���δ� boxRect�� ���� ��������
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = chat;

        area.userNameText.text = "�����";

        area.timeText.text = DateTime.Now.ToString("HH:mm");

        // �ؽ�Ʈ�� ���� ������ �ؽ�Ʈ�� ũ�� �ڽ��� �۰�.. �̷� �� �־
        // ������(?)
        Fit(area.boxRect);


        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭,
        // �� ���� �Ʒ��� �������� ���� �ٷ� �� ũ�⸦ ���ο� ����
        float x = area.textRect.sizeDelta.x + 55;
        float y = area.textRect.sizeDelta.y;

        if (y > 49) // �ؽ�Ʈ�� 3�� �̻�
        {
            for (int i = 0; i < 200; i++)
            {
                area.boxRect.sizeDelta = new Vector2(x - i * 2, area.boxRect.sizeDelta.y);

                Fit(area.boxRect);

                if (area.boxRect.sizeDelta.x <= 130)
                {
                    print(area.boxRect.sizeDelta.x);
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

        // ����� �Ҹ�
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_LodingCat);

        // ��ƼŬ
        createdParticle = Instantiate(particle, TrParticle.position, TrParticle.rotation);

        // �ִϸ��̼�
        chatJump.SetTrigger("ChatJump");

        Invoke("StopAnimation", 3f);


    }

    void StopAnimation()
    {
        chatJump.SetTrigger("StopJump");
        Destroy(createdParticle);
        print("���� ����");

    }

    IEnumerator CoKkamangMessageDelay()
    {
        message.SetActive(true);

        yield return new WaitForSeconds(0.8f);

        message.SetActive(false);
    }

    void OnGetPostFailed(DownloadHandler result)
    {
        print("Chat ����");
    }

    // ��ư�� ������ ä���� ���۵�
    public void OnClickSendBtn()
    {
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONON);

        if (_chatInput.text.Length == 0)
        {
            return;
        }

        string text = _chatInput.text;
        int currChannelIdx = 0; // �ӽ�

        chatClient.PublishMessage(chatChannelNames[0], text);

        //���� �߰� (������ ����)
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BtnAdd);

        // inputChat ���� �ʱ�ȭ
        _chatInput.text = "";

        // inputChat ������ ���õ� ���·�
        _chatInput.ActivateInputField();

        if (text.Contains("���"))
        {
            print("����̸� ȣ���߽��ϴ�.");
            StartCoroutine(CoKkamangWatingMent());
        }

        // ---------------------------------------------------------------------------------

        ChatInfo chatInfo = new ChatInfo();

        string island_id = Managers.Info.FamilyCode;
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
            go = Instantiate(_yellow, _rtContent);
            area = go.GetComponent<AreaScript>();
        }
        else
        {
            print("��밡 ����");
            go = Instantiate(_white, _rtContent);
            area = go.GetComponent<AreaScript>();
            area.userNameText.text = sender;

            // ����� ������ �̹��� ��������
            print(sender);
            area.profileImg.sprite = Resources.Load<Sprite>("member/" + dicAllPlayerProfile[sender]);

        }


        // ���δ� �ִ� 600, ���δ� boxRect�� ���� ��������
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = text;

        // �ؽ�Ʈ�� ���� ������ �ؽ�Ʈ�� ũ�� �ڽ��� �۰�.. �̷� �� �־
        // ������(?)
        Fit(area.boxRect);


        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭,
        // �� ���� �Ʒ��� �������� ���� �ٷ� �� ũ�⸦ ���ο� ����
        float x = area.textRect.sizeDelta.x + 70;
        float y = area.textRect.sizeDelta.y;

        if (y > 49) // �ؽ�Ʈ�� 3�� �̻�
        {
            for (int i = 0; i < 200; i++)
            {
                area.boxRect.sizeDelta = new Vector2(x - i * 2, area.boxRect.sizeDelta.y);

                Fit(area.boxRect);

                if (area.boxRect.sizeDelta.x <= 130)
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

    void ScrollDelay() => _scrollbar.value = 0;


    public void OnclickCloseBtn()
    {
        isChatRoomActive = false;
        _chatBG.SetActive(false);
    }

    public void OnClickChatBtn()
    {
        isChatRoomActive = true;
        _chatBG.SetActive(true);

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
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_BUTTONOFF);
        for (int i = 0; i < senders.Length; i++)
        {
            print(nameof(OnGetMessages));
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
