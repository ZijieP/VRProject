// using UnityEngine;
// using System.Collections.Generic;
// using MLAPI.NetworkVariable.Collections;
// using MLAPI;
// using MLAPI.NetworkVariable;
// using MLAPI.Messaging;
// using System;

// /// <summary>
// /// Class for the Arm IK for RocketBox avatar
// /// Attach this script to RocketBox avatar gameobject.
// /// </summary>
// namespace VRMeeting
// {
//     public class ArmIKSolver : NetworkBehaviour
//     {
//         public bool enableLeftArm = true;
//         public bool enableRightArm = true;
//         public Transform ControllerL;
//         public Transform ControllerR;
//         public Transform vrHelmet;


//         private RocketBoxArm armL;
//         private RocketBoxArm armR;
//         private Transform pelvis, head, eyes;
//         string nameAvatar = "Male_Adult_01";

//         void Start()
//         {

//         }

//         public override void NetworkStart()
//         {
//             pelvis = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis");
//             head = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
//             eyes = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head/Bip01 MMiddleEyebrow");
//             Transform handBoneL = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
//             Transform handBoneR = transform.Find(nameAvatar + "/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
//             if (IsLocalPlayer)
//             {
//                 vrHelmet = GameObject.Find("VRHelmet").transform;
//                 ControllerL = GameObject.Find("ControllerL").transform;
//                 ControllerR = GameObject.Find("ControllerR").transform;
//             }
//             armL = new RocketBoxArm(handBoneL, new Vector3(), new Quaternion(), new Vector3(0.0f, -90.0f, 180.0f));
//             armR = new RocketBoxArm(handBoneR, new Vector3(), new Quaternion(), new Vector3(180.0f, 90.0f, 0.0f));
//         }

//         void Update()
//         {
//         }

//         void LateUpdate()
//         {
//             updateArm();
//         }

        

//         void updateArm()
//         {
//             if (IsLocalPlayer)
//             {
//                 if (enableLeftArm)
//                 {
//                     armL.updateEffectorInfo(ControllerL.position, ControllerL.rotation);
//                     armL.ApplyEffector(-90);
//                 }
//                 if (enableRightArm)
//                 {
//                     armR.updateEffectorInfo(ControllerR.position, ControllerR.rotation);
//                     armR.ApplyEffector(1);
//                 }
//             }
//             else
//             {
//                 var mlapiNetworkManager = transform.gameObject.GetComponent<MLAPINetworkManager>();
//                 if (mlapiNetworkManager)
//                 {
//                     long id = (long)GetComponent<NetworkObject>().NetworkObjectId;
//                     if (enableLeftArm)
//                     {
//                         if (mlapiNetworkManager.ControllerLPositionDict.ContainsKey(id))
//                         {
//                             armL.updateEffectorInfo(mlapiNetworkManager.ControllerLPositionDict[id], mlapiNetworkManager.ControllerLRotationDict[id]);
//                             armL.ApplyEffector(-90);
//                         }

//                     }
//                     if (enableRightArm)
//                     {
//                         if (mlapiNetworkManager.ControllerRPositionDict.ContainsKey(id))
//                         {
//                             armR.updateEffectorInfo(mlapiNetworkManager.ControllerRPositionDict[id], mlapiNetworkManager.ControllerRRotationDict[id]);
//                             armR.ApplyEffector(1);
//                         }
//                     }
//                 }


//             }
//         }



//         /// <summary>
//         /// Class for RocketBox avatar's arm, including 3 bones.
//         /// </summary>
//         class RocketBoxArm
//         {
//             // only deal with IK when arm is complete (no bone missing).
//             public bool isComplete = false;
//             public Vector3 positionEffector;
//             public Quaternion rotationEffector;
//             Transform localEffector;
//             public Transform hand;
//             public Transform forearm;
//             public Transform upperarm;
//             public Vector3 offset;
//             public Vector3 iaPosition;
//             public Vector3 ibPosition;
//             public Vector3 icPosition;
//             Dictionary<string, Quaternion> originalAvatarAbsoluteRotations;

//             public RocketBoxArm(Transform handBoneX, Vector3 positionEffectorX, Quaternion rotationEffectorX, Vector3 offsetX)
//             {
//                 isComplete = false;

//                 if (handBoneX == null)
//                     return;

//                 hand = handBoneX;
//                 if (hand == null)
//                     return;
//                 forearm = hand.parent;
//                 if (forearm == null)
//                     return;

//                 upperarm = forearm.parent;
//                 if (upperarm == null)
//                     return;

//                 isComplete = true;

