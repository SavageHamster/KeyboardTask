namespace CustomInputField
{
    internal sealed class EnterSymbolCommand : InputCommand
    {
        private readonly string _symbol;

        public EnterSymbolCommand(IMyInputField inputField, string symbol) : base(inputField)
        {
            _symbol = symbol;
        }

        public override void Execute()
        {
            if (!IsInFocus)
            {
                CaretPosition = CaretSelectPosition = Text.Length;
            }

            if (HasSelection)
            {
                DeleteSelection();
            }

            Text = Text.Insert(CaretPosition, _symbol);
            CaretPosition += _symbol.Length;
            CaretSelectPosition += _symbol.Length;
            UpdateLabel();
        }
    }
}
