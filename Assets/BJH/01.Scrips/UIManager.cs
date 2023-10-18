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

    // 가족코드가 생성됐다는 UI
    // 확인 버튼을 누르면
    // 가족코드 Input field에 생성된 코드가 들어간다.
    // 임시로 유니티에서 구현
    // 추후 서버와 통신하여 데이터를 받아 올 예정
    public void OnClickCheckBtn()
    {
        createFamilyCodeImg.SetActive(false);
        familyCodeInputField.text = "a12mm4dfg4d123"; // 임시
    }
}
