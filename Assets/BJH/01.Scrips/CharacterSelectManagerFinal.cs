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
        basicCharacter.GetComponent<Animator>().SetFloat("Speed", 1); // 캐릭터 프리팹 애니메이션 적용
        InfoManager.Instance.Character = basicCharacter.name; // 캐릭터 이름 InfoManager에 저장
    }

    public void SelectCharacter(GameObject character)
    {
        //현숙추가 클릭시 사운드
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);

        print($"선택된 플레이어 프리팹 이름 : {character}");

        if(basicCharacter.activeSelf == true)
        {
            basicCharacter.SetActive(false);
            basicSelector.SetActive(false);

        }

        if (prevCharacter != null && prevCharacter.name != character.name)
        {
            preSelector.SetActive(false); // 비활성화 버튼 끄기
            prevCharacter.SetActive(false);
            prevCharacter.GetComponent<Animator>().SetFloat("Speed", 0);
        }

        prevCharacter = character;

        button.Find(character.name).gameObject.SetActive(true); // 비활성화 버튼 끄기
        preSelector = button.Find(character.name).gameObject;
        character.SetActive(true); // 캐릭터 프리팹 활성화
        character.GetComponent<Animator>().SetFloat("Speed", 1); // 캐릭터 프리팹 애니메이션 적용
        
        InfoManager.Instance.Character = character.name; // 캐릭터 이름 InfoManager에 저장
    }

    public void GoGameScene()
    {
        Photon.Pun.PhotonNetwork.LoadLevel(4);
    }
}
