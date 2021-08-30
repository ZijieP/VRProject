/// <summary>
/// This class is used to add, delete, modify, and check a single tag.
/// <summary>

using UnityEngine;
using System.Collections.Generic;
namespace Tags
{
    public static class Extensions
    {


        public static bool isID(this GameObject gameObject, string id)
        {
            if (gameObject.TryGetComponent<Tag>(out Tag t))
            {
                if (t.id.Equals(id))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }
        public static void addID(this GameObject gameObject, string id)
        {
            if (gameObject.TryGetComponent<Tag>(out Tag t))
            {
                if (!TagManager.idsDictionary.ContainsKey(id))
                {
                    TagManager.idsDictionary.Add(id, gameObject);
                }
                else
                {
                    Debug.Log("already have id " + id);
                }
            }
        }

        // if contain all tags it will return true
        public static bool hasTag(this GameObject gameObject, params string[] tag)
        {
            if (gameObject.TryGetComponent<Tag>(out Tag t))
            {
                for (int i = 0; i < tag.Length; i++)
                {
                    if (!t.tags.Contains(tag[i]))
                    {
                        // Debug.Log(gameObject.name + " doesn't have the tag " + tag);
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        // add tag
        public static void addTag(this GameObject gameObject, params string[] tags)
        {
            if (gameObject.TryGetComponent<Tag>(out Tag t))
            {
                foreach (var tag in tags)
                {
                    if (!TagManager.tagsDictionary.ContainsKey(tag))
                    {
                        // Debug.Log("Add a tag：" + tag);
                        TagManager.tagsDictionary.Add(tag, new List<GameObject>());
                        // Debug.Log(tag + " add an object " + gameObject.name);
                        TagManager.tagsDictionary[tag].Add(gameObject);
                    }
                    else
                    {
                        // Debug.Log(tag + " add an object " + gameObject.name);
                        TagManager.tagsDictionary[tag].Add(gameObject);
                    }
                }
            }
        }


        // remove tag
        public static void removeTag(this GameObject gameObject, params string[] tags)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                if (gameObject.hasTag(tags[i]))
                {
                    gameObject.GetComponent<Tag>().tags.Remove(tags[i]);
                    // Debug.Log(gameObject.name + " remove tag " + tags);
                    TagManager.tagsDictionary[tags[i]].Remove(gameObject);
                }
                else
                {
                    // Debug.LogWarning(gameObject.name + " doesn't exist the tag " + tags[i]);
                }
            }
        }
    }
}