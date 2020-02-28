using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace SM3G
{
    public enum HRESULT
    {
        S_OK = 0,
        S_ERROR = -1,
        S_CANCEL = 2
    }

    public enum DATA_TYPE
    {
        BASE = 0,
        T = 1
    }

    public class Settings
    {
        public Int16 NN { get; set; }
        public int D { get; set; }
        public int TT { get; set; }
    }

    public class Detector
    {
        public Int16 Id { get; set; }
        public String Name { get; set; }
        public List<String> Description { get; set; }
        public Dictionary<DATA_TYPE, float[]>Data { get; set; }
        public List<Measure> DataMeasurement { get; set; }

        public Detector()
        { }
        
        public Detector(Int16 id, String name)
        {
            this.Id = id;
            this.Name = name;
            this.Description = new List<String>();

            this.Data = new Dictionary<DATA_TYPE,float[]>();
            this.DataMeasurement = new List<Measure>();

            foreach (DATA_TYPE type in Enum.GetValues(typeof(DATA_TYPE)))
            {
                this.Data.Add(type, null);
            }
        }
    }

    public static class Threshold
    {
        // Speed-up A thresholds
        public static float[] Thresholds = new float[4];
    }
}
