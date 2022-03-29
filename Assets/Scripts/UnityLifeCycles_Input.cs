using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UnityLifeCycles_Input : MonoBehaviour
{

    public Transform CameraAnchor = null;
    public Transform CameraBackgroundAnchor = null;
    Vector3 cameraTarget;


    public Action<InputData> onUserInput;


    void Start()
    {
        CameraAnchor.position = CameraBackgroundAnchor.position;
    }

    void Update()
    {
        if (!Camera.main) return;

        var inputData = new InputData();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputData.MOVE_DIR.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputData.MOVE_DIR.x += 1;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputData.MOVE_DIR.y += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputData.MOVE_DIR.y -= 1;


        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) inputData.DO_ACTION_0 = true;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Mouse1)) inputData.DO_ACTION_1 = true;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                inputData.MOUSE_TARGET_POINT = hit.point;
        }

        inputData.callback = CameraApply;

        cameraTarget = CameraBackgroundAnchor.position;
        if (onUserInput != null) onUserInput(inputData);
        CameraAnchor.position = Vector3.MoveTowards(CameraAnchor.position, cameraTarget, Time.deltaTime * 100);
    }

    void CameraApply(UnitModel m)
    {
        cameraTarget = m.worldPosition;
    }


}
