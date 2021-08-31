using UnityEngine;
using System.Collections.Generic;
using MLAPI;
using Wolf3D.ReadyPlayerMe.AvatarSDK;
using RootMotion.FinalIK;
using Tags;
using VRComponent;
using MLAPI.Spawning;

namespace VRComponent
{


    public class ImportAvatar : NetworkBehaviour
    {
        bool hasImported = false;
        public RuntimeAnimatorController anim1;

        public string avatarUrl = "";
        string input;
        void Update()
        {
            if (!hasImported)
            {
                if (!IsLocalPlayer)
                {
                    var mlapiNetworkManager = GetComponent<NetworkVariableManager>();
                    avatarUrl = mlapiNetworkManager.AvatarURL.Value;
                    Debug.Log(avatarUrl);
                }
                if (!avatarUrl.Equals(""))
                {
                    importAvatar(avatarUrl);
                    hasImported = true;
                }
            }


        }



        void OnGUI()
        {
            if (IsLocalPlayer && !hasImported)
            {
                if (avatarUrl.Equals("") && IsClient)
                {
                    GUI.Label(new Rect(220, 10, 100, 20), "Avatar URL");
                    input = GUI.TextField(new Rect(320, 10, 280, 20), input);
                }
                if (GUI.Button(new Rect(610, 10, 60, 20), "Confirm"))
                {
                    avatarUrl = input;
                }
            }
        }

        public void importAvatar(string avatarUrl)
        {
            AvatarLoader avatarLoader = new AvatarLoader();
            avatarLoader.LoadAvatar(avatarUrl, OnAvatarLoaded);
        }

        public void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
        {
            string path_FControllerL = "Prefabs/VRIK/FControllerL";
            string path_FControllerR = "Prefabs/VRIK/FControllerR";
            string path_FVRHelmet = "Prefabs/VRIK/FVRHelmet";
            GameObject FControllerL = Instantiate(Resources.Load(path_FControllerL) as GameObject);
            GameObject FControllerR = Instantiate(Resources.Load(path_FControllerR) as GameObject);
            GameObject FVRHelmet = Instantiate(Resources.Load(path_FVRHelmet) as GameObject);

            FControllerL.name = FControllerL.name.Replace("(Clone)", "");
            FControllerR.name = FControllerR.name.Replace("(Clone)", "");
            FVRHelmet.name = FVRHelmet.name.Replace("(Clone)", "");

            FControllerL.transform.SetParent(avatar.transform);
            FControllerR.transform.SetParent(avatar.transform);
            FVRHelmet.transform.SetParent(avatar.transform);

            Transform refHead = FControllerL.transform.Find("Reference");
            Transform refHandL = FControllerR.transform.Find("Reference");
            Transform refHandR = FVRHelmet.transform.Find("Reference");

            Transform head = avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/Neck/Head");
            Transform handL = avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand");
            Transform handR = avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand");
            Transform armature = avatar.transform.Find("Armature/");

            Transform fHead = Instantiate(head.gameObject).transform;
            Transform fHandL = Instantiate(handL.gameObject).transform;
            Transform fHandR = Instantiate(handR.gameObject).transform;

            // fHead.position = refHead.position;
            // fHead.rotation = refHead.rotation;
            // fHandL.position = refHandL.position;
            // fHandL.rotation = refHandL.rotation;
            // fHandR.position = refHandR.position;
            // fHandR.rotation = refHandR.rotation;


            fHead.SetParent(FVRHelmet.transform);
            fHandL.SetParent(FControllerL.transform);
            fHandR.SetParent(FControllerR.transform);



            avatar.gameObject.GetComponent<Animator>().runtimeAnimatorController = anim1;
            avatar.AddComponent<VRIK>();

            avatar.GetComponent<VRIK>().solver.spine.headTarget = fHead;
            avatar.GetComponent<VRIK>().solver.leftArm.target = fHandL;
            avatar.GetComponent<VRIK>().solver.rightArm.target = fHandR;

            GameObject localPlayer = getLocalPlayer();
            avatar.transform.SetParent(localPlayer.transform);


            Debug.Log("Avatar Loaded!");
        }

        GameObject getLocalPlayer()
        {
            List<GameObject> list = TagManager.FindObjsWithTag("Player");
            ulong playerNetworkId = transform.GetComponent<NetworkObject>().NetworkObjectId;
            foreach (GameObject obj in list)
            {
                if (obj.GetComponent<NetworkObject>().NetworkObjectId == playerNetworkId)
                    return obj;
            }
            return null;
        }

    }


}