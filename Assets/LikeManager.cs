using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LikeManager : MonoBehaviour
{
    LikeBtnInfo like;

    private void Start()
    {
        like = GetComponent<LikeBtnInfo>();
    }

    public void OnClick_Like()
    {
        if(like.like.activeSelf == false)
        {
            like.like.SetActive(true);
            like.likeCnt.text = "13";
        }
        else
        {
            like.like.SetActive(false);
            like.likeCnt.text = "12";
        }


        //if (like.like.activeSelf == false)
        //{
        //    like.like.SetActive(true);

        //    int temp = int.Parse(like.likeCnt.text);
        //    temp++;
        //    like.likeCnt.text = temp.ToString();
        //    like.like.SetActive(!like.like.activeSelf);
        //}

        //if (like.like.activeSelf == true)
        //{
        //    like.like.SetActive(false);

        //    int temp = int.Parse(like.likeCnt.text);
        //    temp--;
        //    like.likeCnt.text = temp.ToString();
        //    like.like.SetActive(!like.like.activeSelf);
        //}




    }
}
