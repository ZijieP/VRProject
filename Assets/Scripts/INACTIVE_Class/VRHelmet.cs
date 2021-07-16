using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class VRHelmet
{
    Vector3 position;
    Quaternion rotation;
    Vector3 cameraPosition;
    Quaternion cameraRotation;


    public Vector3 Position { get => position; set => position = value; }
    public Quaternion Rotation { get => rotation; set => rotation = value; }
    public Vector3 CameraPosition { get => cameraPosition; set => cameraPosition = value; }
    public Quaternion CameraRotation { get => cameraRotation; set => cameraRotation = value; }
    

    public VRHelmet(Vector3 position, Quaternion rotation, Vector3 cameraPosition, Quaternion cameraRotation)
    {
        Position = position;
        Rotation = rotation;
        CameraPosition = cameraPosition;
        CameraRotation = cameraRotation;
        
    }

    public VRHelmet()
    {
    }

    public Ray getEyeRay()
    {
        return new Ray(cameraPosition, cameraRotation.eulerAngles);
    }

}