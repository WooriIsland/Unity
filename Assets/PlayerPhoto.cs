using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhoto : MonoBehaviour
{
    // �÷��̾� �̹����� ���� ����
    public Image[] imageBox;

    // �÷��̾� �̹�����
    public Sprite[] imageSprite;

    // ���� ���� �̹��� ��ȣ ���� HashSet
    HashSet<int> randomImageNumbers = new HashSet<int>();

    private void Start()
    {
        // �������� ���ڸ� �����ϰ�
        int idx = Random.Range(0, imageSprite.Length);

        // set�� ��´�.
        randomImageNumbers.Add(idx);

        // ���� set�� ũ�Ⱑ 3�� �Ǹ�?
        if(randomImageNumbers.Count == 3)
        {
            // �ݺ��� ���߰� �ش��ϴ� �̹����� box�� ���� �����϶�
            for (int i = 0; i < imageBox.Length; i++)
            {
                
            }
        }
        
    }
}
