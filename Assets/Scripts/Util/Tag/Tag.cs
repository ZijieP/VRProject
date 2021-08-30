/// <summary>
/// This component is used to add multiple tags to the object.
/// <summary>

using UnityEngine;
using System.Collections.Generic;
using MLAPI;

// using Sirenix.OdinInspector;
namespace Tags
{
    public class Tag : NetworkBehaviour
    {
        [SerializeField]
        public string objectName;//one of the tags should be the same as transform's name and ObjectName of component Tag
        public string id;
        public List<string> tags;
        void Start()
        {
            if(gameObject.GetComponent<NetworkObject>())
            {
                id = objectName + gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            }
            if(!tags.Contains(objectName))
            {
                tags.Add(objectName);
            }
            gameObject.addTag(tags.ToArray());
            gameObject.addID(id);
        }
        void Update()
        {
            if(gameObject.GetComponent<NetworkObject>())
            {
                id = objectName + gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                if(gameObject.GetComponent<Tag>())
                {
                    if(!gameObject.isID(id))
                    {
                        gameObject.addID(id);
                    }
                }
            }
        }
    }
    
}