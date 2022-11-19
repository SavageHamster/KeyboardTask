using CustomInputField;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI layout was reorganized a little bit. 
/// Hierarchy in Unity matters for correct work of MyInputField and VirtualKeyboard.
/// </summary>
public class VirtualKeyboard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private MyInputField _inputField;
    [SerializeField]
    private float _maxInputCooldown = 0.1f;
    [SerializeField]
    private float _minInputCooldown = 0.05f;
    [SerializeField]
    private AnimationCurve _inputCooldownReductionCurve;
    [SerializeField]
    private List<CustomButton> _buttons;

    private float _nextInputTime;
    private float _maxCooldownReduction;

    private void Awake()
    {
        foreach(var button in _buttons)
        {
            button.OnHold += OnButtonHold;
            button.OnClick += OnButtonClick;
        }

        _maxCooldownReduction = Mathf.Max(_maxInputCooldown - _minInputCooldown, 0f);
    }

    private void OnDestroy()
    {
        foreach (var button in _buttons)
        {
            button.OnHold -= OnButtonHold;
            button.OnClick -= OnButtonClick;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inputField.OnPointerOverVirtualKeyboard(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inputField.OnPointerOverVirtualKeyboard(false);
    }

    private void OnButtonClick(KeyCode keyCode)
    {
        ProcessInput(keyCode);
    }

    private void OnButtonHold(KeyCode keyCode, float pressTime)
    {
        if (Time.unscaledTime < _nextInputTime)
        {
            return;
        }

        ProcessInput(keyCode);

        var inputCooldownReduction = _maxCooldownReduction * _inputCooldownReductionCurve.Evaluate(pressTime);
        _nextInputTime = Time.unscaledTime + (_maxInputCooldown - inputCooldownReduction);
    }

    private void ProcessInput(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.A:
            case KeyCode.B:
            case KeyCode.C:
                _inputField.EnterSymbol(keyCode.ToString());
                break;
            case KeyCode.LeftArrow:
                _inputField.MoveCaretLeft();
                break;
            case KeyCode.RightArrow:
                _inputField.MoveCaretRight();
                break;
            case KeyCode.Backspace:
                _inputField.DeleteText();
                break;
            default:
                throw new NotImplementedException($"Handler for KeyCode {keyCode} is not implemented");
        }
    }
}