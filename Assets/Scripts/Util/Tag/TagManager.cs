/// <summary>
/// This class is used to manage all Tag components. And we can query multiple objects with a certain Tag through it.
/// <summary>

using UnityEngine;
using System.Collections.Generic;



namespace Tags
{
    public class TagManager : MonoBehaviour
    {

        public static Dictionary<string, List<GameObject>> tagsDictionary = new Dictionary<string, List<GameObject>>();
        public static Dictionary<string, GameObject> idsDictionary = new Dictionary<string, GameObject>();

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
            return null;
        }

        public static GameObject FindObjByID(string id)
        {
            if (idsDictionary.ContainsKey(id))
            {
                return idsDictionary[id];
            }
            return null;
        }
    }
}