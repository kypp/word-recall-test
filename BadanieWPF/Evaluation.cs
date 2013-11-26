using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controls;

namespace BadanieWPF
{
    public class Evaluation
    {
        public WyborBadaniaControl.TypBadania typBadania;
        public TimeSpan czasWpisywania;
        private DateTime timeStarted;
        public bool IsStarted { get; set; }
        public float CorrectFraction { get; set; }

        private DateTime start;

        public void StartTimeEval()
        {
            if (!IsStarted)
            {
                start = DateTime.Now;
                IsStarted = true;
            }
        }

        public void EndTimeEval()
        {
            czasWpisywania = DateTime.Now - start;
            IsStarted = false;
        }

        public Evaluation()
        {
            timeStarted = DateTime.Now;
        }

        public void SaveToFile(string name)
        {
            string type = typBadania == 0 ? "K" : ((int)typBadania).ToString();
            string fname = name + "_" + type + "_" + timeStarted.ToString().Replace(" ","-").Replace(":","-") + ".txt";
            var linie = new string[4];
            linie[0] = "grupa " + type;
            linie[1] = czasWpisywania.ToString();
            linie[2] = Math.Round(CorrectFraction*100, 2).ToString() + "%";
            linie[3] = timeStarted.ToString();
            File.WriteAllLines(fname, linie);
        }

    }
}
