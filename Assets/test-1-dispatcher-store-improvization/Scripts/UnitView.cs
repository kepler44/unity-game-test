using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{



    public ParticleSystem firePartics = null;
    public ParticleSystem explodePartics = null;
    public int id;
    public Transform muzzle = null;
    public Transform towerTransform = null,
    trucks = null;

    public void Apply(UnitModel unitModel)
    {
        if ((unitModel.unitChanged & 1) != 0)
        {

            unitModel.worldPosition.x = Mathf.Clamp(unitModel.worldPosition.x, WorldUnitsPool.minSpawnPoint.x, WorldUnitsPool.maxSpawnPoint.x);
            unitModel.worldPosition.z = Mathf.Clamp(unitModel.worldPosition.z, WorldUnitsPool.minSpawnPoint.z, WorldUnitsPool.maxSpawnPoint.z);
            transform.SetPositionAndRotation(unitModel.worldPosition, unitModel.worldRotation);
            trucks.localPosition = new Vector3(0, UnityEngine.Random.Range(0.40f, 0.55f), 0);
        }

        if ((unitModel.unitChanged & 2) != 0)
            towerTransform.localRotation = unitModel.towerLocalRotation;

        if ((unitModel.unitChanged & 4) != 0) firePartics.Play(true);

        if ((unitModel.unitChanged & 8) != 0) explodePartics.Play(true);

    }
}