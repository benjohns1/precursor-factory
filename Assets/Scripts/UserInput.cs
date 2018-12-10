using UnityEngine;

public class UserInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameManager.EventSystem.Add(new InputEvent(new Vector2(worldPoint.x, worldPoint.y), ShiftDown() ? InputEvent.InputType.PrimaryShift : InputEvent.InputType.Primary));
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameManager.EventSystem.Add(new InputEvent(new Vector2(worldPoint.x, worldPoint.y), ShiftDown() ? InputEvent.InputType.SecondaryShift : InputEvent.InputType.Secondary));
        }
    }

    protected static bool ShiftDown()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
}
