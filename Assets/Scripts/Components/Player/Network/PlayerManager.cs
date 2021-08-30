/// <summary>
/// This component allows players to choose Client, Server or Host mode to enter the game in the GUI interface.
/// <summary>

using UnityEngine;
using MLAPI;

/**
    NetworkManager implements the singleton pattern as it declares its singleton named Singleton. 
    This is defined when the MonoBehaviour is enabled. 
    This component also contains very useful properties, such as IsClient, IsServer, and IsLocalClient. 
    The first two dictate the connection state we have currently established that you will use shortly.
    We will call these methods inside of OnGUI().
**/
public class PlayerManager : MonoBehaviour
{
    //
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

        }

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }


}