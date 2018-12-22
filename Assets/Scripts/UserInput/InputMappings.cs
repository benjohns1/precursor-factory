using System.Collections.Generic;
using UnityEngine;

namespace UserInput
{
    internal class InputMappings
    {
        internal struct InputMap
        {
            internal KeyCode Key;
            internal Action Action;

            public InputMap(KeyCode key, Action action)
            {
                Key = key;
                Action = action;
            }
        }

        internal List<InputMap> StandardKeys = new List<InputMap>()
        {
            new InputMap(KeyCode.W, Action.PanUp),
            new InputMap(KeyCode.A, Action.PanLeft),
            new InputMap(KeyCode.S, Action.PanDown),
            new InputMap(KeyCode.D, Action.PanRight),
        };

        internal List<InputMap> ModifierKeys = new List<InputMap>()
        {
            new InputMap(KeyCode.LeftShift, Action.MultiModifier),
            new InputMap(KeyCode.LeftShift, Action.FastModifier)
        };

        internal List<InputMap> MouseActions = new List<InputMap>()
        {
            new InputMap(KeyCode.Mouse0, Action.Select),
            new InputMap(KeyCode.Mouse1, Action.DoAction),
        };
    }
}
