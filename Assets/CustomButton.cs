using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Button which provides OnClick and OnHold events.
/// </summary>
public sealed class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const float DetectHoldDelay = 0.7f;

    public event Action<KeyCode, float> OnHold;
    public event Action<KeyCode> OnClick;

    [SerializeField]
    private KeyCode _keyCode;

    private bool _isPressed;
    private bool _isHolded;
    private float _holdingStartTime;
    private float _detectHoldTime;

    private void Update()
    {
        if (_isPressed && Time.unscaledTime >= _detectHoldTime)
        {
            if (!_isHolded)
            {
                _holdingStartTime = Time.unscaledTime;
                _isHolded = true;
            }

            OnHold?.Invoke(_keyCode, Time.unscaledTime - _holdingStartTime);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        _detectHoldTime = Time.unscaledTime + DetectHoldDelay;

        OnClick?.Invoke(_keyCode);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        _isHolded = false;
    }
}
