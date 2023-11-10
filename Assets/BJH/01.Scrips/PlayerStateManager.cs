using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    ConnectedPlayerInfo connectedPlayerInfo;

    public Transform playerStateBox;

    string[] players;
    public GameObject playerStatePrefab;

    // ������ ���۵� ��
    // ���� ������ �޾ƿ���
    // UI ������Ʈ
    GameObject go;
    private void Start()
    {
        connectedPlayerInfo = ConnectedPlayerInfo.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Member();
        }
    }

    // �� ������ UI�� �������ִ� �޼���
    public void Member()
    {
        players = connectedPlayerInfo.joinedPlayers;

        foreach (var player in players)
        {
            // ������ ����
            GameObject go = Instantiate(playerStatePrefab, playerStateBox);

            // �������� �˰��ִ� image ���ӿ�����Ʈ�� image ������Ʈ�� ������
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures���� ������ ������
            Texture2D picture = Resources.Load<Texture2D>("/Member" + player);

            // resources���� ������ ������ image�� �����ϱ�
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
        }


    }




}
