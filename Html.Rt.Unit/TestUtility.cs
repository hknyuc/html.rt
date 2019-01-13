using System;
using System.IO;

namespace Html.Rt.Unit
{
    public static class TestUtility
    {
        public static string GetFile(string path)
        {
            string r = Path.Combine(@"C:\Users\Pc-Arete\Documents\Github\html.rt\Html.Rt.Unit\", @"Files\" + path);
            return System.IO.File.ReadAllText(r);
            
        }

        public static void Times(this int value,Action<int> action)
        {
            for (var i = 0; i < value; i++)
                action(i);
        }

        public static void Times(this int value, Action action)
        {
            for (var i = 0; i < value; i++)
                action();
        }
        
    }
}