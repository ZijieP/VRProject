using Tags;
using UnityEngine;
using MLAPI;
using PlayerData;


namespace VRComponent
{
    public class GazeRay : NetworkBehaviour
    {
        public LineRenderer rayLine;

        void Start()
        {
            /* Ray Line*/
            rayLine = GetComponent<LineRenderer>();
            /* Ray Line*/
        }

        void Update()
        {
            string id = GetComponent<Tag>().id;
            drawRayLine(PlayerInfos.getEyeTrackingRay(id));
        }

        /**
            Draw the ray line
        **/
        void drawRayLine(Ray ray)
        {
            int maxRaylength = 100;
            rayLine.SetPosition(0, ray.origin);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRaylength))
            {
                rayLine.SetPosition(1, hit.point);
            }
            else
            {
                rayLine.SetPosition(1, (ray.origin + ray.direction * maxRaylength));
            }
        }

    }
}