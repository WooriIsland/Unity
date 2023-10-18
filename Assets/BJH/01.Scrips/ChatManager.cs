using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class ChatManager : MonoBehaviour, IPointerDownHandler
{
    public Button chatBtn;
    public GameObject chatRoom;
    public GameObject chatExcept;
    bool isChatRoomActive = false;
    bool isChatExcept = false;

    void Start()
    {
        chatRoom.SetActive(false);
        chatExcept.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

#if PC
    public void OnTouchChatBtn()
    {
        if(!isChatRoomActive)
        {
            isChatRoomActive = true;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = true;
            chatExcept.SetActive(isChatExcept);
        }
        else if(isChatRoomActive)
        {
            isChatRoomActive = false;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = false;
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
}
