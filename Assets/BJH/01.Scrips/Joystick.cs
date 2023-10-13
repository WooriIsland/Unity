using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Ű����, ���콺, ��ġ�� �̺�Ʈ�� ������Ʈ�� ������ ����


// �÷��̾ �̵���Ų��.
// �ӽ� : awds
// Ȯ�� : ȭ�� ���� �ϴ� ���̽�ƽ
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

    // ������Ʈ�� Ŭ���Ͽ� �巡���ϴ� �߿� ȣ��
    // ������ : Ŭ���� ������ ���·� ���콺�� ���߸� �̺�Ʈ�� ������ ����
    // �ذ� : ��ǲ�� Ȱ��ȭ �� ���¿����� ������Ʈ �Լ����� ���������� ȣ��ǵ��� ����
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
        // Ŭ���Ǵ� ���� ��ġ - ��Ŀ ������
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
    }

    private void InputControlVector()
    {
        // ĳ���Ϳ��� �Էº��͸� ����
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
