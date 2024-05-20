using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float FOVscale = 10f;
    float FOVscaleMin = 24f;
    float FOVscaleMax = 80f;

    private void Update()
    {
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (Input.mouseScrollDelta.y != 0)
        {
            float scrollResult = Camera.main.fieldOfView + Input.mouseScrollDelta.y * FOVscale;
            if (scrollResult > FOVscaleMax || scrollResult < FOVscaleMin)
            {
                return;
            }

            Camera.main.fieldOfView = scrollResult;
        }

    }

}
