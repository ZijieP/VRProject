using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using System.Linq;
using MLAPI.Configuration;

public class EquipPlayer : NetworkBehaviour
{



    void Start()
    {
        
    }

    public override void NetworkStart()
    {
        if(IsLocalPlayer)
        {
            equipPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        // Debug.Log("111");
        // equipPlayer(NetworkManager.Singleton.LocalClientId);
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
        GameObject avatar = (GameObject)Instantiate(Resources.Load(path));
        avatar.transform.position = Vector3.zero;
        avatar.transform.rotation = Quaternion.identity;
        if (!avatar.GetComponent<NetworkObject>())
            avatar.AddComponent<NetworkObject>();
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
