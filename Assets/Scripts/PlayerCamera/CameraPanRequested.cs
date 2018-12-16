using GameEvents;

namespace PlayerCamera
{
    class CameraPanRequested : IEvent
    {
        public enum Direction { Up, Down, Left, Right }

        public enum RequestType { Start, Stop }

        protected Topic topic = new Topic("CameraPan");

        public Direction Dir { get; protected set; }
        public RequestType Request { get; protected set; }
        public bool FastSpeed { get; protected set; }

        public CameraPanRequested(Direction dir, RequestType request, bool fastSpeed)
        {
            Dir = dir;
            Request = request;
            FastSpeed = fastSpeed;
        }

        public Topic GetTopic()
        {
            return topic;
        }
    }
}