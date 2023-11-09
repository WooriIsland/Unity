using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    

    void Start()
    {
        List<string> list = new List<string>();
        list.Add("현숙이 멍청이");
        list.Add("지환이도 마찬가지");

        AudioManager audioManager = AudioManager.Instance;
        Hashtable hash = new Hashtable();
        hash.Add("현숙이바보", list);
        hash.Add(10, "니마넝라ㅣㅓㄴㅇ");

        List<string> list2 = (List<string>)hash["현숙이바보"];
        print(list2[0]);
        print(list2[1]);

        print(hash[10].ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
