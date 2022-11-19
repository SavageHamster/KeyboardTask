namespace CustomInputField
{
    /// <summary>
    /// Interface for manipulating input field from commands.
    /// </summary>
    internal interface IMyInputField
    {
        bool HasSelection { get; }
        bool IsInFocus { get; }
        int CaretPosition { get; set; }
        int CaretSelectPosition { get; set; }
        string Text { get; set; }

        void DeleteSelection();
        void UpdateLabel();
    }
}
