using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassiopeia
{
    public static class Utils
    {
        static public String ConvertToRomanNumeral(int arabicNumeral)
        {
            String romanNumeral = "";
            int x = arabicNumeral;

            switch (x / 10)
            {
                case 1:
                    romanNumeral += ("X");
                    x -= 10;
                    break;
                case 2:
                    romanNumeral += ("XX");
                    x -= 20;
                    break;
                case 3:
                    romanNumeral += ("XXX");
                    x -= 30;
                    break;
                case 4:
                    romanNumeral += ("XL");
                    x -= 40;
                    break;
                case 5:
                    romanNumeral += ("L");
                    x -= 50;
                    break;
                case 6:
                    romanNumeral += ("LX");
                    x -= 60;
                    break;
                case 7:
                    romanNumeral += ("LXX");
                    x -= 70;
                    break;
                case 8:
                    romanNumeral += ("LXXX");
                    x -= 80;
                    break;
                case 9:
                    romanNumeral += ("XC");
                    x -= 90;
                    break;
                default:
                    break;
            }

            switch (x)
            {
                case 1:
                    romanNumeral += ("I");
                    x -= 10;
                    break;
                case 2:
                    romanNumeral += ("II");
                    x -= 20;
                    break;
                case 3:
                    romanNumeral += ("III");
                    x -= 30;
                    break;
                case 4:
                    romanNumeral += ("IV");
                    x -= 40;
                    break;
                case 5:
                    romanNumeral += ("V");
                    x -= 50;
                    break;
                case 6:
                    romanNumeral += ("VI");
                    x -= 60;
                    break;
                case 7:
                    romanNumeral += ("VII");
                    x -= 70;
                    break;
                case 8:
                    romanNumeral += ("VIII");
                    x -= 80;
                    break;
                case 9:
                    romanNumeral += ("IX");
                    x -= 90;
                    break;
                default:
                    break;
            }

            return romanNumeral;
        }
    }
}
