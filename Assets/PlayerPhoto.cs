using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhoto : MonoBehaviour
{
    // 플레이어 이미지를 담을 공간
    public Image[] imageBox;

    // 플레이어 이미지들
    public Sprite[] imageSprite;

    // 집어 넣을 이미지 번호 담을 HashSet
    HashSet<int> randomImageNumbers = new HashSet<int>();

    private void Start()
    {
        // 랜덤으로 숫자를 생성하고
        int idx = Random.Range(0, imageSprite.Length);

        // set에 담는다.
        randomImageNumbers.Add(idx);

        // 만약 set의 크기가 3이 되면?
        if(randomImageNumbers.Count == 3)
        {
            // 반복을 멈추고 해당하는 이미지를 box에 각각 생성하라
            for (int i = 0; i < imageBox.Length; i++)
            {
                
            }
        }
        
    }
}
