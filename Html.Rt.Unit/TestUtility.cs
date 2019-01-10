using System.IO;

namespace Html.Rt.Unit
{
    public class TestUtility
    {
        public static string GetFile(string path)
        {
            string r = Path.Combine(@"C:\Users\Hakanyu\Documents\Github\html.rt\Html.Rt.Unit\", @"Files\" + path);
            return System.IO.File.ReadAllText(r);
        }
    }
}