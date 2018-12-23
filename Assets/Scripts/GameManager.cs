using Asteroids;
using Cargo;
using GameEvents;
using Manufacture;
using Movement;
using System.Collections.Generic;
using UI;
using UnitSelection;
using UnitTask;
using UnityEngine;
using UserInput;
using static Asteroids.AsteroidGenerator;
using static UI.UISystem;

public class GameManager : MonoBehaviour
{

    // Game setups
    public int GameSeed;
    public bool RandomSeed;
    public Transform AsteroidParent;
    public AsteroidSettingsEditable AsteroidSettings;
    public UISettings UISettings;

    // Game-wide systems
    public static EventSystem EventSystem = new EventSystem();
    public static SelectionSystem SelectionSystem = new SelectionSystem();
    public static TaskSystem TaskSystem = new TaskSystem();
    public static MovementSystem MovementSystem = new MovementSystem();
    public static ManufactureSystem ManufactureSystem = new ManufactureSystem();
    public static CargoRepo CargoRepo = new CargoRepo();
    public static UISystem UISystem;
    public static UserInputSystem UserInput;
    public static AsteroidGenerator AsteroidGenerator;

    private void Awake()
    {
        if (RandomSeed)
        {
            GameSeed = (int)System.DateTime.Now.Ticks;
            Debug.Log("Random game seed: " + GameSeed);
        }
        Random.InitState(GameSeed);

        UISystem = new UISystem(UISettings);
        UserInput = new UserInputSystem();
        AsteroidGenerator = new AsteroidGenerator(AsteroidSettings.GetSettings());
    }

    private void Start()
    {
        for (int i = -3; i < 3; i++)
        {
            for (int j = -3; j < 3; j++)
            {
                GameObject go = AsteroidGenerator.Generate();
                go.GetComponent<AsteroidComponent>().OreAmount = new Cargo.OreAmount(1000000, new Dictionary<Cargo.Matter, int>()
                {
                    { Cargo.Matter.Iron, 1 }
                });
                go.transform.SetParent(AsteroidParent);
                go.transform.position = new Vector3(val(i), val(j), 0);
            }
        }
    }

    private float val(int i)
    {
        return i * (AsteroidSettings.Scale * 2f);
    }

    private void Update()
    {
        UserInput.Update();
        TaskSystem.Update();
        MovementSystem.Update();
        ManufactureSystem.Update();
    }
}
