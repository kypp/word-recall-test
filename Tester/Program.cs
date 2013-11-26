using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadanieWPF;
using Controls;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Evaluation ev = new Evaluation();
            ev.StartTimeEval();
            for (int i = 0; i < 1000000; i++)
            {
                if (i == 37) i = 38;
            }
            ev.EndTimeEval();
            ev.CorrectFraction = .45846841f;
            ev.typBadania = WyborBadaniaControl.TypBadania.Badanie10;
            ev.SaveToFile("raport");
        }
    }
}
