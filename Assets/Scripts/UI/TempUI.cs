using System;
using UnityEngine;
using UnityEngine.UI;

class TempUI : MonoBehaviour
{

    public Canvas DisconnectCanvas = null, ConnectCanvas = null;
    const int POOL_SIZE = 20;
    public GameObject connectPrefab = null;
    Canvas[] unitCanvasPool = new Canvas[POOL_SIZE];
    void Start()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            unitCanvasPool[i] = GameObject.Instantiate(connectPrefab, connectPrefab.transform.parent).GetComponent<Canvas>();
            unitCanvasPool[i].gameObject.SetActive(true);
            unitCanvasPool[i].enabled = false;
            unitCanvasPool[i].name = "UnitCanvas " + i.ToString();
        }
    }


    WorldModel world;
    public void AttachUnitViewsArray(WorldModel world)
    {
        this.world = world;
    }

    void Update()
    {
        if (!Camera.main) return;

        bool hasConnected = false;

        for (int i = 0; i < POOL_SIZE; i++)
        {
            if (i < world.unitsPool.Count)
            {
                if (world.unitsPool.unitModels[i].drivenByUser) hasConnected = true;
                if (!unitCanvasPool[i].enabled) unitCanvasPool[i].enabled = true;
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, world.unitsPool.unitViews[i].transform.position);
                unitCanvasPool[i].GetComponent<RectTransform>().position = screenPoint;
            }
            else
            {
                if (unitCanvasPool[i].enabled) unitCanvasPool[i].enabled = false;
                else break;
            }
        }

        if (hasConnected) DisconnectCanvas.enabled = true;
        else DisconnectCanvas.enabled = false;

        if (hasConnected) ConnectCanvas.enabled = false;
        else ConnectCanvas.enabled = true;
    }


    public void OnConnectButtonClick(Transform source)
    {
        var unitIndex = int.Parse(source.name.Split(' ')[1]);
        if (unitCanvasPool[unitIndex].enabled)
        {
            world.ConnectUser(unitIndex);
        }
    }
    public void OnDisconnectButtonClick()
    {
        for (int i = 0; i < POOL_SIZE; i++)
            if (world.unitsPool.unitModels[i].drivenByUser) world.unitsPool.unitModels[i].DisconnectUser();
    }
    public void OnDifficultChangeClick(int dif)
    {
        world.aiCommander.DifficultChange(dif);
    }

    public void OnAddButtonClick()
    {
        world.unitsPool.AddNewItem();
    }

    public void OnRemoveButtonClick(Transform source)
    {
        var unitIndex = int.Parse(source.name.Split(' ')[1]);
        if (unitCanvasPool[unitIndex].enabled)
        {
            world.unitsPool.RemoveItem(unitIndex);
        }
    }


}