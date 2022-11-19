namespace CustomInputField
{
    internal sealed class BackspaceCommand : InputCommand
    {
        public BackspaceCommand(IMyInputField inputField) : base(inputField) { }

        public override void Execute()
        {
            if (HasSelection)
            {
                DeleteSelection();
            }
            else if (CaretPosition > 0)
            {
                Text = Text.Remove(CaretPosition - 1, 1);
                CaretSelectPosition = CaretPosition -= 1;
            }
        }
    }
}
