using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보내기 가능


// 플레이어를 이동시킨다.
// 임시 : awds
// 확정 : 화면 왼쪽 하단 조이스틱
public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // lever
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    // stici range
    [SerializeField, Range(10, 150)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    [SerializeField]
    private MovePlayer movePlayer;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }

    // 오브젝트를 클릭하여 드래그하는 중에 호출
    // 문제점 : 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음
    // 해결 : 인풋이 활성화 된 상태에서는 업데이트 함수에서 지속적으로 호출되도록 만듦
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        movePlayer.Move(Vector2.zero);
    }

    private void ControlJoystickLever(PointerEventData eventData)
    {
        // 클릭되는 순간 위치 - 앵커 포지션
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
    }

    private void InputControlVector()
    {
        // 캐릭터에게 입력벡터를 전달
        movePlayer.Move(inputDirection);

    }

    // Update is called once per frame
    void Update()
    {
        if(isInput)
        {
            InputControlVector();
        }
    }
}
