using System;
using System.Collections.Generic;
using Tobii.G2OM;
using UnityEngine;
using MLAPI.NetworkVariable.Collections;
using MLAPI;
using Tags;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

namespace VRComponent
{
    //Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
    public class HighlightAtGazeZJ : NetworkBehaviour, IGazeFocusable
    {
        public Color HighlightColor = Color.red;
        public float AnimationTime = 0.1f;
        private Renderer _renderer;
        private Color _originalColor;
        private Color _targetColor;

        //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
        public void GazeFocusChanged(bool hasFocus)
        {
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                _targetColor = HighlightColor;
            }
            //If this object lost focus, fade the object's color to it's original color
            else
            {
                _targetColor = _originalColor;
            }
        }

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _originalColor = _renderer.material.color;
            _targetColor = _originalColor;
        }

        private void Update()
        {
            updateAtGaze();
            //This lerp will fade the color of the object
            if (_renderer.material.HasProperty(Shader.PropertyToID("_BaseColor"))) // new rendering pipeline (lightweight, hd, universal...)
            {
                _renderer.material.SetColor("_BaseColor", Color.Lerp(_renderer.material.GetColor("_BaseColor"), _targetColor, Time.deltaTime * (1 / AnimationTime)));
            }
            else // old standard rendering pipline
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / AnimationTime));
            }
        }

        private void updateAtGaze()
        {

                // NetworkDictionary<long, Ray> dict = player.EyeTrackingRayDict;
                // printDict(dict);
                string playerTag = "Player";
                List<GameObject> players = TagManager.FindObjsWithTag(playerTag);
                if(players!=null)
                    foreach (GameObject player in players)
                    {
                        Ray ray  =  player.GetComponent<NetworkVariableManager>().EyeTrackingRay.Value;
                        RaycastHit hit;
                        bool isCollider = Physics.Raycast(ray, out hit);
                        if (isCollider && hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                        {
                            GazeFocusChanged(true);
                            return;
                        }
                    }
                GazeFocusChanged(false);
        }

        void printDict(NetworkDictionary<long, Ray> dict)
        {
            foreach (KeyValuePair<long, Ray> kvp in dict)
            {
                Ray ray = kvp.Value;
                if (ray.direction.x > 0.1 || ray.direction.x < -0.1)
                {
                    print("Network:" + ray);
                    print(kvp.Key + ":" + kvp.Value);
                }

            }
        }
    }
}
