using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManagerFinal : MonoBehaviour
{
    public Transform button;
    
    public GameObject basicCharacter, basicSelector;

    GameObject prevCharacter, preSelector;
    

    private void Start()
    {
        basicCharacter.SetActive(true);
        basicSelector.SetActive(true);
        basicCharacter.GetComponent<Animator>().SetFloat("Speed", 1); // ĳ���� ������ �ִϸ��̼� ����
        InfoManager.Instance.Character = basicCharacter.name; // ĳ���� �̸� InfoManager�� ����
    }

    public void SelectCharacter(GameObject character)
    {
        //�����߰� Ŭ���� ����
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);

        print($"���õ� �÷��̾� ������ �̸� : {character}");

        if(basicCharacter.activeSelf == true)
        {
            basicCharacter.SetActive(false);
            basicSelector.SetActive(false);

        }

        if (prevCharacter != null && prevCharacter.name != character.name)
        {
            preSelector.SetActive(false); // ��Ȱ��ȭ ��ư ����
            prevCharacter.SetActive(false);
            prevCharacter.GetComponent<Animator>().SetFloat("Speed", 0);
        }

        prevCharacter = character;

        button.Find(character.name).gameObject.SetActive(true); // ��Ȱ��ȭ ��ư ����
        preSelector = button.Find(character.name).gameObject;
        character.SetActive(true); // ĳ���� ������ Ȱ��ȭ
        character.GetComponent<Animator>().SetFloat("Speed", 1); // ĳ���� ������ �ִϸ��̼� ����
        
        InfoManager.Instance.Character = character.name; // ĳ���� �̸� InfoManager�� ����
    }

    public void GoGameScene()
    {
        Photon.Pun.PhotonNetwork.LoadLevel(4);
    }
}
