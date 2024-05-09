using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 캐릭터를 선택하는 클래스
// 선택한 캐릭터는 InfoManager에 저장된다.
public class CharacterSelectManagerFinal : MonoBehaviour
{

    public Transform button;

    // 기본 선택된 게임오브젝트
    [SerializeField] GameObject _basicCharacter, _basicSelector;

    // 이전 선택된 게임오브젝트
    GameObject _prevCharacter, _preSelector;


    private void Start()
    {
        // 기본으로 선택된 캐릭터 UI 활성화
        _basicCharacter.SetActive(true);
        _basicSelector.SetActive(true);

        // 선택된 캐릭터 애니메이션 활성화
        _basicCharacter.GetComponent<Animator>().SetFloat("Speed", 1);

        // 캐릭터 이름을 InfoManager에 저장
        Managers.Info.Character = _basicCharacter.name;
        Managers.Info.dicMemberCharacter[Managers.Info.NickName] = _basicCharacter.name;

        // 사운드 실행
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);
    }

    // 캐릭터가 선택될 때 호출되는 메서드
    public void SelectCharacter(GameObject character)
    {
        // 사운드 실행
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);

        // 현재 캐릭터가 선택되어 있다면
        if (_basicCharacter.activeSelf == true)
        {
            // 선택 풀기
            _basicCharacter.SetActive(false);
            _basicSelector.SetActive(false);
        }

        // 새로 선택한 캐릭터 활성화
        if (_prevCharacter != null && _prevCharacter.name != character.name)
        {
            _preSelector.SetActive(false); // 비활성화 버튼 끄기
            _prevCharacter.SetActive(false);
            _prevCharacter.GetComponent<Animator>().SetFloat("Speed", 0);
        }

        // 새로 선택한 캐릭터로 업데이트
        _prevCharacter = character;

        button.Find(character.name).gameObject.SetActive(true); // 비활성화 버튼 끄기
        _preSelector = button.Find(character.name).gameObject;
        character.SetActive(true); // 캐릭터 프리팹 활성화
        character.GetComponent<Animator>().SetFloat("Speed", 1); // 캐릭터 프리팹 애니메이션 적용

        Managers.Info.Character = character.name; // 캐릭터 이름 InfoManager에 저장
        Managers.Info.dicMemberCharacter[Managers.Info.NickName] = character.name; // 닉네임 : 캐릭터 로 저장 임시
    }

    //public void GoGameScene()
    //{
    //    Photon.Pun.PhotonNetwork.LoadLevel(4);
    //}
}
