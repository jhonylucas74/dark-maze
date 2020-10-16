using UnityEngine;

public static class Events
{
    public static SimpleEvent OnLeaveGame;

    public static IntEvent OnTriggerStartGame;
    public static SimpleEvent OnGameStart;
    public static Vector3Event OnMazeGenerated;
    public static SimpleEvent OnGameEnd;

    public static SimpleEvent OnPasswordFound;
    public static SimpleEvent OnLightOn;
    public static SimpleEvent OnLightOff;
    public static FloatEvent OnLampIntensityUpdate;
    public static FloatEvent OnBatteryLampUpdate;
    public static BoolEvent OnDoorTouching;

    public static StringEvent OnPasswordGenerated;
    public static StringEvent OnPasswordButtonPress;
}

public delegate void SimpleEvent();
public delegate void IntEvent(int i);
public delegate void FloatEvent(float f);
public delegate void BoolEvent(bool b);
public delegate void StringEvent(string s);
public delegate void Vector3Event(Vector3 v3);
