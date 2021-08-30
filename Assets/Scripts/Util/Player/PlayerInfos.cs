/// <summary>
/// This class is used to obtain player information.
/// <summary>

using UnityEngine;
using Tobii.XR;
using Tags;
using VRComponent;


namespace PlayerData
{
    public class PlayerInfos : MonoBehaviour
    {
        public static Ray getEyeTrackingRay()
        {
            var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            return new Ray(eyeTrackingData.GazeRay.Origin, eyeTrackingData.GazeRay.Direction);
        }

        public static Ray getEyeTrackingRay(string playerID)
        {
            GameObject player = TagManager.FindObjByID(playerID);
            Ray ray = player.GetComponent<NetworkVariableManager>().EyeTrackingRay.Value;
            return ray;
        }

        public static GameObject getGazedObject(Ray eyeTrackingRay)
        {
            RaycastHit hit;
            if (Physics.Raycast(eyeTrackingRay, out hit))
            {
                return hit.transform.gameObject;
            }
            return null;
        }
    }
}