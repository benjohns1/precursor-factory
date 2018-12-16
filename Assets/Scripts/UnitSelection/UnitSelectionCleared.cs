using GameEvents;
using UnityEngine;

namespace UserInput
{
    class UnitSelectionCleared : IEvent
    {
        protected Topic topic = new Topic("UnitSelection");

        public Topic GetTopic()
        {
            return topic;
        }
    }
}