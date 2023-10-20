using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SerializeField]
public class Chatting
{
    public string name;
    public string msg;
    public string familyCode;
}

[SerializeField]
public class Family
{
    public List<Chatting> family;
}

public class SaveChatting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Chatting> familyList = new List<Chatting>();

        Chatting chatting01 = new Chatting();
        chatting01.name = "jihwan";
        chatting01.msg = "현숙아,, 언제,, 와?";
        chatting01.familyCode = "98bjh98phs";

        familyList.Add(chatting01);


        Family family = new Family();
        family.family = familyList;

        //print(family.family[0].msg);

        // ToJson
        string jsonData = JsonUtility.ToJson(family.family[0], true);

        string path = Application.dataPath + "/Data";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "/Family.txt", jsonData);

        //// FromJson
        //string fromJsonData = File.ReadAllText(path + "/FamilyData.txt");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
