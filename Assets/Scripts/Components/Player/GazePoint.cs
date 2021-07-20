using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using Tags;
namespace VRMeeting
{
    public class GazePoint : NetworkBehaviour
    {

        public LineRenderer rayLine;
        public List<Transform> tfGazePointList;
        public Color gazePointColor;

        // Start is called before the first frame update
        void Start()
        {
            /* Ray Line*/
            rayLine = GetComponent<LineRenderer>();
            /* Ray Line*/
        }


        /**
            function:Update is called once per frame
        **/
        void Update()
        {

            string id = GetComponent<Tag>().id;
            var mlapiNetworkManager = GetComponent<MLAPINetworkManager>();
            gazePointColor = mlapiNetworkManager.GazePointColor.Value;


            if (mlapiNetworkManager.GazeObjectName.Value.Equals("Untagged"))
            {
                foreach (Transform tf in tfGazePointList)
                {
                    tf.gameObject.SetActive(false);
                }
            }
            /*
                ******************************.Contains("Gaze")**********************************
            */
            // else if(mlapiNetworkManager.GazeObjectNameDict[id].Contains("Gaze"))
            else
            {
                /*  Gaze points */
                if(TagManager.FindObjsWithTag(mlapiNetworkManager.GazeObjectName.Value) == null)
                    return;
                GameObject[] gos = TagManager.FindObjsWithTag(mlapiNetworkManager.GazeObjectName.Value).ToArray();
                Vector3 relativePosition = mlapiNetworkManager.GazePointRelativePosition.Value;
                int numGOs = gos.Length;
                int numGazePoints = tfGazePointList.Count;
                if (numGazePoints > numGOs)
                {
                    for (int i = numGOs; i < numGazePoints; i++)
                    {
                        tfGazePointList[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < numGOs - numGazePoints; i++)
                    {
                        GameObject GazePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        initGazePoint(GazePoint.transform);
                        tfGazePointList.Add(GazePoint.transform);
                    }
                }
                for (int i = 0; i < numGOs; i++)
                {
                    tfGazePointList[i].position = gos[i].transform.TransformPoint(relativePosition);
                    if (tfGazePointList[i].gameObject.activeSelf == false)
                    {
                        tfGazePointList[i].gameObject.SetActive(true);
                    }
                }
                /*  Gaze points */

                /* Motion according to Gaze */

                /* Motion according to Gaze */
            }




            /* Draw the ray line */
            // drawRayLine(mlapiNetworkManager.EyeTrackingRayDict[(long)GetComponent<NetworkObject>().NetworkObjectId]);
            /* Draw the ray line */
        }

        void initGazePoint(Transform tfGazePoint)
        {
            tfGazePoint.gameObject.GetComponent<Renderer>().material = Resources.Load("Materials/GazePoint", typeof(Material)) as Material;
            tfGazePoint.gameObject.GetComponent<Renderer>().material.color = gazePointColor;
            tfGazePoint.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            tfGazePoint.gameObject.SetActive(false);
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
        /**
            Draw the ray line
        **/


    }
}
