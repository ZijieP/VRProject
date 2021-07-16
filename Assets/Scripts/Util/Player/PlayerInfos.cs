using UnityEngine;
using Tobii.XR;

using Tobii.G2OM;

namespace PlayerData
{
    public class PlayerInfos : MonoBehaviour
    {
        public static Ray getEyeTrackingRay()
        {
            var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            return new Ray(eyeTrackingData.GazeRay.Origin, eyeTrackingData.GazeRay.Direction);
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

        public static GameObject getGazedObject()
        {
            Ray eyeTrackingRay = getEyeTrackingRay();
            RaycastHit hit;
            if (Physics.Raycast(eyeTrackingRay, out hit))
            {
                return hit.transform.gameObject;
            }
            return null;
        }

        

    }

    
}