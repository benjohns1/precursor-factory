using GameEvents;
using UnityEngine;

namespace UserInput
{
    class UnitSelected : IEvent
    {
        public enum SelectionType { Single, Multi }

        protected Topic topic = new Topic("UnitSelection");

        public SelectableComponent Selection { get; protected set; }
        public SelectionType Type { get; protected set; }

        public UnitSelected(SelectableComponent selection, SelectionType selectionType)
        {
            Selection = selection;
            Type = selectionType;
        }

        public Topic GetTopic()
        {
            return topic;
        }
    }
}