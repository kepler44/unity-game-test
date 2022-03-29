
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AiUnit
{
    Ai ai;
    UnitModel unitModel;
    public AiUnit(Ai ai, UnitModel unit)
    {
        this.ai = ai;
        this.unitModel = unit;
    }




    IEnumerator IDLE_MOVE_CURRENT;
    public InputData CalcInputData()
    {

        var inputData = new InputData();

        if (IDLE_MOVE_CURRENT != null && IDLE_MOVE_CURRENT.MoveNext())
        {
            inputData.MOVE_DIR += (Vector2)IDLE_MOVE_CURRENT.Current;
        }
        else
        {
            IDLE_MOVE_CURRENT = ai.aiActions.CreateMoveAction();
        }

        var targetUnitModel = GetNearestEnemy((ai.currentDifficult + 1) * 5);
        if (targetUnitModel != null)
        {
            inputData.MOUSE_TARGET_POINT = ai.aiActions.HandleTowerAttack(unitModel, targetUnitModel);
            inputData.DO_ACTION_0 = true;
            inputData.MOVE_DIR += ai.aiActions.HandleNearEnemyCorrectino(unitModel, targetUnitModel);
        }

        if (ai.currentDifficult > 1) inputData.SPEED_MULTIPLY = 1.25f;

        return inputData;
    }




    UnitModel GetNearestEnemy(float dist)
    {
        var targets = ai.targetWorldModel.unitsPool.unitModels;
        float minMag = float.MaxValue;
        UnitModel targetUnitModel = null;
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].enabled || targets[i].id == unitModel.id) continue;
            var MAG = (targets[i].worldPosition - unitModel.worldPosition).sqrMagnitude;
            if (MAG > dist * dist) continue;
            if (minMag > MAG)
            {
                minMag = MAG;
                targetUnitModel = targets[i];
            }
        }
        return targetUnitModel;
    }

}
