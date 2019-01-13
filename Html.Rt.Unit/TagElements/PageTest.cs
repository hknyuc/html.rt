using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Html.Rt.Seperator;
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
            var document = NewDocument(file).ToArray();
            Assert.AreEqual(732, document.Length);
        }
        
        [TestMethod]
        
        public void tdk2()
        {
            var file = ReadFile("tdk3.html");
            var document = NewDocument(file).ToArray();
            Assert.AreEqual(1578, document.Length);
            //           var table = document.GetElementById("hor-minimalist-a");
            //           var elements = table.Elements.OfType<Text>().ToArray();

        }

        [TestMethod]
        public void twitter()
        {
            var file = ReadFile("twitter.html");
            var htmContent = new HtmlContent(file);
            var document = NewDocument(htmContent).ToArray();
            Assert.AreEqual(1445, document.Length);

        }

        [TestMethod]
        public void facebook()
        {
            var file = ReadFile("facebook.html");
            var html = new HtmlContent(file);
            var document = NewDocument(html).ToArray();
            Assert.AreEqual(131, document.Length);
        }


        private static string ReadFile(string path)
        {
            return TestUtility.GetFile(path);
        }

        private Document NewDocument(string content)
        {
            return new Document(new HtmlContent(content));
        }

        private Document NewDocument(IHtmlContent content)
        {
            return new Document(content);
        }
    }
}