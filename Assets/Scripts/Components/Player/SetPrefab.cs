using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using System.Linq;
using MLAPI.Configuration;

public class SetPrefab : NetworkBehaviour
{
    
    void Awake()
    {
        
        GameObject myPrefab = Resources.Load("Prefabs/Player") as GameObject;
        myPrefab.transform.position = Vector3.zero;
        myPrefab.transform.rotation = Quaternion.identity;
        myPrefab.AddComponent<NetworkObject>();
        NetworkPrefab netWorkPrefab = new NetworkPrefab();
        netWorkPrefab.Prefab = myPrefab;
        netWorkPrefab.PlayerPrefab = true;
        NetworkManager.Singleton.NetworkConfig.NetworkPrefabs.Add(netWorkPrefab);

        
    }
}
