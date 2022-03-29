using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel : MonoBehaviour
{

    public Material sourceMaterial = null;
    public GameObject bullet_prefab = null;
    public Ai aiCommander;
    public WorldUnitsPool unitsPool = new WorldUnitsPool();
    void Start()
    {
        FindObjectOfType<TempUI>().AttachUnitViewsArray(this);
        aiCommander = new Ai(this);
        unitsPool.InitPool(20, this);
        for (int i = 0; i < 3; i++) unitsPool.AddNewItem();
    }

    public void ConnectUser(int i)
    {
        var UnityLifeCycles_Input = FindObjectOfType<UnityLifeCycles_Input>();
        unitsPool.unitModels[i].ConnectUser(UnityLifeCycles_Input);
    }


    void Update()
    {
        for (int i = 0; i < unitsPool.Count; i++)
        {
            if (!unitsPool.unitModels[i].drivenByUser) aiCommander.Command(unitsPool.unitModels[i]);

            if (unitsPool.unitModels[i].unitChanged != 0) unitsPool.unitModels[i].unitView.Apply(unitsPool.unitModels[i]);
        }
    }


}

