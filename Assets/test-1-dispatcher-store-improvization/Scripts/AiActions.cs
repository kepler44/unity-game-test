

using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class AiActions_Root
{
    public AiActions_Root()
    {
    }

    public IEnumerator CreateMoveAction()
    {
        return MoveAction().GetEnumerator();
    }

    protected abstract IEnumerable MoveAction();
    public abstract Vector3 HandleTowerAttack(UnitModel source, UnitModel target);

    public Vector2 HandleNearEnemyCorrectino(UnitModel source, UnitModel target)
    {
        var dist = (source.worldPosition - target.worldPosition).magnitude;
        if (dist < 2.5f)
        {
            return -(target.worldPosition - source.worldPosition).normalized;
        }
        if (dist > 5f)
        {
            return (target.worldPosition - source.worldPosition).normalized / 2;
        }
        if (dist > 10f)
        {
            return (target.worldPosition - source.worldPosition).normalized * 3;
        }
        return Vector2.zero;
    }

}


public class AiActions_Dif0 : AiActions_Root
{


    protected override IEnumerable MoveAction()
    {
        var distX = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        var distY = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        Vector2 direction = new Vector2(distX, distY);
        float time = UnityEngine.Random.Range(1.5f, 3.5f);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return direction;
        }
        float time_idle = UnityEngine.Random.Range(0.5f, 1.5f);
        while (time_idle > 0)
        {
            time_idle -= Time.deltaTime;
            yield return Vector2.zero;
        }
    }

    public override Vector3 HandleTowerAttack(UnitModel source, UnitModel target)
    {
        var distX = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        var distY = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        Vector3 direction = new Vector3(distX, 0, distY);
        return source.worldPosition + direction;
    }

}

public class AiActions_Dif1 : AiActions_Root
{

    protected override IEnumerable MoveAction()
    {
        var distX = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        var distY = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        Vector2 direction = new Vector2(distX, distY);
        float time = UnityEngine.Random.Range(0.5f, 2.0f);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return direction;
        }
        float time_idle = UnityEngine.Random.Range(0.5f, 1.5f);
        while (time_idle > 0)
        {
            time_idle -= Time.deltaTime;
            yield return Vector2.zero;
        }
    }

    public override Vector3 HandleTowerAttack(UnitModel source, UnitModel target)
    {
        return target.worldPosition;
    }
}


public class AiActions_Dif2 : AiActions_Root
{

    protected override IEnumerable MoveAction()
    {
        var distX = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        var distY = Mathf.Sign(UnityEngine.Random.Range(-1, 1)) * UnityEngine.Random.Range(0.5f, 1.5f);
        Vector2 direction = new Vector2(distX, distY);
        float time = UnityEngine.Random.Range(0.5f, 2.0f);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return direction;
        }
    }

    public override Vector3 HandleTowerAttack(UnitModel source, UnitModel target)
    {
        var dist = (source.worldPosition - target.worldPosition).magnitude;
        var bulletSpeed = 5;
        return target.CalcMoveActionForSecond(dist / bulletSpeed);
    }
}