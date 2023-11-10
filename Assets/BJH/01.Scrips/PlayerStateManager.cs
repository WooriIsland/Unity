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

    // 게임이 시작될 때
    // 현재 접속자 받아오기
    // UI 업데이트
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

    // 방 구성원 UI를 생성해주는 메서드
    public void Member()
    {
        players = connectedPlayerInfo.joinedPlayers;

        foreach (var player in players)
        {
            // 프리팹 생성
            GameObject go = Instantiate(playerStatePrefab, playerStateBox);

            // 프리팹이 알고있는 image 게임오브젝트의 image 컴포넌트를 가져옴
            Image image = go.GetComponent<PlayerState>().playerImg.GetComponent<Image>();

            // resoures에서 사진을 가져옴
            Texture2D picture = Resources.Load<Texture2D>("/Member" + player);

            // resources에서 가져온 사진을 image에 적용하기
            image.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
        }


    }




}
