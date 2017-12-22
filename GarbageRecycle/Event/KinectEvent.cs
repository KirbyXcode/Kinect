namespace EventCenter
{
    public static class KinectEvent
    {
        public delegate void UserDetectedHandler(bool isDetected);
        public static event UserDetectedHandler UserDetectedEvent;

        public static void OnUserDetectedEvent(bool isDetected)
        {
            if (UserDetectedEvent != null)
                UserDetectedEvent(isDetected);
        }
    }
}


