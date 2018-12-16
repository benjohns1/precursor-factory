using System.Collections.Generic;

namespace GameEvents
{
    public class Topic : ValueObject
    {
        public string Name { get; protected set; }

        private Topic() { }

        public Topic(string topicName)
        {
            Name = topicName;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }
    }
}
