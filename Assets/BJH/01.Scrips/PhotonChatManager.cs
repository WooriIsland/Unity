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

    // chat 총괄하는 객체
    ChatClient chatClient;

    // 기본 채팅 채널 목록
    public List<string> channelNames = new List<string>();

    // 현재 선택된 채널
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

        // 프리팹을 생성
        AreaScript area = Instantiate(isSend ? yellow : white).GetComponent<AreaScript>();

        // 프리팹이 하이어라키 제일 아래에 있을거니까
        // content의 자식으로 들어가게끔
        area.transform.SetParent(contentRect.transform, false);

        // 가로는 최대 600, 세로는 boxRect의 기존 사이즈대로
        area.boxRect.sizeDelta = new Vector2(600, area.boxRect.sizeDelta.y);

        area.textRect.GetComponent<TMP_Text>().text = text;

        // 텍스트의 엔터 때문에 텍스트는 크고 박스는 작고.. 이럴 수 있어서
        // 리빌딩(?)
        Fit(area.boxRect);

        // 두 줄 이상이면 크기를 줄여가면서,
        // 한 줄이 아래로 내려가는 시점 바로 전 크기를 가로에 대입
        float x = area.textRect.sizeDelta.x + 42; // 왜 42?
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

        // 시간
        DateTime t = DateTime.Now;
        area.time = t.ToString("yyyy-MM-dd-HH-dd");
        area.user = user;

        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if(t.Hour == 0)
        {
            hour = 12;
        }
        else if(t.Hour > 12)
        {
            hour -= 12;
        }
        area.timeText.text = (t.Hour > 12 ? "오후" : "오전") + hour + " : " + t.Minute.ToString("D2");


        // 이전 것과 날짜가 다르면 날짜영역 보이기
        if (lastArea != null && lastArea.time.Substring(0, 10) != area.time.Substring(0, 10))
        {
            Transform curDataArea = Instantiate(date).transform;
            curDataArea.SetParent(contentRect.transform, false);
            curDataArea.SetSiblingIndex(curDataArea.GetSiblingIndex() - 1);

            string week = "";
            switch(t.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    week = "일";
                    break;
                case DayOfWeek.Monday:
                    week = "월";
                    break;
                case DayOfWeek.Tuesday:
                    week = "화";
                    break;
                case DayOfWeek.Wednesday:
                    week = "수";
                    break;
                case DayOfWeek.Thursday:
                    week = "목";
                    break;
                case DayOfWeek.Friday:
                    week = "금";
                    break;
                case DayOfWeek.Saturday:
                    week = "토";
                    break;
            }
            curDataArea.GetComponent<AreaScript>().dataText.text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";

        }

        // 스크롤바가 위로 올라간 상태에서 새 메시지를 받으면 맨 아래로 내리지 않음
        if(!isSend && !isBottom)
        {
            return;
        }
        Invoke("ScrollDelay", 0.03f);

        // 
    }

    // 강제로 채팅박스 조정
    void Fit(RectTransform rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

    void ScrollDelay() => scrollbar.value = 0;

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
