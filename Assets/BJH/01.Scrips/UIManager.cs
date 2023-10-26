using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Text nickName;

    public GameObject createFamilyCodeImg;

    public InputField familyCodeInputField;

    // Start is called before the first frame update
    void Start()
    {
        createFamilyCodeImg.SetActive(false);

        chatCanvas.SetActive(false);
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

    // ä�� Canvas
    public GameObject chatCanvas;
    private bool isChatCanvasActive = false;

    // Canvas_Chat�� Ȱ��/��Ȱ��ȭ�ϴ� �޼���
    public void OnOffChatCanvas()
    {
        isChatCanvasActive = !isChatCanvasActive;

        chatCanvas.SetActive(isChatCanvasActive);
    }
}
