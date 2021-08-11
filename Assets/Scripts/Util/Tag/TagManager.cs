using UnityEngine;
using System.Collections.Generic;
using System;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;


namespace Tags
{
    public class TagManager : MonoBehaviour
    {
        // public static TagManager Instance { get; private set; }
        private void Awake()
        {
            // if (Instance == null)
            // {
            //     Instance = (TagManager)this;
            //     DontDestroyOnLoad(gameObject);
            // }
            // else
            // {
            //     Destroy(gameObject);
            // }
        }
        // public static List<string> tagsList = new List<string>();
        public static Dictionary<string, List<GameObject>> tagsDictionary = new Dictionary<string, List<GameObject>>();
        public static Dictionary<string, GameObject> idsDictionary = new Dictionary<string, GameObject>();

        // private static void InitTags()
        // {
        //     int length = tagsList.Count;
        //     tagsDictionary = new Dictionary<string, List<GameObject>>(length);
        //     for (int i = 0; i < length; i++)
        //     {
        //         tagsDictionary.Add(tagsList[i], new List<GameObject>());
        //     }
        // }
        public static bool hasID(string id)
        {
            return idsDictionary.ContainsKey(id);
        }

        public static List<GameObject> FindObjsWithTag(string tag)
        {
            if (tagsDictionary.ContainsKey(tag))
            {
                return tagsDictionary[tag];
            }
            // Debug.Log("It doesn't exist the object whose tag is " + tag);
            return null;
        }



        public static GameObject FindObjByID(string id)
        {
            if (idsDictionary.ContainsKey(id))
            {
                return idsDictionary[id];
            }
            // Debug.Log("It doesn't exist the object whose tag is " + id);
            return null;
        }

        // public static void removeObjsNotContainsTag(this List<GameObject> list, string subTag)
        // {
        //     foreach(GameObject obj in list)
        //     {
        //         if(!obj.tag.Contains(subTag))
        //         {
        //             list.Remove(obj);
        //         }
        //     }
        // }
        
        // public static bool tagListContains(string tag)
        // {
        //     foreach (string key in tagsDictionary.Keys)
        //     {
        //         if(key.Equals(tag))
        //             return true;
        //     }
        //     return false;
        // }
    }
}