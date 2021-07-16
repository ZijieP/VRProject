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
        // public override void NetworkStart()
        void Start()
        {
            // objectName = transform.name;
            if(gameObject.GetComponent<NetworkObject>())
            {
                id = objectName + gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            }
            gameObject.addTag(tags.ToArray());
            gameObject.addID(id);


            // bool idHasCreated = false;
            // while(!idHasCreated)
            // {
            //     System.Random rand = new System.Random();
            //     int temp = rand.Next();
            //     if(!TagManager.idsDictionary.ContainsKey(temp.ToString()))
            //     {
            //         id = objectName + temp.ToString();
            //         idHasCreated = true;
            //     }
            // }
            // gameObject.AddID(id);
            
            
            // gameObject.HasTag("Player");
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
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     gameObject.RemoveTag("Player", "tag1");
            // }
            // foreach (KeyValuePair<string, List<GameObject>> pair in TagManager.tagsDictionary)
            // {
            //     if(pair.Key.Equals("Player"))
            //         Debug.Log("Key: "+ pair.Key +" num: " + pair.Value.Count);
            //     else if(pair.Key.Equals("Focus"))
            //         Debug.Log("Key: "+ pair.Key +" num: " + pair.Value.Count);
            //     else if(pair.Key.Equals("Laptop"))
            //         Debug.Log("Key: "+ pair.Key +" num: " + pair.Value.Count);
            // }
        }
    }
    
}