using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public float maxAcceleration = 1f;

    public Vector2 currentVelocity = new Vector2(0, 0);

    private void Awake()
    {
        GameManager.MovementSystem.Register(this);
    }

    private void OnDestroy()
    {
        GameManager.MovementSystem.Unregister(this);
    }
}
