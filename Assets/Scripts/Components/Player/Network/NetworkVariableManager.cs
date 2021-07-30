using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;
using System.Collections.Generic;
using PlayerData;
using Tags;
using Valve.VR;
using RootMotion.FinalIK;



namespace VRComponent
{
    public class NetworkVariableManager : NetworkBehaviour
    {
        public NetworkVariableColor GazePointColor = new NetworkVariableColor(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.OwnerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableVector3 VRHelmetPosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableQuaternion VRHelmetRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableVector3 ControllerLPosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableQuaternion ControllerLRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableVector3 ControllerRPosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableQuaternion ControllerRRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableRay EyeTrackingRay = new NetworkVariableRay(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        public NetworkVariableVector3 GazePointRelativePosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            /**
            Value is the position of the Gaze point relative to the object
            If the eye tracking ray don't hit any object, the KeyValuePair will be removed
            **/
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        public NetworkVariableString GazeObjectName = new NetworkVariableString(new NetworkVariableSettings
        {
            /**
            Value is the name of the hit object
            If the eye tracking ray don't hit any object, the KeyValuePair will be removed
            **/
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        // public NetworkDictionary<long, Vector3> PelvisPosition = new NetworkDictionary<long, Vector3>(new NetworkVariableSettings
        // {
        //     WritePermission = NetworkVariablePermission.Everyone,
        //     ReadPermission = NetworkVariablePermission.Everyone
        // });
        // public NetworkDictionary<long, Quaternion> PelvisRotation = new NetworkDictionary<long, Quaternion>(new NetworkVariableSettings
        // {
        //     WritePermission = NetworkVariablePermission.Everyone,
        //     ReadPermission = NetworkVariablePermission.Everyone
        // });
        // public NetworkDictionary<long, Vector3> Bip01PositionDict = new NetworkDictionary<long, Vector3>(new NetworkVariableSettings
        // {
        //     WritePermission = NetworkVariablePermission.Everyone,
        //     ReadPermission = NetworkVariablePermission.Everyone
        // });
        // public NetworkDictionary<long, Quaternion> Bip01RotationDict = new NetworkDictionary<long, Quaternion>(new NetworkVariableSettings
        // {
        //     WritePermission = NetworkVariablePermission.Everyone,
        //     ReadPermission = NetworkVariablePermission.Everyone
        // });



        Transform vrDevice, vrHelmet, controllerL, controllerR;
        Transform bip01, eyes, head, handL, handR, toe;
        Transform fVRHelmet, fControllerL, fControllerR;
        Transform floor;

        /** 
        In order to extend the ray line
        1. It must be int, otherwise if it's float we can't let Vector3 multiply with this
        2. we must initialize it in the Start() or NetworkStart but not here
        3. It shouldn't be int.MaxValue, the ray line can't be showed when the position is too far away
        **/
        public int maxRaylength;


        // Vector3 offset_rotation_head, offset_rotation_handR, offset_rotation_handL;
        Vector3 offset_position_head_bip01;
        Vector3 relative_position_handL_head, relative_position_handR_head;


        // Start is called before the first frame update
        void Start()
        {

        }

        public override void NetworkStart()
        {
            initBodyTF();
            if (IsLocalPlayer)
            {
                initAttribute();
                initVRDevice();
                // initLocalPlayerInNetwork();
            }
        }



        private void initAttribute()
        {
            // offset_rotation_handL = new Vector3(180.0f, 0.0f, 180.0f);
            // offset_rotation_handR = new Vector3(180.0f, 0.0f, 0.0f);
            // offset_rotation_head = new Vector3(0.0f, -90.0f, -90.0f);
            offset_position_head_bip01 = head.position - bip01.position;
            relative_position_handL_head = head.InverseTransformPoint(handL.position);
            relative_position_handR_head = head.InverseTransformPoint(handR.position);

        }

        private void initBodyTF()
        {
            head = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
            handL = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
            handR = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
            bip01 = transform.Find("Bip01");
            // bip01 = transform.Find("/Bip01/Bip01 Pelvis");
            head = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
            eyes = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head/Bip01 MMiddleEyebrow");
            toe = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0");
            fVRHelmet = transform.Find("FVRHelmet");
            fControllerL = transform.Find("FControllerL");
            fControllerR = transform.Find("FControllerR");
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsLocalPlayer)
            {
                updateLocalPlayerInNetWork();
            }
            updatePlayerBody();

        }

        /**
        Initialize all the attributes in the object of this class
        **/
        void initVRDevice()
        {
            System.Random ran = new System.Random();
            vrDevice = GameObject.Find("VRDevice").transform;
            vrHelmet = GameObject.Find("VRHelmet").transform;
            // vrDevice.position = new Vector3(ran.Next(-3,3), 2f, ran.Next(-3,3));
            // Transform tfloor = GameObject.Find("Floor").transform;
            // vrDevice.position = new Vector3(0, vrDevice.localScale.y/2 + tfloor.localScale.y/2, 0);
            floor = GameObject.Find("Plane").transform;
            vrDevice.position = new Vector3(0, (head.position.y - toe.position.y + floor.localScale.y / 2) / 2, 0);
            controllerL = GameObject.Find("ControllerL").transform;
            controllerR = GameObject.Find("ControllerR").transform;
            transform.position = vrHelmet.position - offset_position_head_bip01;
        }

        /**
        Initialize the information of the local player in the network.
        **/

        // void initLocalPlayerInNetwork(ServerRpcParams rpcParams = default)
        // {
        //     string id = GetComponent<Tag>().id;
        //     VRHelmetPositionDict.Add(id, vrHelmet.position);
        //     VRHelmetRotationDict.Add(id, vrHelmet.rotation);
        //     EyeTrackingRayDict.Add(id, PlayerInfos.getEyeTrackingRay());
        //     ControllerLPositionDict.Add(id, controllerL.position);
        //     ControllerLRotationDict.Add(id, controllerL.rotation);
        //     ControllerRPositionDict.Add(id, controllerR.position);
        //     ControllerRRotationDict.Add(id, controllerR.rotation);
        //     GazePointRelativePositionDict.Add(id, new Vector3());
        //     GazeObjectNameDict.Add(id, "Untagged");
        //     GazePointColor.Add(id, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)); // Pick a random, saturated and not-too-dark color
        //     // Bip01PositionDict.Add(id, bip01.position);
        //     // Bip01RotationDict.Add(id, bip01.rotation);
        // }




        /**
        Update the information of player in the network.
        **/
        void updateLocalPlayerInNetWork()
        {
            Ray eyeTrackingRay = PlayerInfos.getEyeTrackingRay();
            VRHelmetPosition.Value = vrHelmet.position;
            VRHelmetRotation.Value = vrHelmet.rotation;
            EyeTrackingRay.Value = eyeTrackingRay;
            ControllerLPosition.Value = controllerL.position;
            ControllerLRotation.Value = controllerL.rotation;
            ControllerRPosition.Value = controllerR.position;
            ControllerRRotation.Value = controllerR.rotation;
            GazePointRelativePosition.Value = new Vector3();
            GazeObjectName.Value = "Untagged";
            GazePointColor.Value = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            // if (Bip01PositionDict.ContainsKey(id))
            //     Bip01PositionDict[id] = bip01.position;
            // if (Bip01RotationDict.ContainsKey(id))
            //     Bip01RotationDict[id] = bip01.rotation;

            /* Eye Gaze*/
            RaycastHit hit;


            if (Physics.Raycast(eyeTrackingRay, out hit))
            {
                Transform tfHitObject = hit.transform;
                if (tfHitObject.GetComponent<Tag>())
                {

                    // GazeObjectTagDict[id] = tfHitObject.tag;
                    GazeObjectName.Value = tfHitObject.GetComponent<Tag>().objectName;
                    if (tfHitObject.gameObject.hasTag("GazePoint"))
                    {

                        Vector3 relativePosition = tfHitObject.InverseTransformPoint(hit.point); // World coordinate to local coordinate
                        GazePointRelativePosition.Value = relativePosition;
                    }
                    else
                    {
                        GazeObjectName.Value = "Untagged";
                    }
                }
                else
                {
                    GazeObjectName.Value = "Untagged";
                }
            }
            else
            {
                GazeObjectName.Value = "Untagged";
            }

            /* Eye Gaze*/
        }

        void updatePlayerBody()
        {
            if (IsLocalPlayer)
            {
                if (vrHelmet)
                {
                    fVRHelmet.position = vrHelmet.position;
                    fVRHelmet.rotation = vrHelmet.rotation; // VRIK internally performs IK calculations based on the body parts of the Avatar allocated to it, so there is no need to offset it.

                    // bip01.position = vrHelmet.position - offset_position_head_bip01;
                    // head.position = vrHelmet.position;
                    // head.rotation = vrHelmet.rotation * Quaternion.Euler(offset_rotation_head);
                }
                if (controllerL)
                {
                    if (controllerL.GetComponent<SteamVR_Behaviour_Pose>().isActive)
                    {
                        fControllerL.position = controllerL.position;
                        fControllerL.rotation = controllerL.rotation;
                        // ik.solvers.leftHand.IKPosition = ControllerL.position;
                        // ik.solvers.leftHand.IKRotation = ControllerL.rotation;
                        // handL.position = ControllerL.position;
                        // handL.rotation = ControllerL.rotation * Quaternion.Euler(offset_rotation_handL);
                    }
                    else
                    {
                        fControllerL.position = head.TransformPoint(relative_position_handL_head);
                        ControllerLPosition.Value = fControllerL.position;
                    }
                }
                else
                {
                    fControllerL.position = head.TransformPoint(relative_position_handL_head);
                    ControllerLPosition.Value = fControllerL.position;
                }
                if (controllerR)
                {
                    if (controllerR.GetComponent<SteamVR_Behaviour_Pose>().isActive)
                    {
                        fControllerR.position = controllerR.position;
                        fControllerR.rotation = controllerR.rotation;
                        // ik.solvers.rightHandEffector.position = ControllerR.position;
                        // ik.solvers.rightHandEffector.rotation = ControllerR.rotation;
                        // handR.position = ControllerR.position;
                        // handR.rotation = ControllerR.rotation * Quaternion.Euler(offset_rotation_handR);
                    }
                    else
                    {
                        fControllerR.position = head.TransformPoint(relative_position_handR_head);
                        ControllerRPosition.Value = fControllerR.position;
                    }
                }

                else
                {
                    fControllerR.position = head.TransformPoint(relative_position_handR_head);
                    ControllerRPosition.Value = fControllerR.position;
                }
            }
            else
            {
                string id = GetComponent<Tag>().id;
                fVRHelmet.position = VRHelmetPosition.Value;
                fVRHelmet.rotation = VRHelmetRotation.Value;
                fControllerL.position = ControllerLPosition.Value;
                fControllerL.rotation = ControllerLRotation.Value;
                fControllerR.position = ControllerRPosition.Value;
                fControllerR.rotation = ControllerRRotation.Value;
            }
        }

        // void updateHead()
        // {
        //     if (IsLocalPlayer)
        //     {

        //     }
        //     else
        //     {

        //     }
        // }


        // // Calculate the position of an object A relative to another object B (origin) at the same level
        // private Vector3 getRelativePosition(Transform origin, Vector3 position)
        // {
        //     Vector3 distance = position - origin.position;
        //     Vector3 relativePosition = Vector3.zero;
        //     relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        //     relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        //     relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);
        //     return relativePosition;
        // }

        /**
            Get the ray of the eye tracker
        **/

    }
}