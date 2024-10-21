using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCustom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
    public enum eButtonStyle
    {
        BUTTON,
        SELECT,
    }
    public enum eButtonState
    {
        NORMAL,
        PRESSED,
        SELECTED,
        DISABLED,
        NONE,
    }

    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] public GameObject normalButton;
    [SerializeField] public GameObject pressedButton;
    [SerializeField] public GameObject selectedButton;
    [SerializeField] public GameObject disabledButton;

    public UnityEvent onPointerDown = new UnityEvent();
    public UnityEvent onPointerUp = new UnityEvent();
    public UnityEvent onClickBtn = new UnityEvent();
    public UnityEvent onLongPress = new UnityEvent();

    private eButtonStyle buttonType = eButtonStyle.BUTTON;
    private eButtonState buttonState = eButtonState.NORMAL;

    bool isPressedBtn = false;

    void Awake()
    {
        if (pressedButton && pressedButton.transform.localScale.x == 1f)
            pressedButton.transform.localScale = Vector3.one * 0.95f;
    }
    public void SetButtonMode(eButtonState eState)
    {
        if (normalButton != null) normalButton.SetActive(false);
        if (pressedButton != null) pressedButton.SetActive(false);
        if (selectedButton != null) selectedButton.SetActive(false);
        if (disabledButton != null) disabledButton.SetActive(false);
        buttonState = eState;
        switch (eState)
        {
            case eButtonState.NORMAL:
                if (normalButton != null) normalButton.SetActive(true);
                break;
            case eButtonState.PRESSED:
                if (pressedButton != null) pressedButton.SetActive(true);
                break;
            case eButtonState.SELECTED:
                if (selectedButton != null) selectedButton.SetActive(true);
                break;
            case eButtonState.DISABLED:
                if (disabledButton != null) disabledButton.SetActive(true);
                break;
        }
    }

    public void SetButtonType(eButtonStyle type)
    {
        buttonType = type;
    }

    public void OnClickAddListener(UnityAction _action)
    {
        onClickBtn.RemoveListener(_action);
        onClickBtn.AddListener(_action);
    }

    public void OnClickRemoveListener(UnityAction _action)
    {
        onClickBtn.RemoveListener(_action);
    }

    public void OnClickRemoveAllListeners()
    {
        onClickBtn.RemoveAllListeners();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPressedBtn)
            return;

        isPressedBtn = true;
        SetButtonMode(eButtonState.PRESSED);
        onPointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case eButtonStyle.BUTTON:
                SetButtonMode(eButtonState.NORMAL);
                break;
            case eButtonStyle.SELECT:
                SetButtonMode(buttonState == eButtonState.SELECTED ? eButtonState.NORMAL : eButtonState.SELECTED);
                break;
        }

        isPressedBtn = false;
        onPointerUp?.Invoke();
        onClickBtn?.Invoke();
    }

    public void OnLongPressed(PointerEventData eventData)
    {

    }


}
