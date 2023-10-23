using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class Monster
{
    public string Name;
    public string AttackType;
    public float Power;
    public int Age;
}

[Serializable]
public class MonsterList
{
    public List<Monster> monsters;
}


public class PracticeSaveChatting : MonoBehaviour
{
    private void Start()
    {
        List<Monster> monsterList = new List<Monster>();

        Monster zombie = new Monster();
        zombie.Name = "Zombie";
        zombie.AttackType = "Bite";
        zombie.Power = 10;
        zombie.Age = 100;

        Monster wizard = new Monster();
        wizard.Name = "Wizard";
        wizard.AttackType = "Magic";
        wizard.Power = 30;
        wizard.Age = 30;

        Monster dracula = new Monster();
        dracula.Name = "Dracula";
        dracula.AttackType = "Bite";
        dracula.Power = 20.5f;
        dracula.Age = 10000;

        monsterList.Add(wizard);
        monsterList.Add(dracula);
        monsterList.Add(zombie);

        MonsterList Monster = new MonsterList();
        Monster.monsters = monsterList;

        //ToJson 부분
        string jsonData = JsonUtility.ToJson(Monster, true);

        string path = Application.dataPath + "/Data";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "/MonsterData.txt", jsonData);

        //FromJson 부분
        string fromJsonData = File.ReadAllText(path + "/MonsterData.txt");

        MonsterList MonsterFromJson = new MonsterList();
        MonsterFromJson = JsonUtility.FromJson<MonsterList>(fromJsonData);
    }

}
