namespace CustomInputField
{
    /// <summary>
    /// Base class for input field commads.
    /// </summary>
    internal abstract class InputCommand
    {
        private readonly IMyInputField _inputField;

        public InputCommand(IMyInputField inputField)
        {
            _inputField = inputField;
        }

        public abstract void Execute();

#region Shorcuts for inheritors

        public bool HasSelection => _inputField.HasSelection;
        public bool IsInFocus => _inputField.IsInFocus;

        public int CaretPosition 
        { 
            get => _inputField.CaretPosition; 
            set => _inputField.CaretPosition = value; 
        }

        public int CaretSelectPosition 
        { 
            get => _inputField.CaretSelectPosition; 
            set => _inputField.CaretSelectPosition = value; 
        }

        public string Text 
        { 
            get => _inputField.Text; 
            set => _inputField.Text = value; 
        }

        public void DeleteSelection()
        {
            _inputField.DeleteSelection();
        }

        public void UpdateLabel()
        {
            _inputField.UpdateLabel();
        }

#endregion
    }
}
