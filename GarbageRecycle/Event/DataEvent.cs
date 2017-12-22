namespace EventCenter
{
    public static class DataEvent
    {
        public delegate void DataTriggerHandler(DataType type, int data);
        public static event DataTriggerHandler DataTriggerEvent;

        public static void OnDataTriggerEvent(DataType type, int data)
        {
            if (DataTriggerEvent != null)
                DataTriggerEvent(type, data);
        }
    }
}