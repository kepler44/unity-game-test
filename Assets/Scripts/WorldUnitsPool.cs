using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class WorldUnitsPool
{
    public GameObject unitViewPrefab = null;

    [HideInInspector] public UnitModel[] unitModels = new UnitModel[0];
    [HideInInspector] public UnitView[] unitViews = new UnitView[0];

    public int Count = 0;
    int POOL_SIZE;
    public static Vector3 minSpawnPoint, maxSpawnPoint;
    WorldModel world;

    string[] colors = new[] {"FD2020", "FDCD20","59FD20","20FDA8","2071FD","A620FD","FD20C4",
"874A77", "874A49","878149","588749","149877","496087","6A4987", "87497F", "878787" , "FFFFFF" ,"2E2E2E"};
    public void InitPool(int POOL_SIZE, WorldModel world)
    {
        this.world = world;
        this.POOL_SIZE = POOL_SIZE;
        Array.Resize(ref unitModels, unitModels.Length + POOL_SIZE);
        Array.Resize(ref unitViews, unitViews.Length + POOL_SIZE);
        var ground = GameObject.FindGameObjectWithTag("Ground");
        var v = ground.GetComponent<MeshFilter>().sharedMesh.vertices;
        var min = new Vector3(v.Min(v => v.x), 0, v.Min(v => v.z));
        var max = new Vector3(v.Max(v => v.x), 0, v.Max(v => v.z));
        minSpawnPoint = ground.transform.TransformPoint(min) + new Vector3(0.5f, 0, 0.5f);
        maxSpawnPoint = ground.transform.TransformPoint(max) - new Vector3(0.5f, 0, 0.5f);

        for (int i = 0; i < POOL_SIZE; i++)
        {
            unitModels[i] = new UnitModel();
            unitModels[i].id = i;
            unitViews[i] = GameObject.Instantiate(unitViewPrefab).GetComponent<UnitView>();
            unitViews[Count].gameObject.SetActive(false);
            unitViews[i].id = i;

            var mat = new Material(world.sourceMaterial);
            var c = colors[i % colors.Length];
            var res_color = new Color32();
            res_color.a = 255;
            for (int h = 0; h < 3; h++)
                res_color[h] = (byte)Convert.ToInt32("0x" + c[h * 2] + c[h * 2 + 1], 16);
            mat.SetColor("_Color", res_color);
            foreach (var item in unitViews[i].GetComponentsInChildren<MeshRenderer>())
                item.sharedMaterial = mat;
        }
    }

    public void AddNewItem()
    {
        if (Count > POOL_SIZE) throw new System.IndexOutOfRangeException();
        var x = Mathf.Lerp(minSpawnPoint.x, maxSpawnPoint.x, UnityEngine.Random.Range(0f, 1f));
        var z = Mathf.Lerp(minSpawnPoint.z, maxSpawnPoint.z, UnityEngine.Random.Range(0f, 1f));
        unitModels[Count].ResetUnit(unitModels[Count].id, new Vector3(x, 0, z), world);
        var _id = unitModels[Count].id;
        unitViews[Count].gameObject.SetActive(true);
        Count++;
    }

    public void RemoveItem(int index)
    {
        if (unitModels[index].drivenByUser) unitModels[index].DisconnectUser();
        unitModels[index].enabled = false;
        unitViews[index].gameObject.SetActive(false);

        var t = unitModels[Count - 1];
        unitModels[Count - 1] = unitModels[index];
        unitModels[index] = t;

        var d = unitViews[Count - 1];
        unitViews[Count - 1] = unitViews[index];
        unitViews[index] = d;

        Count--;
    }

    /* 
        int poolPos = 0;
        Canvas GetUnitCanvas()
        {
            Canvas result = null;
            bool overflow = false;
            while ((result = unitCanvasPool[poolPos]).enabled)
            {
                poolPos++;
                if (poolPos == POOL_SIZE)
                {
                    if (overflow) throw new System.IndexOutOfRangeException();
                    overflow = true;
                    poolPos = 0;
                }
            }
            result.enabled = true;
            return result;
        } */

}