using UnityEngine;

[RequireComponent(typeof(Cargo.CargoComponent))]
public class ManufactureComponent : MonoBehaviour
{
    public int DrillOrePerHour = 0;

    private void Awake()
    {
        GameManager.ManufactureSystem.Register(this);
    }

    private void OnDestroy()
    {
        GameManager.ManufactureSystem.Unregister(this);
    }
}
