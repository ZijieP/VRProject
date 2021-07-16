using System;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.Configuration;

public class NetworkPlayerSpawner : NetworkBehaviour
{

    bool hasPlayer = false;
    void Awake()
    {
        
    }
    
    void Update()
    {
        if(!hasPlayer && IsClient)
        {
            equipPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
            hasPlayer = true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
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
        avatar.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerNetID);
        ulong avatarNetID = avatar.GetComponent<NetworkObject>().NetworkObjectId;
        SpawnClientRpc(avatarNetID);
    }

    [ClientRpc]
     private void SpawnClientRpc(ulong objectId)
     {

         NetworkObject player = NetworkSpawnManager.SpawnedObjects[objectId];
         Instantiate(player.gameObject);
     }
}