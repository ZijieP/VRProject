/// <summary>
/// This class is used to store the player's attention information.
/// <summary>

using System.Collections.Generic;
using System;
namespace VRComponent
{
    public class FocusInfos
    {
        public long focus_start_time;
        public long focus_end_time;
        public double normalized_score ;
        public double maxScore;
        public double minScore;
        public Dictionary<string, double> dict_score = new Dictionary<string, double>();
        public static Dictionary<string, double> dict_score_weight = new Dictionary<string, double>();
        public Dictionary<string, double> Dict_score_weight { get => dict_score_weight; set => dict_score_weight = value; }
        public Dictionary<string, double> Dict_score { get => dict_score; set => dict_score = value; }
        // public string ObjID { get => objID; set => objID = value; }
        public long Focus_start_time { get => focus_start_time; set => focus_start_time = value; }
        public long Focus_end_time { get => focus_end_time; set => focus_end_time = value; }
        
        public double Normalized_score{ get => normalized_score; set => normalized_score = value; }
        public double MaxScore{ get => maxScore; set => maxScore = value; }
        public double MinScore{ get => minScore; set => minScore = value; }

        public FocusInfos(double normalized_score, double maxScore, double minScore)
        {
            // ObjID = objID;
            Focus_start_time = 0;
            Focus_end_time = 0;
            foreach(string method in dict_score_weight.Keys)
            {
                dict_score.Add(method, 0); 
            }
            Dict_score_weight = dict_score_weight;
            Normalized_score = normalized_score;
            MaxScore = maxScore;
            MinScore = minScore;
        }
        public double GetTotalScore()
        {
            double total = 0;
            foreach(string method in dict_score_weight.Keys)
            {
                total += dict_score[method] * dict_score_weight[method] * normalized_score / (maxScore - minScore);
            }

            return Math.Round(total,2);
        }
    }
}
