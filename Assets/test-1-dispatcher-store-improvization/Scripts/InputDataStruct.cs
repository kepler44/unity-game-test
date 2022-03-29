using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public struct InputData
{
    public Vector2 MOVE_DIR;
    public Vector3? MOUSE_TARGET_POINT;
    public bool DO_ACTION_0;
    public bool DO_ACTION_1;
    public float? SPEED_MULTIPLY;

    public Action<UnitModel> callback;
}