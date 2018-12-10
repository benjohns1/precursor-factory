using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static EventSystem.EventSystem EventSystem;

    private void Awake()
    {
        EventSystem = new EventSystem.EventSystem();
    }
}
