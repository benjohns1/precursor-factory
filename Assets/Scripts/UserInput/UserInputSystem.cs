namespace UserInput
{
    public class UserInputSystem
    {
        internal InputMappings inputMappings = new InputMappings();
        internal CaptureInput captureInput;
        internal MapInput mapInput;

        public UserInputSystem()
        {
            mapInput = new MapInput(inputMappings);
            captureInput = new CaptureInput(inputMappings);
        }

        public void Update()
        {
            captureInput.CaptureLoop();
        }
    }
}