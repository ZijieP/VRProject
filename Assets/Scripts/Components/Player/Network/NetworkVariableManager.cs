/// <summary>
/// This is a composant which synchronizes the information of each user in the network.
/// <summary>

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
        public NetworkVariableString AvatarURL = new NetworkVariableString(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        
        // This attribute store the color of the user's gaze point.
        public NetworkVariableColor GazePointColor = new NetworkVariableColor(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.OwnerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the position of the VR helmet.
        public NetworkVariableVector3 VRHelmetPosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the rotation of the VR helmet.
        public NetworkVariableQuaternion VRHelmetRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the position of the left controller of the VR.
        public NetworkVariableVector3 ControllerLPosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the rotation of the left controller of the VR.
        public NetworkVariableQuaternion ControllerLRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the position of the right controller of the VR.
        public NetworkVariableVector3 ControllerRPosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the rotation of the right controller of the VR.
        public NetworkVariableQuaternion ControllerRRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the ray of the user's eye gaze.
        public NetworkVariableRay EyeTrackingRay = new NetworkVariableRay(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the coordinates of the gaze point relative to the object the user is gazing at.
        public NetworkVariableVector3 GazePointRelativePosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });
        // This attribute stores the object name in the Tag component of the object the user is gazing at.
        public NetworkVariableString GazeObjectName = new NetworkVariableString(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });



        // The transform information of each GameObject.
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


        // The coordinates of the left hand relative to the head
        Vector3 relative_position_handL_head;
        // The coordinates of the right hand relative to the head
        Vector3 relative_position_handR_head;

        // After entering the network (after selecting one of the Host, Client, and Server modes), he will execute it.
        public override void NetworkStart()
        {
            initBodyTF();
            if (IsLocalPlayer)
            {
                initAttribute();
                initVRDevice();
            }
        }

        // Initialize two relative coordinates
        private void initAttribute()
        {
            relative_position_handL_head = head.InverseTransformPoint(handL.position);
            relative_position_handR_head = head.InverseTransformPoint(handR.position);

        }

        // The transform information of each GameObject.
        private void initBodyTF()
        {
            head = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
            handL = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
            handR = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
            bip01 = transform.Find("Bip01");
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

        // Initialize the VR device information of the local player.
        void initVRDevice()
        {
            System.Random ran = new System.Random();
            vrDevice = GameObject.Find("VRDevice").transform;
            vrHelmet = GameObject.Find("VRHelmet").transform;
            floor = GameObject.Find("Plane").transform;
            vrDevice.position = new Vector3(0, floor.localScale.y/2, 0);
            controllerL = GameObject.Find("ControllerL").transform;
            controllerR = GameObject.Find("ControllerR").transform;
        }

        
        // Update the information of local player to the network.
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

            if(GetComponent<ImportAvatar>())
            {
                var ia = GetComponent<ImportAvatar>();
                AvatarURL.Value = ia.avatarUrl;
            }

            // Gaze point information update
            RaycastHit hit;
            if (Physics.Raycast(eyeTrackingRay, out hit))
            {
                Transform tfHitObject = hit.transform;
                if (tfHitObject.GetComponent<Tag>())
                {
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
        }

        // Update information for VRIK
        void updatePlayerBody()
        {
            if (IsLocalPlayer)
            {
                if (vrHelmet)
                {
                    fVRHelmet.position = vrHelmet.position;
                    fVRHelmet.rotation = vrHelmet.rotation; // VRIK internally performs IK calculations based on the body parts of the Avatar allocated to it, so there is no need to offset it.
                }
                if (controllerL)
                {
                    if (controllerL.GetComponent<SteamVR_Behaviour_Pose>().isActive)
                    {
                        fControllerL.position = controllerL.position;
                        fControllerL.rotation = controllerL.rotation;
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
    }
}