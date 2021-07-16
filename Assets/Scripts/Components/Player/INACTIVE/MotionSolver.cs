// using System.Collections.Generic;
// using UnityEngine;
// using MLAPI;
// using Tags;

// namespace VRMeeting
// {
    

//     public class MotionSolver : NetworkBehaviour
//     {
//         public Dictionary<string, float> dictFocusObjectWeight = new Dictionary<string, float>(); // For adjust the body
//         public Transform tFloor;
//         public float RotateAngleUnit = 0.1f;
//         private Transform pelvis, spine1, head, eyes;
//         public Transform effectorL;
//         public Transform effectorR;
//         public Transform vrHelmet;

//         string nameAvatar = "Male_Adult_01";

//         // string nameAvatar = "Male_Adult_01";
//         // public Transform tfPelvis;
//         void Start()
//         {
//             // integrate the dictFocusObjectWeight
            
//         }

//         public override void NetworkStart()
//         {
//             initAttributes();
//         }
//         void initAttributes()
//         {
//             dictFocusObjectWeight.Add("Player", 0.1f);
//             dictFocusObjectWeight.Add("Laptop", 0.1f);
//             tFloor = GameObject.Find("Floor").transform;
            
//             pelvis = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis");
//             head = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
//             eyes = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head/Bip01 MMiddleEyebrow");
//             spine1 = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
//             if (IsLocalPlayer)
//             {
//                 vrHelmet = GameObject.Find("VRHelmet").transform;
//                 effectorL = GameObject.Find("EffectorL").transform;
//                 effectorR = GameObject.Find("EffectorR").transform;
//             }
//         }

//         void Update()
//         {
//             updateHead();
//             var mlapiNetworkManager = transform.gameObject.GetComponent<MLAPINetworkManager>();
//             if (mlapiNetworkManager)
//             {
//                 long id = (long)GetComponent<NetworkObject>().NetworkObjectId;
//                 /* adjust the body according to those objects whose tags contain "Focus"*/ 
//                 List<GameObject> focusObjects = TagManager.FindObjsWithTag("Focus");
//                 Vector3 rotationHelmet = mlapiNetworkManager.VRHelmetRotationDict[id].eulerAngles;
//                 Vector3 positionHelmet = mlapiNetworkManager.VRHelmetPositionDict[id];
                
//                 // tfPelvis.Rotate(new Vector3(0.1f,0,0),Space.Self);

//                 /* adjust all the motion according to the head */
                

//                 // head and body

//                 // hand and body

                
                 
//             }
//         }

//         void updateHead()
//         {
//             if (IsLocalPlayer)
//             {
//                 pelvis.position = vrHelmet.position - (eyes.position - pelvis.position);
//                 head.rotation = vrHelmet.rotation * Quaternion.Euler(new Vector3(0.0f, -90.0f, -90.0f));
//             }
//             else
//             {
//                 var mlapiNetworkManager = transform.gameObject.GetComponent<MLAPINetworkManager>();
//                 if (mlapiNetworkManager)
//                 {
//                     long id = (long)GetComponent<NetworkObject>().NetworkObjectId;
//                     if (mlapiNetworkManager.VRHelmetPositionDict.ContainsKey(id))
//                         pelvis.position = mlapiNetworkManager.VRHelmetPositionDict[id] - (eyes.position - pelvis.position);
//                     if (mlapiNetworkManager.VRHelmetRotationDict.ContainsKey(id))
//                         head.rotation = mlapiNetworkManager.VRHelmetRotationDict[id] * Quaternion.Euler(new Vector3(0.0f, -90.0f, -90.0f));
//                 }
//             }
//         }
//     }
// }