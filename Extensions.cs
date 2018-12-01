using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace PathCleaner
{
    static class Extensions
    {
        public static Path ToPath(this string rawText)
        {
            var k = string.IsNullOrEmpty(rawText) ? new List<string>() : rawText.Split(';').ToList();


            return new Path(k);
        }

        public static string GetPathVariable()
        {
            return Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
        }

        public static void UpdatePathVariable(this Path path)
        {
            var s = string.Join(";", path.Folders);

            Environment.SetEnvironmentVariable("PATH", s, EnvironmentVariableTarget.Machine);
        }

        public static List<string> ToList(this SelectedListViewItemCollection collection)
        {
            var k = new List<string>();

            foreach (ListViewItem item in collection)
            {
                k.Add(item.Text);
            }

            return k;
        }


        
        
    }
}
