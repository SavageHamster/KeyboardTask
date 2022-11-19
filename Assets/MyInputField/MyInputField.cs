using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace CustomInputField
{
    /// <summary>
    /// In accordance with the task InputFieldOriginal was not modified at all. 
    /// But in theory it could be possible to reduce amount of code twice by 
    /// changing few access modifiers in InputFieldOriginal.
    /// </summary>
    public sealed class MyInputField : InputFieldOriginal, IMyInputField
    {
        public int CaretPosition
        {
            get => caretPositionInternal;
            set => caretPositionInternal = value;
        }
        public int CaretSelectPosition
        {
            get => caretSelectPositionInternal;
            set => caretSelectPositionInternal = value;
        }

        public string Text
        {
            get => text;
            set
            {
                m_Text = value;
                UpdateLabel();
            }
        }

        public bool IsInFocus { get; private set; }
        public bool HasSelection => caretPositionInternal != caretSelectPositionInternal;

        // In real task it's better to have a pool of commands of course.
        private readonly List<InputCommand> _scheduledCommands = new();

        private int _caretPositionCached;
        private int _caretSelectPositionCached;
        private bool _isPointerOverVirtualKeyboard;
        private bool _needToRestoreCaretPosition;

        protected override void LateUpdate()
        {
            // Here original input field selects text, if ActivateInputField() was called in the previous frame.
            // There is no way to prevent selection without modifing base class (at least I didn't find it).
            base.LateUpdate();

            // Here I undo text selection and restore caret position to values before focus loss.
            if (_needToRestoreCaretPosition)
            {
                RestoreCaretPosition();
                _needToRestoreCaretPosition = false;
            }

            _scheduledCommands.ForEach(c => c.Execute());
            _scheduledCommands.Clear();
        }

        /// <summary>
        /// This method helps to figure out, was OnDeselect callback received because of VirtualKeyboard,
        /// or because of deselection by clicking on some other not related to input field object.
        /// </summary>
        public void OnPointerOverVirtualKeyboard(bool isOver)
        {
            _isPointerOverVirtualKeyboard = isOver;
        }

        /// <summary>
        /// Unity invokes this method when input field lose selection state 
        /// both because of virtual keyboard and because of any other UI element.
        /// </summary>
        public override void OnDeselect(BaseEventData eventData)
        {
            IsInFocus = _isPointerOverVirtualKeyboard;

            _caretPositionCached = caretPositionInternal;
            _caretSelectPositionCached = caretSelectPositionInternal;

            base.OnDeselect(eventData);
        }

        public void EnterSymbol(string symbol)
        {
            TryActivateInputField();
            _scheduledCommands.Add(new EnterSymbolCommand(this, symbol));
        }

        /// <summary>
        /// It isn't directly mentioned in the task, how Backspace button and arrows should behave itself
        /// on click, if input field in not in focus. Logically thinking, they should do nothing (by analogy with phisical keyboard). 
        /// So they do nothing in my implementation.
        /// </summary>
        public void DeleteText()
        {
            if (!IsInFocus)
            {
                return;
            }

            TryActivateInputField();
            _scheduledCommands.Add(new BackspaceCommand(this));
        }

        public void MoveCaretLeft()
        {
            if (!IsInFocus)
            {
                return;
            }

            TryActivateInputField();
            _scheduledCommands.Add(new ShiftCaretCommand(this, true));
        }

        public void MoveCaretRight()
        {
            if (!IsInFocus)
            {
                return;
            }

            TryActivateInputField();
            _scheduledCommands.Add(new ShiftCaretCommand(this, false));
        }

        void IMyInputField.DeleteSelection()
        {
            if (!HasSelection)
            {
                return;
            }

            if (caretPositionInternal < caretSelectPositionInternal)
            {
                Text = text.Substring(0, caretPositionInternal) + 
                    text.Substring(caretSelectPositionInternal, text.Length - caretSelectPositionInternal);
                caretSelectPositionInternal = caretPositionInternal;
            }
            else
            {
                Text = text.Substring(0, caretSelectPositionInternal) + 
                    text.Substring(caretPositionInternal, text.Length - caretPositionInternal);
                caretPositionInternal = caretSelectPositionInternal;
            }
        }

        void IMyInputField.UpdateLabel()
        {
            UpdateLabel();
        }

        private void TryActivateInputField()
        {
            if (isFocused)
            {
                return;
            }

            _needToRestoreCaretPosition = true;
            ActivateInputField();
        }

        private void RestoreCaretPosition()
        {
            caretPositionInternal = _caretPositionCached;
            caretSelectPositionInternal = _caretSelectPositionCached;
        }
    }
}