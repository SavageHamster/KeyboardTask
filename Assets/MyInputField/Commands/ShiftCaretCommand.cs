using UnityEngine;

namespace CustomInputField
{
    internal sealed class ShiftCaretCommand : InputCommand
    {
        private readonly bool _left;

        public ShiftCaretCommand(IMyInputField inputField, bool left) : base(inputField) 
        {
            _left = left;
        }

        public override void Execute()
        {
            if (HasSelection)
            {
                CaretPosition = CaretSelectPosition = GetCaretPosition();
                return;
            }
            
            CaretSelectPosition = CaretPosition = _left ? 
                CaretSelectPosition - 1 : 
                CaretSelectPosition + 1;

            UpdateLabel();
        }

        private int GetCaretPosition()
        {
            return _left
                ? Mathf.Min(CaretPosition, CaretSelectPosition)
                : Mathf.Max(CaretPosition, CaretSelectPosition);
        }
    }
}
