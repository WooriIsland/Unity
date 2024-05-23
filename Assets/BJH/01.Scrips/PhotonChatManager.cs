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
using static System.Net.Mime.MediaTypeNames;


public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    // 채팅창
    public TMP_InputField inputChat;

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

    // Chat UI
    public GameObject yellow, white, date;
    public RectTransform contentRect;
    public Scrollbar scrollbar;
    AreaScript lastArea;

    void Start()
    {
        inputChat.onSubmit.AddListener((string s) =>
        {
            // 채팅 보내기
            chatClient.PublishMessage(channelNames[currChannelIdx], s);
            // 채팅을 보내고 나서 inputChat 초기화
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
        chatClient.AuthValues = new Photon.Chat.AuthenticationValues(InfoManager.Instance.NickName);

        // 초기설정을 이용해서 채팅서버에 연결 시도
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    public void CreateChat(bool isSend, string text, string user)
    {
        // 만약 텍스트에 내용이 없다면?
        // 아무것도 실행되지 않는다.
        if (text.Trim() == "")
        {
            return;
        }

        // 바닥임을 판별
        bool isBottom = scrollbar.value <= 0.00001f;
        
        // 프리팹을 생성
        GameObject go = Instantiate(isSend ? yellow : white);
        AreaScript area = go.GetComponent<AreaScript>();

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

        // 시간
        DateTime t = DateTime.Now;
        area.time = t.ToString("yyyy-MM-dd-HH-dd");
        area.user = user;

        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0)
        {
            hour = 12;
        }
        else if (t.Hour > 12)
        {
            hour -= 12;
        }
        area.timeText.text = (t.Hour > 12 ? "오후" : "오전") + hour + " : " + t.Minute.ToString("D2");

        #region 날짜(기획 추가되면 주석 풀기)
        // 이전 것과 날짜가 다르면 날짜영역 보이기
        //if (lastArea != null && lastArea.time.Substring(0, 10) != area.time.Substring(0, 10))
        //{
        //    Transform curDataArea = Instantiate(date).transform;
        //    curDataArea.SetParent(contentRect.transform, false);
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
        #endregion

        // 스크롤바가 위로 올라간 상태에서 새 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom)
        {
            return;
        }
        Invoke("ScrollDelay", 0.03f);
    }

    // 강제로 채팅박스 조정
    void Fit(RectTransform rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

    // 스크롤 업데이트
    void ScrollDelay() => scrollbar.value = 0;

    // photon chat 메시지 보내기
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {

        for (int i = 0; i < senders.Length; i++)
        {
            CreateChat(false, messages[i].ToString(), PhotonNetwork.NickName);
        }
    }

    #region Photonchat 관련 인터페이스 구현부(필요한 거 있으면 가져다 쓰기)
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
    #endregion
}
