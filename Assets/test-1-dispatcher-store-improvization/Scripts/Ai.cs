using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ai
{

    public int currentDifficult = 1;
    public WorldModel targetWorldModel;
    public AiActions_Root aiActions;
    public Ai(WorldModel targetWorldModel)
    {
        this.targetWorldModel = targetWorldModel;
        DifficultChange(null);
    }

    Dictionary<int, AiUnit> _AiUnit = new Dictionary<int, AiUnit>();
    public void Command(UnitModel unit)
    {
        if (!_AiUnit.ContainsKey(unit.id))
            _AiUnit.Add(unit.id, new AiUnit(this, unit));
        var inputData = _AiUnit[unit.id].CalcInputData();
        unit.onInputEvent(inputData);
    }


    public void DifficultChange(int? dif)
    {
        Debug.Log(dif);
        switch (dif ?? currentDifficult)
        {
            case 0: aiActions = new AiActions_Dif0(); break;
            case 1: aiActions = new AiActions_Dif1(); break;
            case 2: aiActions = new AiActions_Dif2(); break;
        }
    }

}