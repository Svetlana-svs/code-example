using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace Wlds
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
        FFT = 1,
        FFT_S = 2
    }

    public enum MODE
    {
        BASE = 0,
        CIRCLE = 1,
        PAUSE = 2
    }

    public class Info
    {
        public Int16 NN { get; set; }
        public Int16 D { get; set; }
        public Int16 TT { get; set; }
    }

    public class Detector
    {
        public Int16 Id { get; set; }
        public String Number { get; set; }
        public Info Info { get; set; }
        public Int16 Order { get; set; }
        public List<String> Description { get; set; }
        public Dictionary<DATA_TYPE, float[]> data { get; set; }
        public List<Dictionary<DATA_TYPE, float[]>> dataMeasurement { get; set; }

        public Detector()
        { }
        
        public Detector(Int16 id, String number)
        {
            this.Id = id;
            this.Number = number;
            this.Description = new List<String>();

            this.Info = new Info();

            data = new Dictionary<DATA_TYPE, float[]>();
            dataMeasurement = new List<Dictionary<DATA_TYPE, float[]>>();

            foreach (DATA_TYPE type in Enum.GetValues(typeof(DATA_TYPE)))
            {
                data.Add(type, null);
            }
        }
    }

    public class Circle
    {
        // Array of indexes detectors whose measurements display in circle
        public static int[] detectorIndexes;
        // Current detector index
        public static int detectorIndex;

        // Current maesurement
        public static int measurementNumber = 0;
        // Number of measurements
        public static int measurementNumberCount = 0;
    }
}
