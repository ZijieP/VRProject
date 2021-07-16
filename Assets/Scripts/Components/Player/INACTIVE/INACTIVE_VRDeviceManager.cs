// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using MLAPI;
// using MLAPI.Messaging;
// using MLAPI.NetworkVariable;
// using MLAPI.NetworkVariable.Collections;
// using Tobii.XR;
// using Tobii.G2OM;

// namespace Test
// {
//     public class VRDeviceManager : NetworkBehaviour
//     {
//         string nameAvatar = "Male_Adult_01";
//         Transform vrDevice;
//         Transform vrHelmet;
//         Transform effectorL;
//         Transform effectorR;

//         /** 
//         In order to extend the ray line
//         1. It must be int, otherwise if it's float we can't let Vector3 multiply with this
//         2. we must initialize it in the Start() or NetworkStart but not here
//         3. It shouldn't be int.MaxValue, the ray line can't be showed when the position is too far away
//         **/
//         public int maxRaylength;
//         public LineRenderer rayLine;



//         // Start is called before the first frame update
//         void Start()
//         {

//         }

//         public override void NetworkStart()
//         {
//             initAttributes();
//             if (IsLocalPlayer)
//             {
//                 initLocalPlayerInNetwork();
//             }
//         }

//         // Update is called once per frame
//         void Update()
//         {
//             updatePlayer();
//         }

//         /**
//         Initialize all the attributes in the object of this class
//         **/
//         void initAttributes()
//         {
//             maxRaylength = 100;
//             /* draw ray */
//             // rayLine = GetComponent<LineRenderer>();
//             /* draw ray */

//             System.Random ran = new System.Random();
//             transform.position = new Vector3(ran.Next(-5, 5), ran.Next(1, 5), ran.Next(-5, 5));
            
//             if(IsLocalPlayer)
//             {
//                 vrDevice = GameObject.Find("VRDevice").transform;
//                 vrHelmet = GameObject.Find("VRHelmet").transform;
//                 vrDevice.position = new Vector3(0, 1.0f, 0);
//                 effectorL = GameObject.Find("EffectorL").transform;
//                 effectorR = GameObject.Find("EffectorR").transform;
//             }
            
//         }

//         /**
//         Initialize the information of the local player in the network.
//         **/
//         [ServerRpc]
//         void initLocalPlayerInNetwork(ServerRpcParams rpcParams = default)
//         {
//             var mlapiNetworkManager = transform.gameObject.GetComponent<MLAPINetworkManager>();
//             if (mlapiNetworkManager)
//             {
//                 long id = (long)GetComponent<NetworkObject>().NetworkObjectId;
//                 mlapiNetworkManager.EyeTrackingRayDict.Add(id, getEyeTrackingRay());
//                 mlapiNetworkManager.VRHelmetPositionDict.Add(id, vrHelmet.position);
//                 mlapiNetworkManager.VRHelmetRotationDict.Add(id, vrHelmet.rotation);
//                 mlapiNetworkManager.EffectorLPositionDict.Add(id, effectorL.position);
//                 mlapiNetworkManager.EffectorLRotationDict.Add(id, effectorL.rotation);
//                 mlapiNetworkManager.EffectorRPositionDict.Add(id, effectorR.position);
//                 mlapiNetworkManager.EffectorRRotationDict.Add(id, effectorR.rotation);
//             }

//         }



//         /**
//         Update the information of player in the network.
//         **/
//         [ServerRpc]
//         void submitLocalPlayerInfo(ServerRpcParams rpcParams = default)
//         {
//             var mlapiNetworkManager = transform.gameObject.GetComponent<MLAPINetworkManager>();
//             if (mlapiNetworkManager)
//             {
//                 long id = (long)GetComponent<NetworkObject>().NetworkObjectId;
//                 mlapiNetworkManager.VRHelmetPositionDict[id] = vrHelmet.position;
//                 mlapiNetworkManager.VRHelmetRotationDict[id] = vrHelmet.rotation;
//                 mlapiNetworkManager.EyeTrackingRayDict[id] = getEyeTrackingRay();
//                 mlapiNetworkManager.EffectorLPositionDict[id] = effectorL.position;
//                 mlapiNetworkManager.EffectorLRotationDict[id] = effectorL.rotation;
//                 mlapiNetworkManager.EffectorRPositionDict[id] = effectorR.position;
//                 mlapiNetworkManager.EffectorRRotationDict[id] = effectorR.rotation;
//             }
//         }

//         /**
//         Update player according to whether the player is local or not
//         **/
//         void updatePlayer()
//         {
//             if (IsLocalPlayer)
//             {
//                 /* draw ray */
//                 // drawRayLine(getEyeTrackingRay());
//                 /* draw ray */
//                 submitLocalPlayerInfo();
//             }
//             else
//             {
//                 /* draw ray */
//                 // var mlapiNetworkManager = transform.gameObject.GetComponent<MLAPINetworkManager>();
//                 // if (mlapiNetworkManager)
//                 // {
//                 //     drawRayLine(mlapiNetworkManager.EyeTrackingRayDict[(long)GetComponent<NetworkObject>().NetworkObjectId]);
//                 // }
//                 /* draw ray */
//             }
//         }

//         /**
//             Draw the ray line
//         **/
//         // void drawRayLine(Ray ray)
//         // {
//         //     rayLine.SetPosition(0, ray.origin);
//         //     RaycastHit hit;
//         //     if (Physics.Raycast(ray, out hit, maxRaylength))
//         //     {
//         //         rayLine.SetPosition(1, hit.point);
//         //     }
//         //     else
//         //     {
//         //         rayLine.SetPosition(1, (ray.origin + ray.direction * maxRaylength));
//         //     }
//         // }
//         /**
//             Draw the ray line
//         **/

//         void printDict(NetworkDictionary<long, Vector3> dict)
//         {
//             foreach (KeyValuePair<long, Vector3> kvp in dict)
//             {
//                 print(kvp.Key + ":" + kvp.Value);
//             }
//         }

//         /**
//             Get the ray of the eye tracker
//         **/
//         Ray getEyeTrackingRay()
//         {
//             var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
//             return new Ray(eyeTrackingData.GazeRay.Origin, eyeTrackingData.GazeRay.Direction);
//         }
//     }
// }
