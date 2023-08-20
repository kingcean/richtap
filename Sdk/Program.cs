using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Trivial.CommandLine;
using Trivial.Text;

namespace RichTap;

class Program
{
    static void Main(string[] args)
    {
        var verb = new VibrationCommandVerb();
        verb.Process();
    }
}
