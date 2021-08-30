/// <summary>
/// This component will update the player's attention score information.
/// <summary>

using UnityEngine;
using System.Collections.Generic;
using System;
using Tags;
using MLAPI;
using PlayerData;
using System.Linq;

namespace VRComponent
{
    public class Focus : NetworkBehaviour
    {
        public Dictionary<string, double> dict_score_weight = new Dictionary<string, double>();
        public Dictionary<string, FocusInfos> dict_objID_focusInfos = new Dictionary<string, FocusInfos>();
        string playerID;
        string id_obj_gaze_now = null;
        string id_obj_gaze_just_now = null;
        long now_focus_start_time; // second through DateTime.Now.Ticks
        bool has_saved_now_focus = false;
        double maxScore;
        double minScore;

        string method_Distance = "Distance";
        string method_FocusDuration = "FocusDuration";
        string method_NowGaze = "NowGaze";
        string method_OtherPlayerGaze = "OtherPlayerGaze";

        public long time_GUI = 0;


        void Start()
        {
            initAttributes();
        }

        void initAttributes()
        {
            double weightDistance = 0.1f;
            double weightFocusDuration = 0.5f;
            double weightNowGaze = 0.25f;
            double weightOtherGaze = 0.15f;
            dict_score_weight.Add(method_Distance, weightDistance);
            dict_score_weight.Add(method_FocusDuration, weightFocusDuration);
            dict_score_weight.Add(method_NowGaze, weightNowGaze);
            dict_score_weight.Add(method_OtherPlayerGaze, weightOtherGaze);
            maxScore = 100;
            minScore = 0;
        }

        void distanceScore(double c, double maxDistance)
        {
            double a = Math.Pow(maxScore - minScore + 1, 1.0 / (c * maxDistance));
            foreach (string objID in dict_objID_focusInfos.Keys)
            {
                GameObject obj = TagManager.FindObjByID(objID);
                double dist = Vector3.Distance(transform.position, obj.transform.position);
                double score;
                if (dist >= maxDistance)
                {
                    score = minScore;
                }
                else
                {
                    score = maxScore - (Math.Pow(a, c * dist) - 1);
                }
                dict_objID_focusInfos[objID].Dict_score[method_Distance] = score;

            }
        }
        void focusDurationScore(double c_gazed, double c_not_gazed, double maxGazedTime, double maxNotGazedTime, double error)
        {
            long now = DateTime.Now.Ticks;
            double a_gazed = Math.Pow(maxScore + 1, 1.0 / (c_gazed * maxGazedTime));
            double a_not_gazed = Math.Pow(maxScore - minScore + 1, 1.0 / (c_not_gazed * maxNotGazedTime));
            foreach (string objID in dict_objID_focusInfos.Keys)
            {
                double score;
                if (dict_objID_focusInfos[objID].Focus_start_time != 0)
                {
                    if (dict_objID_focusInfos[objID].Focus_end_time == 0) // The object is gazed now
                    {
                        long t = now - dict_objID_focusInfos[objID].Focus_start_time;
                        score = minScore + Math.Pow(a_gazed, (c_gazed * t)) - 1;
                    }
                    else // The object is not gazed now
                    {
                        long t = now - dict_objID_focusInfos[objID].Focus_end_time;
                        if (t < error)
                        {
                            t = now - dict_objID_focusInfos[objID].Focus_start_time;
                            score = minScore + Math.Pow(a_gazed, (c_gazed * t)) - 1;
                        }
                        else
                        {
                            score = maxScore - (Math.Pow(a_not_gazed, (c_not_gazed * t)) - 1);
                        }
                    }
                }
                else
                {
                    score = minScore;
                }
                if (score < minScore)
                {
                    score = minScore;
                }
                if (score > maxScore)
                {
                    score = maxScore;
                }
                dict_objID_focusInfos[objID].Dict_score[method_FocusDuration] = score;

            }
        }
        void nowGazeScore(double error)
        {
            long now = DateTime.Now.Ticks;
            var mlapiNetworkManager = GetComponent<NetworkVariableManager>();
            GameObject gazedObject = PlayerInfos.getGazedObject(mlapiNetworkManager.EyeTrackingRay.Value);
            string objID = null;
            if (gazedObject)
            {
                if (gazedObject.TryGetComponent<Tag>(out Tag t))
                {
                    objID = t.id;
                }
                foreach (string id in dict_objID_focusInfos.Keys)
                {
                    if (id == objID)
                        dict_objID_focusInfos[id].Dict_score[method_NowGaze] = maxScore;
                    else
                    {
                        if (now - dict_objID_focusInfos[id].Focus_end_time > error)
                        {
                            dict_objID_focusInfos[id].Dict_score[method_NowGaze] = minScore;
                        }
                    }
                }
            }

        }
        void otherPlayerGazeScore()
        {
            string expectedObjName = "Player";
            var mlapiNetworkManager = GetComponent<NetworkVariableManager>();
            GameObject gazedObj = PlayerInfos.getGazedObject(mlapiNetworkManager.EyeTrackingRay.Value);
            string gazedObjName = "";
            string gazedObjID = null;
            if (gazedObj)
            {
                if (gazedObj.TryGetComponent<Tag>(out Tag t))
                {
                    gazedObjID = t.id;
                    gazedObjName = t.objectName;
                }
            }

            string otherGazedObjID = null;
            if (gazedObjName.Equals(expectedObjName))
            {
                var otherMLAPINetworkManager = gazedObj.GetComponent<NetworkVariableManager>();
                GameObject otherGazedObj = PlayerInfos.getGazedObject(otherMLAPINetworkManager.EyeTrackingRay.Value);
                if (otherGazedObj.TryGetComponent<Tag>(out Tag t1))
                {
                    otherGazedObjID = t1.id;

                }
            }
            foreach (string id in dict_objID_focusInfos.Keys)
            {
                if (id == otherGazedObjID)
                    dict_objID_focusInfos[id].Dict_score[method_OtherPlayerGaze] = maxScore;
                else
                {
                    dict_objID_focusInfos[id].Dict_score[method_OtherPlayerGaze] = minScore;
                }
            }
        }
        void Update()
        {
            updateDictOF();
            updateFocusInfos();
            updateFocusScore();
        }

