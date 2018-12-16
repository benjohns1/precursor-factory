using System;
using UnityEngine;
using GameEvents;
using UnitSelection;
using UserInput;

public class GameManager : MonoBehaviour
{
    public static EventSystem EventSystem = new EventSystem();
    public static SelectionSystem SelectionSystem = new SelectionSystem();
    public static UserInputSystem UserInput;

    private void Awake()
    {
        UserInput = new UserInputSystem();
    }

    private void Update()
    {
        UserInput.Update();
    }
}
