using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Html.Rt.Unit.TagElements
{
    [TestClass]
    public class ElementTest
    {
        public Seperator.ElementSeperator Seperator { get; } = new Seperator.ElementSeperator();
        
     
        [TestMethod]
        public void get_tagName_no_attributes()
        {
            const string testCode = "<div>";
            Assert.IsTrue(Seperator.CanParse(testCode));
            var elements = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1,elements.Length);
            var element = elements.First();
            Assert.IsInstanceOfType(element, typeof(IHtmlElement));
            var htmlElement = (IHtmlElement) element;
            Assert.AreEqual("div",htmlElement.Name);
        }

        [TestMethod]
        public void get_tagName_from_un_ended()
        {
            const string testCode = "<div ";
            Assert.IsTrue(Seperator.CanParse(testCode));
            var elements = Seperator.Parse(testCode).ToArray();
        }


        [TestMethod]
        public void get_tag_Name_with_attributes()
        {
            const string testCode = "<div name='hakan' value=\"12345\">";
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(IHtmlElement));
            var htmlElement = (IHtmlElement) element;
            Assert.AreEqual("div", htmlElement.Name);
        }


        [TestMethod]
        public void get_attributes_from_single_element()
        {
            const string testCode = "<div name='hakan' value=\"12345\">";
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(IHtmlElement));
            var htmlElement = (IHtmlElement) element;
            var attributes = htmlElement.Attributes.ToArray();
            Assert.AreEqual(2, attributes.Length);
            var attributeOne = attributes[0];
            var attributeTwo = attributes[1];
            Assert.AreEqual("name", attributeOne.Key);
            Assert.AreEqual("hakan", attributeOne.Value);
            Assert.AreEqual("value", attributeTwo.Key);
            Assert.AreEqual("12345", attributeTwo.Value);
        }

        [TestMethod]
        public void get_tag_name_from_single_tag()
        {
            const string testCode = "<br />";
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(IHtmlElement));
            var htmlElement = (IHtmlElement) element;
            Assert.AreEqual("br", htmlElement.Name);
        }

        [TestMethod]
        public void get_attributes_from_single_tag()
        {
            const string testCode = "<br name='key' value='41323' />";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1,result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(IHtmlElement));
            var htmlElement = (IHtmlElement) element;
            var attributes = htmlElement.Attributes.ToArray();
            var attributeOne = attributes[0];
            var attributeTwo = attributes[1];
            Assert.AreEqual("name", attributeOne.Key);
            Assert.AreEqual("key", attributeOne.Value);
            Assert.AreEqual("value", attributeTwo.Key);
            Assert.AreEqual("41323", attributeTwo.Value);
        }
        
        [TestMethod]
        public void get_end_tag()
        {
            const string testCode = "</div>";
            Assert.IsTrue(Seperator.CanParse(testCode),"Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(EndTag));
            var endTag = (EndTag) result[0];
            Assert.AreEqual("div", endTag.Name);
        }
        
  

 


    }
}