//                 iaPosition = upperarm.position;
//                 ibPosition = forearm.position;
//                 icPosition = hand.position;
//                 originalAvatarAbsoluteRotations = new Dictionary<string, Quaternion>();
//                 originalAvatarAbsoluteRotations.Add(hand.name, hand.rotation);
//                 originalAvatarAbsoluteRotations.Add(forearm.name, forearm.rotation);
//                 originalAvatarAbsoluteRotations.Add(upperarm.name, upperarm.rotation);

//                 offset = offsetX;

//                 positionEffector = positionEffectorX;
//                 rotationEffector = rotationEffectorX;
//             }


//             public void updateEffectorInfo(Vector3 position, Quaternion rotation)
//             {
//                 positionEffector = position;
//                 rotationEffector = rotation;
//             }

//             /// <summary>
//             /// If the arm is detected complete, let the arm move along with hand (wrist) joint according to the effector's position and rotation. 
//             /// Call this method in your LateUpdate() 
//             /// </summary>
//             public void ApplyEffector(int deg)
//             {
//                 if (isComplete)
//                 {
//                     Solve(deg);
//                 }
//             }

//             /// <summary>
//             /// Returns the angle needed between v1 and v2 so that their extremities are
//             /// spaced with a specific length.
//             /// </summary>
//             /// <returns>The angle between v1 and v2.</returns>
//             /// <param name="aLen">The desired length between the extremities of v1 and v2.</param>
//             /// <param name="v1">First triangle edge.</param>
//             /// <param name="v2">Second triangle edge.</param>
//             private static float TriangleAngle(Vector3 hypotenuse, Vector3 v1, Vector3 v2)
//             {
//                 float hLen = hypotenuse.magnitude;
//                 float abLen = v1.magnitude;
//                 float bcLen = v2.magnitude;
//                 float c = Mathf.Clamp((abLen * abLen + bcLen * bcLen - hLen * hLen) / (abLen * bcLen * 2.0f), -1.0f, 1.0f);
//                 //if((abLen * abLen + bcLen * bcLen - hLen * hLen)>0)
//                 return Mathf.Acos(c);
//                 //else
//                 //  return 0;
//             }

//             private void Solve(int deg)
//             {

//                 Quaternion aRotation = upperarm.rotation;
//                 Quaternion bRotation = forearm.rotation;
//                 Quaternion eRotation = rotationEffector * Quaternion.Euler(offset);

//                 Vector3 aPosition = upperarm.position;
//                 Vector3 bPosition = forearm.position;
//                 Vector3 cPosition = hand.position;
//                 Vector3 ePosition = positionEffector;


//                 Vector3 ab = bPosition - aPosition;
//                 Vector3 bc = cPosition - bPosition;
//                 Vector3 ac = cPosition - aPosition;
//                 Vector3 ae = ePosition - aPosition;
//                 if (ae.magnitude > (icPosition - iaPosition).magnitude)
//                 {
//                     ae.Normalize();
//                     ae *= (icPosition - iaPosition).magnitude;
//                     return;
//                 }

//                 float abcAngle = TriangleAngle(ac, ab, bc);
//                 float abeAngle = TriangleAngle(ae, ab, bc);

//                 float angle = (abcAngle - abeAngle) * Mathf.Rad2Deg;
//                 Vector3 axis = Vector3.Cross(ibPosition - iaPosition, icPosition - ibPosition).normalized;

//                 Quaternion fromToRotation = Quaternion.AngleAxis(angle, axis);

//                 Quaternion worldQ = fromToRotation * bRotation;

//                 forearm.rotation = worldQ;
//                 FixRoll(forearm);

//                 cPosition = hand.position;
//                 ac = cPosition - aPosition;
//                 Quaternion fromTo = Quaternion.FromToRotation(ac, ae);
//                 upperarm.rotation = fromTo * aRotation;
//                 FixRoll(upperarm);
//                 hand.rotation = eRotation;
//                 //FixRoll(hand);
//             }

//             private void FixRoll(Transform currentbone)
//             {
//                 // Rotate the X axis from the direction in the original Avatar Absolute Rotation to the desired direction.

//                 Vector3 Orig_Right = originalAvatarAbsoluteRotations[currentbone.name] * new Vector3(1, 0, 0);
//                 Vector3 New_Right = currentbone.right;

//                 Vector3 Axis = Vector3.Cross(Orig_Right, New_Right);
//                 float angle = Vector3.Angle(Orig_Right, New_Right);

//                 currentbone.rotation = originalAvatarAbsoluteRotations[currentbone.name];
//                 //const float epsilon = 0.000000001f;
//                 // if (angle >= epsilon)
//                 currentbone.transform.Rotate(Axis, angle, Space.World);
//             }
//         }
//     }
// }