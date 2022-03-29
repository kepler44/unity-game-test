using System;
using System.Collections;
using UnityEngine;


static class UnitActions
{
    static public int MOVE_UNIT = "MOVE_UNIT".GetHashCode();
    static public int ROTATE_TOWER = "ROTATE_TOWER".GetHashCode();
    static public int FIRE = "FIRE".GetHashCode();
    static public int RELOAD = "RELOAD".GetHashCode();
    static public int SPAWN = "SPAWN".GetHashCode();
    static public int DIE = "DIE".GetHashCode();


    static public IEnumerable SPAWN_ACTION(UnitModel u)
    {
        float timeLeft = 1;
        while (timeLeft > 0)
        {
            u.worldPosition.y = timeLeft * 5;
            yield return true;
            timeLeft -= Time.deltaTime;
        }
    }

    static public IEnumerable RELOAD_ACTION(UnitModel u, object payload)
    {
        float timeLeft = 2;
        if (payload != null) timeLeft = (float)payload;
        while (timeLeft > 0)
        {
            yield return false;
            timeLeft -= Time.deltaTime;
        }
    }

    static public IEnumerable DIE_ACTION(UnitModel u)
    {
        yield return true;
        float timeLeft = 3;
        u.enabled = false;
        while (timeLeft > 0)
        {
            yield return false;
            timeLeft -= Time.deltaTime;
        }
        u.world.unitsPool.RemoveItem(u.id);
    }
}

