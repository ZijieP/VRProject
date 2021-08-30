/// <summary>
/// This component will draw a gaze point on all the same objects.
/// <summary>

using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using Tags;

namespace VRComponent
{
    
    public class GazePoint : NetworkBehaviour
    {

        public List<Transform> tfGazePointList;
        public Color gazePointColor;
        
        void Update()
        {

            string id = GetComponent<Tag>().id;
            var mlapiNetworkManager = GetComponent<NetworkVariableManager>();
            gazePointColor = mlapiNetworkManager.GazePointColor.Value;


            if (mlapiNetworkManager.GazeObjectName.Value.Equals("Untagged"))
            {
                foreach (Transform tf in tfGazePointList)
                {
                    tf.gameObject.SetActive(false);
                }
            }

            else
            {
                if(TagManager.FindObjsWithTag(mlapiNetworkManager.GazeObjectName.Value) == null)
                    return;
                GameObject[] gos = TagManager.FindObjsWithTag(mlapiNetworkManager.GazeObjectName.Value).ToArray();
                Vector3 relativePosition = mlapiNetworkManager.GazePointRelativePosition.Value;
                int numGOs = gos.Length;// the number of GameObjects who have the same objectname
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
            Destroy(tfGazePoint.GetComponent<SphereCollider>());
            tfGazePoint.gameObject.SetActive(false);
        }

        


    }
}
