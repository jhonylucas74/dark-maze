using UnityEngine;

public static class Events
{
    public static Vector3Event OnMazeGenerated;

    public static FloatEvent OnBatteryLampUpdate;
}

public delegate void SimpleEvent();
public delegate void IntEvent(int i);
public delegate void FloatEvent(float f);
public delegate void BoolEvent(bool b);
public delegate void Vector3Event(Vector3 v3);