        void updateFocusScore()
        {
            double c_distance = 1000;
            double maxDistance = 8;
            distanceScore(c_distance, maxDistance);
            double c_gazed = 1;
            double maxGazedTime = secTO100ns(5);
            double c_not_gazed = 1;
            double maxNotGazedTime = secTO100ns(10);
            double error = secTO100ns(0.5);
            focusDurationScore(c_gazed, c_not_gazed, maxGazedTime, maxNotGazedTime, error);
            nowGazeScore(error);
            otherPlayerGazeScore();
        }
        void updateDictOF()
        {
            // Check if all the Focus objs are in the dict
            FocusInfos.dict_score_weight = dict_score_weight;
            List<GameObject> list = TagManager.FindObjsWithTag("Focus");
            foreach (GameObject obj in list)
            {
                if (!dict_objID_focusInfos.ContainsKey(obj.GetComponent<Tag>().id))
                {
                    double normalized_score = 100;
                    dict_objID_focusInfos.Add(obj.GetComponent<Tag>().id, new FocusInfos(normalized_score, maxScore, minScore));
                }
            }
        }
        void OnGUI()
        {
            if (IsLocalPlayer)
            {

                GUILayout.BeginArea(new Rect(500, 10, 100, 100));
                int max = 3;
                int i = 0;
                var new_dict = from pair in dict_objID_focusInfos
                               orderby pair.Value.GetTotalScore() descending
                               select pair;
                foreach (KeyValuePair<string, FocusInfos> kvp in new_dict)
                {
                    if (i < max)
                    {
                        GUILayout.Label(kvp.Key + ": " + kvp.Value.GetTotalScore());

                    }
                    i++;
                }
                GUILayout.EndArea();
            }
        }
        void updateFocusInfos()
        {
            playerID = gameObject.GetComponent<Tag>().id;
            long now = DateTime.Now.Ticks;
            var mlapiNetworkManager = GetComponent<NetworkVariableManager>();
            GameObject objGazed = PlayerInfos.getGazedObject(mlapiNetworkManager.EyeTrackingRay.Value);
            string objGazedID = null;
            if (objGazed)
            {
                if (objGazed.hasTag("Focus"))
                {
                    objGazedID = objGazed.GetComponent<Tag>().id;
                    id_obj_gaze_now = objGazedID;
                }
                else
                {
                    id_obj_gaze_now = null;
                }
            }
            else
            {
                id_obj_gaze_now = null;
            }

            if (id_obj_gaze_now != null)
            {
                if (id_obj_gaze_now == id_obj_gaze_just_now)
                {
                    if (has_saved_now_focus == false)
                    {
                        if (dict_objID_focusInfos[objGazedID].Dict_score[method_FocusDuration] > 0)
                        {
                            dict_objID_focusInfos[objGazedID].Focus_end_time = 0;
                        }
                        else
                        {
                            dict_objID_focusInfos[objGazedID].Focus_start_time = now_focus_start_time;
                            dict_objID_focusInfos[objGazedID].Focus_end_time = 0;
                        }

                        has_saved_now_focus = true;
                    }
                }
                else
                {
                    now_focus_start_time = now;
                    if (id_obj_gaze_just_now != null)
                        dict_objID_focusInfos[id_obj_gaze_just_now].Focus_end_time = now;
                    id_obj_gaze_just_now = id_obj_gaze_now;
                    has_saved_now_focus = false;
                }
            }
            else
            {
                if (id_obj_gaze_just_now != null)
                {
                    dict_objID_focusInfos[id_obj_gaze_just_now].Focus_end_time = now;
                    id_obj_gaze_just_now = id_obj_gaze_now;
                    id_obj_gaze_now = objGazedID;
                    has_saved_now_focus = false;
                }

            }
        }

        double secTO100ns(double seconds)
        {
            return seconds * 10000000;
        }
    }
}