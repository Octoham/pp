using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;
    public Box mapBounds;

    private float xMin, xMax, yMin, yMax;
    private float camY, camX;
    private float camOrthsize;
    private float cameraRatio;
    private Camera mainCam;

    private void Start()
    {
        mapBounds = CameraBoundary.boundary;
        xMin = mapBounds.bottomLeft.x;
        xMax = mapBounds.topRight.x;
        yMin = mapBounds.bottomLeft.y;
        yMax = mapBounds.topRight.y;
        mainCam = GetComponent<Camera>();
        camOrthsize = mainCam.orthographicSize;
        cameraRatio = (xMax + camOrthsize) / 2.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followTransform == null && Player.localPlayer.transform != null)
        {
            followTransform = Player.localPlayer.transform;
        }
        if (followTransform != null)
        {
            camY = Mathf.Clamp(followTransform.position.y, yMin + camOrthsize, yMax - camOrthsize);
            camX = Mathf.Clamp(followTransform.position.x, xMin + cameraRatio, xMax - cameraRatio);
            mainCam.transform.position = new Vector3(camX, camY, -10f);
        }


    }
}