using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.NetworkVariable;
using Wolf3D.ReadyPlayerMe.AvatarSDK;
using RootMotion.FinalIK;
using System.Linq;
using MLAPI.Configuration;

public class EquipPlayer : NetworkBehaviour
{
    


    void Start()
    {
        
    }

    public override void NetworkStart()
    {
        if(IsLocalPlayer && IsClient)
        {
            AvatarLoader avatarLoader = new AvatarLoader();
            string avatarUrl = "https://d1a370nemizbjq.cloudfront.net/29d42edb-2705-4ee4-ab6b-ff079322e48b.glb";
	        avatarLoader.LoadAvatar(avatarUrl, OnAvatarLoaded);
        }
    }

    void Update()
    {

    }

    public void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        avatar.AddComponent<VRIK>();
	    Debug.Log("Avatar Loaded!");
    }



    // void equipPlayer(ulong playerNetID)
    // {
    //     Debug.Log("Start");
    //     string path = "";
    //     if(IsHost)
    //         path = "Prefabs/Avatar";
    //     else
    //         path = "Prefabs/Avatar2";
    //     GameObject avatar = (GameObject)Instantiate(Resources.Load(path));
    //     avatar.transform.position = Vector3.zero;
    //     avatar.transform.rotation = Quaternion.identity;
    //     if (!avatar.GetComponent<NetworkObject>())
    //         avatar.AddComponent<NetworkObject>();
    //     avatar.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerNetID);
    //     ulong avatarNetID = avatar.GetComponent<NetworkObject>().NetworkObjectId;
        
    // }
    
    [ServerRpc]
    void equipPlayerServerRpc(ulong playerNetID)
    {
        Debug.Log("Start");
        string path = "";
        if(IsHost)
            path = "Prefabs/Avatar";
        else
            path = "Prefabs/Avatar2";
        GameObject avatar_res = Resources.Load(path) as GameObject;
        
        if (!avatar_res.GetComponent<NetworkObject>())
            avatar_res.AddComponent<NetworkObject>();

        GameObject avatar = Instantiate(avatar_res);
        avatar.transform.position = Vector3.zero;
        avatar.transform.rotation = Quaternion.identity;
        avatar.GetComponent<NetworkObject>().SpawnWithOwnership(playerNetID);
        ulong avatarNetID = avatar.GetComponent<NetworkObject>().NetworkObjectId;
        equipPlayerClientRpc(avatarNetID);
        Debug.Log("End");
    }

    [ClientRpc]
    void equipPlayerClientRpc(ulong avatarId)
    {
        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[avatarId];
        GameObject avatar = netObj.gameObject;
        avatar.transform.SetParent(transform);
    }

    // public override void NetworkStart()
    // {
        // GameObject go = Instantiate(myPrefab, Vector3.zero, Quaternion.identity);
        // ulong id = NetworkManager.Singleton.LocalClientId;
        // go.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
        // Debug.Log("111");
        // NetworkPrefab netWorkPrefab = new NetworkPrefab();
        // netWorkPrefab.Prefab = myPrefab;
        // netWorkPrefab.PlayerPrefab = true;
        // NetworkManager.Singleton.NetworkConfig.NetworkPrefabs.Add(netWorkPrefab);
    // }

    // public void Update()
    // {
        // if (!hasPlayer)
        //     if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        //     {
        //         spawnPlayerObjectServerRpc();
        //         hasPlayer = true;
        //     }
    // }

    // [ServerRpc]
    // void spawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    // {

    //     GameObject myPrefab = Resources.Load("Prefabs/Player") as GameObject;
    //     // myPrefab.transform.position = Vector3.zero;
    //     // myPrefab.transform.rotation = Quaternion.identity;
    //     if (!myPrefab.GetComponent<NetworkObject>())
    //         myPrefab.AddComponent<NetworkObject>();
    //     GameObject go = Instantiate(myPrefab, Vector3.zero, Quaternion.identity);
    //     ulong id = NetworkManager.Singleton.LocalClientId;
    //     go.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
    // }
}
