using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Html.Rt.Unit.TagElements
{
    [TestClass]
    public class PageTest
    {
        [TestMethod]
        public void tdk()
        {
            var file = ReadFile("tdk.html");
            var document = NewDocument(file).OfType<Text>().ToArray();
        }
        
        [TestMethod]
        public void tdk2()
        {
            var file = ReadFile("tdk3.html");
            var document = NewDocument(file).ToArray();
            var table = document.GetElementById("hor-minimalist-a");
            var elements = table.Elements.OfType<Text>().ToArray();

        }


        private static string ReadFile(string path)
        {
            string r = Path.Combine(@"C:\Users\Pc-Arete\Documents\github\html.rt\Html.Rt.Unit", @"Files\" + path);
            return System.IO.File.ReadAllText(r);
        }

        private Document NewDocument(string content)
        {
            return new Document(content);
        }
    }
}