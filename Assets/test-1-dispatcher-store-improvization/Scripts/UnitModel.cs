using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel
{



    public int id;

    public Vector3 worldPosition;
    public Quaternion worldRotation;
    public Quaternion towerLocalRotation;
    public int unitChanged = -1;
    public bool drivenByUser => userInput != null;
    public WorldModel world;
    public bool enabled;
    public UnitView unitView = null;

    UnityLifeCycles_Input userInput;
    public void ConnectUser(UnityLifeCycles_Input UnityLifeCycles_Input)
    {
        if (userInput) return;
        userInput = UnityLifeCycles_Input;
        UnityLifeCycles_Input.onUserInput += onInputEvent;
    }
    public void DisconnectUser()
    {
        if (userInput) userInput.onUserInput -= onInputEvent;
        userInput = null;
    }


    public void ResetUnit(int newId, Vector3 pos, WorldModel world)
    {
        unitChanged = 1 | 2;
        id = newId;
        userInput = null;
        worldPosition = pos;
        worldRotation = Quaternion.identity;
        towerLocalRotation = Quaternion.identity;
        this.world = world;
        enabled = true;
        DispatchAction(UnitActions.SPAWN);
        DispatchAction(UnitActions.RELOAD, UnityEngine.Random.Range(0, 2f));
    }


    IEnumerator MOVE_ACTION_STACK, TOWER_ACTION_STACK, FIRE_ACTION_STACK, LOCK_ACTIONS;
    public void onInputEvent(InputData input)
    {


        unitChanged = 0;

        if (LOCK_ACTIONS != null && LOCK_ACTIONS.MoveNext())
        {
            if ((bool)LOCK_ACTIONS.Current) unitChanged |= 8;
        }
        else
        {

            if (!enabled) return;

            // Move Body
            if (MOVE_ACTION_STACK != null && MOVE_ACTION_STACK.MoveNext())
            {
                if ((bool)MOVE_ACTION_STACK.Current) unitChanged |= 1;
            }
            else if (input.MOVE_DIR != Vector2.zero)
                if (Default_MoveAction(input.MOVE_DIR, input.SPEED_MULTIPLY))
                    unitChanged |= 1;


            // Move Tower
            if (TOWER_ACTION_STACK != null && TOWER_ACTION_STACK.MoveNext())
            {
                if ((bool)TOWER_ACTION_STACK.Current) unitChanged |= 2;
            }
            else if (input.MOUSE_TARGET_POINT.HasValue)
                if (Default_TowerAction(input.MOUSE_TARGET_POINT.Value))
                    unitChanged |= 2;

            // Fire
            if (FIRE_ACTION_STACK != null && FIRE_ACTION_STACK.MoveNext())
            {
                if ((bool)FIRE_ACTION_STACK.Current) unitChanged |= 4;
            }
            else if (input.DO_ACTION_0)
                if (Default_FireAction())
                    unitChanged |= 4;

        }

        // Callback for input or ai
        if (input.callback != null)
            input.callback(this);
    }


    void DispatchAction(int ACTION_TYPE, object payload = null)
    {
        if (ACTION_TYPE == UnitActions.SPAWN) MOVE_ACTION_STACK = UnitActions.SPAWN_ACTION(this).GetEnumerator();
        if (ACTION_TYPE == UnitActions.RELOAD) FIRE_ACTION_STACK = UnitActions.RELOAD_ACTION(this, payload).GetEnumerator();
        if (ACTION_TYPE == UnitActions.DIE) LOCK_ACTIONS = UnitActions.DIE_ACTION(this).GetEnumerator();
    }
    public void CreateDieAction()
    {
        DispatchAction(UnitActions.DIE);
    }

    static float UNITY_SPEED = 3;
    Vector2 lastMoveVector;
    float lastSpeedMultiply;
    int lastFrame = 0;
    bool Default_MoveAction(Vector2 moveVector, float? SPEED_MULTIPLY)
    {
        lastMoveVector = moveVector;
        var speed = UNITY_SPEED * (SPEED_MULTIPLY ?? 1);
        lastSpeedMultiply = speed;
        lastFrame = Time.frameCount;

        moveVector.Normalize();
        worldPosition.x += moveVector.x * speed * Time.deltaTime;
        worldPosition.y = 0;
        worldPosition.z += moveVector.y * speed * Time.deltaTime;
        worldRotation = Quaternion.RotateTowards(worldRotation, Quaternion.LookRotation(new Vector3(moveVector.x, 0, moveVector.y), Vector3.up), Time.deltaTime * speed * 300);
        return true;
    }
    public Vector3 CalcMoveActionForSecond(float second)
    {
        if (Time.frameCount - lastFrame > 3) return worldPosition;
        lastMoveVector.Normalize();
        Vector3 result = Vector3.zero;
        result.x = worldPosition.x + lastMoveVector.x * lastSpeedMultiply * second;
        result.y = worldPosition.y;
        result.z = worldPosition.z + lastMoveVector.y * lastSpeedMultiply * second;
        return result;
    }

    bool Default_TowerAction(Vector3 MOUSE_TARGET_POINT)
    {
        var worldDir = (MOUSE_TARGET_POINT - worldPosition);
        worldDir.y = worldPosition.y;
        worldDir.Normalize();
        var newTowerLocalRotation = Quaternion.LookRotation(worldDir, Vector3.up) * Quaternion.Inverse(worldRotation);

        if (
            (
                Mathf.Abs(towerLocalRotation.x - newTowerLocalRotation.x) > 0.0001f ||
                Mathf.Abs(towerLocalRotation.y - newTowerLocalRotation.y) > 0.0001f ||
                Mathf.Abs(towerLocalRotation.z - newTowerLocalRotation.z) > 0.0001f ||
                Mathf.Abs(towerLocalRotation.w - newTowerLocalRotation.w) > 0.0001f
                )
         )
        {
            towerLocalRotation = newTowerLocalRotation;
            return true;
        }
        return false;
    }

    bool Default_FireAction()
    {
        var muzzleTransform = unitView.muzzle;
        var bullet = GameObject.Instantiate(world.bullet_prefab, muzzleTransform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.direction = muzzleTransform.forward;
        bullet.sourceId = id;
        bullet.worldModel = world;
        bullet.gameObject.SetActive(true);
        DispatchAction(UnitActions.RELOAD);
        return true;
    }
}
