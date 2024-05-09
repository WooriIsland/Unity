using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ĳ���͸� �����ϴ� Ŭ����
// ������ ĳ���ʹ� InfoManager�� ����ȴ�.
public class CharacterSelectManagerFinal : MonoBehaviour
{

    public Transform button;

    // �⺻ ���õ� ���ӿ�����Ʈ
    [SerializeField] GameObject _basicCharacter, _basicSelector;

    // ���� ���õ� ���ӿ�����Ʈ
    GameObject _prevCharacter, _preSelector;


    private void Start()
    {
        // �⺻���� ���õ� ĳ���� UI Ȱ��ȭ
        _basicCharacter.SetActive(true);
        _basicSelector.SetActive(true);

        // ���õ� ĳ���� �ִϸ��̼� Ȱ��ȭ
        _basicCharacter.GetComponent<Animator>().SetFloat("Speed", 1);

        // ĳ���� �̸��� InfoManager�� ����
        Managers.Info.Character = _basicCharacter.name;
        Managers.Info.dicMemberCharacter[Managers.Info.NickName] = _basicCharacter.name;

        // ���� ����
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);
    }

    // ĳ���Ͱ� ���õ� �� ȣ��Ǵ� �޼���
    public void SelectCharacter(GameObject character)
    {
        // ���� ����
        SoundManager_LHS.instance.PlaySFX(SoundManager_LHS.ESfx.SFX_Hellow);

        // ���� ĳ���Ͱ� ���õǾ� �ִٸ�
        if (_basicCharacter.activeSelf == true)
        {
            // ���� Ǯ��
            _basicCharacter.SetActive(false);
            _basicSelector.SetActive(false);
        }

        // ���� ������ ĳ���� Ȱ��ȭ
        if (_prevCharacter != null && _prevCharacter.name != character.name)
        {
            _preSelector.SetActive(false); // ��Ȱ��ȭ ��ư ����
            _prevCharacter.SetActive(false);
            _prevCharacter.GetComponent<Animator>().SetFloat("Speed", 0);
        }

        // ���� ������ ĳ���ͷ� ������Ʈ
        _prevCharacter = character;

        button.Find(character.name).gameObject.SetActive(true); // ��Ȱ��ȭ ��ư ����
        _preSelector = button.Find(character.name).gameObject;
        character.SetActive(true); // ĳ���� ������ Ȱ��ȭ
        character.GetComponent<Animator>().SetFloat("Speed", 1); // ĳ���� ������ �ִϸ��̼� ����

        Managers.Info.Character = character.name; // ĳ���� �̸� InfoManager�� ����
        Managers.Info.dicMemberCharacter[Managers.Info.NickName] = character.name; // �г��� : ĳ���� �� ���� �ӽ�
    }

    //public void GoGameScene()
    //{
    //    Photon.Pun.PhotonNetwork.LoadLevel(4);
    //}
}
