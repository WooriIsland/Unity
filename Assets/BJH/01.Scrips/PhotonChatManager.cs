using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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


    // Chat UI
    public GameObject yellow, white, date;
    public RectTransform contentRect;
    public Scrollbar scrollbar;
    AreaScript lastArea;

    public void Chat(bool isSend, string text, string user, Texture picture)
    {
        if(text.Trim() == "")
        {
            return;
        }

        bool isBottom = scrollbar.value <= 0.0001f;

        print(text);

        // �������� ����
        AreaScript area = Instantiate(isSend ? yellow : white).GetComponent<AreaScript>();

        // �������� ���̾��Ű ���� �Ʒ��� �����Ŵϱ�
        // content�� �ڽ����� ���Բ�
        area.transform.SetParent(contentRect.transform, false);

        // ���δ� �ִ� 600, ���δ� boxRect�� ���� ��������
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = text;

        // �ؽ�Ʈ�� ���� ������ �ؽ�Ʈ�� ũ�� �ڽ��� �۰�.. �̷� �� �־
        // ������(?)
        Fit(area.boxRect);

        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭,
        // �� ���� �Ʒ��� �������� ���� �ٷ� �� ũ�⸦ ���ο� ����
        float x = area.textRect.sizeDelta.x + 42; // �� 42?
        float y = area.textRect.sizeDelta.y;

        if (y > 49) 
        {
            for (int i = 0; i < 200; i++)
            {
                area.boxRect.sizeDelta = new Vector2(x - i * 2, area.boxRect.sizeDelta.y);
                Fit(area.boxRect);

                if(y != area.textRect.sizeDelta.y)
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

        // �ð�
        DateTime t = DateTime.Now;
        area.time = t.ToString("yyyy-MM-dd-HH-dd");
        area.user = user;

        // ���� ���� �׻� ���ο� �ð� ����
        int hour = t.Hour;
        if(t.Hour == 0)
        {
            hour = 12;
        }
        else if(t.Hour > 12)
        {
            hour -= 12;
        }
        area.timeText.text = (t.Hour > 12 ? "����" : "����") + hour + " : " + t.Minute.ToString("D2");


        // ���� �Ͱ� ��¥�� �ٸ��� ��¥���� ���̱�
        if (lastArea != null && lastArea.time.Substring(0, 10) != area.time.Substring(0, 10))
        {
            Transform curDataArea = Instantiate(date).transform;
            curDataArea.SetParent(contentRect.transform, false);
            curDataArea.SetSiblingIndex(curDataArea.GetSiblingIndex() - 1);

            string week = "";
            switch(t.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    week = "��";
                    break;
                case DayOfWeek.Monday:
                    week = "��";
                    break;
                case DayOfWeek.Tuesday:
                    week = "ȭ";
                    break;
                case DayOfWeek.Wednesday:
                    week = "��";
                    break;
                case DayOfWeek.Thursday:
                    week = "��";
                    break;
                case DayOfWeek.Friday:
                    week = "��";
                    break;
                case DayOfWeek.Saturday:
                    week = "��";
                    break;
            }
            curDataArea.GetComponent<AreaScript>().dataText.text = t.Year + "�� " + t.Month + "�� " + t.Day + "�� " + week + "����";

        }

        // ��ũ�ѹٰ� ���� �ö� ���¿��� �� �޽����� ������ �� �Ʒ��� ������ ����
        if(!isSend && !isBottom)
        {
            return;
        }
        Invoke("ScrollDelay", 0.03f);

        // 
    }

    // ������ ä�ùڽ� ����
    void Fit(RectTransform rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

    void ScrollDelay() => scrollbar.value = 0;

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
