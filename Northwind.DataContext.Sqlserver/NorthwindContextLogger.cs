using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Context
{
    public class NorthwindContextLogger
    {
        public static void WriteLine(string message)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Northwind.txt");
            StreamWriter writer = File.AppendText(path);
            writer.WriteLine(message);
            writer.Close();
        }
    }
}
