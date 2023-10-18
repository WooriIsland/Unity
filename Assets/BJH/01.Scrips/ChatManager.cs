using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Net.NetworkInformation;

public class ChatManager : MonoBehaviour, IPointerDownHandler
{
    public Button chatBtn;
    public GameObject chatRoom;
    public GameObject chatExcept;
    bool isChatRoomActive = true;
    bool isChatExcept = true;

    public GameObject myPlayer;
    ClickMove clickMove;

    void Start()
    {
        isChatRoomActive = false;
        isChatExcept = false;
        chatRoom.SetActive(isChatRoomActive);
        chatExcept.SetActive(isChatExcept);

        clickMove = myPlayer.GetComponentInChildren<ClickMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

#if PC
    public void OnClickChatBtn()
    {
        if(isChatRoomActive) // true�� �� ������? ��, ä�÷��� ������
        {
            clickMove.canMove = true;

            isChatRoomActive = false;
            chatRoom.SetActive(isChatRoomActive);

            isChatExcept = false;
            chatExcept.SetActive(isChatExcept);
        }
        else if(!isChatRoomActive) // false�� �� ������? ��, ä�÷��� ������
        {
            clickMove.canMove = false;

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
}
