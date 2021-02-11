using System;

public enum ShipBody
{
    Standard = 0,
    Square,
    ShipBody3,
    ShipBody4
}

public enum ShipTower
{
    Standard = 0,
    ShipTower2,
    ShipTower3,
    ShipTower4
}

public enum State
{
    MainMenu,
    NextLevel,
    Game,
    GameOver
}

[Serializable]
public enum PoolObject
{
    AsteroidA,
    AsteroidB,
    AsteroidC
}