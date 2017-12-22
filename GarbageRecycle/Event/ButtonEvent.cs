namespace EventCenter
{
    public static class ButtonEvent
    {
        public delegate void ButtonTriggerHandler(ButtonType type);
        public static event ButtonTriggerHandler ButtonTriggerEvent;

        public static void OnButtonTriggerEvent(ButtonType type)
        {
            if (ButtonTriggerEvent != null)
                ButtonTriggerEvent(type);
        }
    }
}
