using EventSystem;
using UnityEngine;

class InputEvent : IEventData
{
    public enum InputType { Primary, Secondary, PrimaryShift, SecondaryShift }

    protected Topic topic = new Topic("input");

    public Vector2 Position { get; protected set; }
    public InputType Type { get; protected set; }

    public InputEvent(Vector2 position, InputType type)
    {
        Position = position;
        Type = type;
    }

    public Topic GetTopic()
    {
        return topic;
    }
}
