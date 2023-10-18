using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameObject createFamilyCodeImg;

    public InputField familyCodeInputField;

    // Start is called before the first frame update
    void Start()
    {
        createFamilyCodeImg.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickCreateFamilyCod()
    {
        createFamilyCodeImg.SetActive(true);
    }

    // �����ڵ尡 �����ƴٴ� UI
    // Ȯ�� ��ư�� ������
    // �����ڵ� Input field�� ������ �ڵ尡 ����.
    // �ӽ÷� ����Ƽ���� ����
    // ���� ������ ����Ͽ� �����͸� �޾� �� ����
    public void OnClickCheckBtn()
    {
        createFamilyCodeImg.SetActive(false);
        familyCodeInputField.text = "a12mm4dfg4d123"; // �ӽ�
    }
}
