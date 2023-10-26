using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 0 : �������
    // 1 : ������ ����
    public AudioClip[] audioClips;

    public AudioSource audioSource;

    private static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        audioSource.clip = audioClips[0];
    }

    // ������� �÷���
    private void PlayBGM()
    {
        audioSource.clip = audioClips[0];
    }

    // ���ٹ̱� ���� �÷���
    private void PlayEditBGM()
    {
        audioSource.clip = audioClips[1];
    }

}
