using GameEvents;
using Movement;
using UnitCommand;
using UnitSelection;
using UnityEngine;
using UserInput;

public class GameManager : MonoBehaviour
{
    public static EventSystem EventSystem = new EventSystem();
    public static SelectionSystem SelectionSystem = new SelectionSystem();
    public static CommandSystem CommandSystem = new CommandSystem();
    public static MovementSystem MovementSystem = new MovementSystem();
    public static UserInputSystem UserInput;

    private void Awake()
    {
        UserInput = new UserInputSystem();
    }

    private void Update()
    {
        UserInput.Update();
        CommandSystem.Update();
        MovementSystem.Update();
    }
}
