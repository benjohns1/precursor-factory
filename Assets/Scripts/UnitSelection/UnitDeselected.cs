using GameEvents;
using UnityEngine;

namespace UserInput
{
    class UnitDeselected : IEvent
    {
        public enum SelectionType { Single, Multi }

        protected Topic topic = new Topic("UnitSelection");

        public SelectableComponent Deselected { get; protected set; }
        public SelectionType Type { get; protected set; }

        public UnitDeselected(SelectableComponent deselected, SelectionType selectionType)
        {
            Deselected = deselected;
            Type = selectionType;
        }

        public Topic GetTopic()
        {
            return topic;
        }
    }
}