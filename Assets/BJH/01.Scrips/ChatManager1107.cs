using ExitGames.Client.Photon;
using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager1107 : MonoBehaviour, IChatClientListener
{
    // Chat
    public GameObject chatBG, yellow, white, date;
    public Transform trContent;
    public Scrollbar scrollbar;
    public TMP_InputField chatInput;
    AreaScript lastArea;

    // bool
    bool isActiveChatBG;

    // photon chat
    ChatAppSettings chatAppSettings;
    ChatClient chatClient;
    public List<string> chatChannelName = new List<string>();


    void Start()
    {
        chatBG.SetActive(false);
        isActiveChatBG = false;

        chatInput.onSubmit.AddListener(OnSubmit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Ã¤ÆÃ UI¸¦ Ä×´Ù ²ô´Â ¸Þ¼­µå
    public void OnClickChatBtn()
    {
        isActiveChatBG = !isActiveChatBG;
        chatBG.SetActive(isActiveChatBG);
    }

    public void OnSubmit(string text)
    {
        chatClient.PublishMessage(chatChannelName[0], text);

        chatInput.text = "";

        chatInput.ActivateInputField();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
}
