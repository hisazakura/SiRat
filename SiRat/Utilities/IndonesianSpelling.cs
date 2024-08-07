using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Utilities
{
    // PORTED FROM MY OLD JAVA PROJECT
    public class IndonesianSpelling
    {
        private int number;

        public IndonesianSpelling(int number)
        {
            this.number = number;
        }

        public IndonesianSpelling(string number)
        {
            this.number = int.Parse(number);
        }

        private static string basicNumber(int i)
        {
            if (i == 1) return "SATU";
            if (i == 2) return "DUA";
            if (i == 3) return "TIGA";
            if (i == 4) return "EMPAT";
            if (i == 5) return "LIMA";
            if (i == 6) return "ENAM";
            if (i == 7) return "TUJUH";
            if (i == 8) return "DELAPAN";
            if (i == 9) return "SEMBILAN";
            return "";
        }
        public override string ToString()
        {
            if (this.number == 0)
                return "NOL";
            string str = "";
            int index = 0;
            while (number > 0)
            {
                string currentString = "";
                int currentNumber = number % 1000;

                if (currentNumber / 100 != 0)
                {
                    if (currentNumber / 100 == 1)
                        currentString += "SERATUS ";
                    else
                        currentString += basicNumber(currentNumber / 100) + " RATUS ";
                }

                if (currentNumber % 100 / 10 == 1)
                {
                    if (currentNumber % 10 == 1)
                        currentString += "SEBELAS";
                    else if (currentNumber % 10 == 0)
                        currentString += "SEPULUH";
                    else
                        currentString += basicNumber(currentNumber % 10) + " BELAS";
                }
                else if (currentNumber % 100 / 10 == 0)
                {
                    if (currentNumber == 1 && index == 1)
                        currentString += "SERIBU ";
                    else
                        currentString += basicNumber(currentNumber % 10);
                }
                else
                    currentString += basicNumber(currentNumber % 100 / 10) + " PULUH " + basicNumber(currentNumber % 10);

                if (currentNumber > 1 && index == 1)
                    currentString += " RIBU ";
                if (index == 2)
                    currentString += " JUTA ";
                if (index == 3)
                    currentString += " MILIAR ";

                str = currentString + str;
                number /= 1000;
                index += 1;
            }

            str = str.Replace("\\s+", " ");

            if (str.Length > 0)
                while (str[str.Length - 1] == ' ')
                    str = str.Substring(0, str.Length - 1);

            return str;
        }
    }
}
