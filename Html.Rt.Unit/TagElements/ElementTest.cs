using System.Linq;
using Html.Rt.Seperator;
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
            Assert.IsInstanceOfType(element, typeof(ITag));
            var htmlElement = (ITag) element;
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
        public void get_tag_none_closed()
        {
            var testCode = new HtmlContent("<div name='hakan'");
            Assert.IsTrue(Iterate(Seperator).CanParse(testCode));
            var result = Iterate(Seperator).Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            var tagElement = (ITag) result[0];
            Assert.AreEqual("div", tagElement.Name);
            Assert.AreEqual(1, tagElement.Attributes.Count());
            var attribute =  (IAttribute)tagElement.Attributes.ToArray()[0];
            Assert.AreEqual("name", attribute.Key);
            
            
            

        }


        [TestMethod]
        public void get_tag_Name_with_attributes()
        {
            var testCode = new HtmlContent("<div name='hakan' value=\"12345\">");
            Assert.IsTrue(Iterate(Seperator).CanParse(testCode));
            var result = Iterate(Seperator).Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(ITag));
            var htmlElement = (ITag) element;
            Assert.AreEqual("div", htmlElement.Name);
        }

        private static IHtmlSeperator Iterate(IHtmlSeperator seperator)
        {
            return new SeperatorIterator(seperator);
        }


        [TestMethod]
        public void get_attributes_from_single_element()
        {
            var testCode = new HtmlContent("<div name='hakan' value=\"12345\">");
            Assert.IsTrue(Iterate(Seperator).CanParse(testCode));
            var result = Iterate(Seperator).Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(ITag));
            var htmlElement = (ITag) element;
            var attributes = htmlElement.Attributes.ToArray();
            Assert.AreEqual(2, attributes.Length,htmlElement.Attributes.ToHtml());
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
            Assert.IsInstanceOfType(element, typeof(ITag));
            var htmlElement = (ITag) element;
            Assert.AreEqual("br", htmlElement.Name);
        }

        [TestMethod]
        public void get_attributes_from_single_tag()
        {
            var testCode = new HtmlContent("<br name='key' value='41323' />");
            Assert.IsTrue(Iterate(Seperator).CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Iterate(Seperator).Parse(testCode).ToArray();
            Assert.AreEqual(1,result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(ITag));
            var htmlElement = (ITag) element;
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


        [TestMethod]
        public void get_tag()
        {
            var testCode = new HtmlContent("<a href=\"index.php?option=com_content&view=frontpage&Itemid=1\">");
            Assert.IsTrue(Iterate(Seperator).CanParse(testCode),"Seperator.CanParse(testCode)");
            var result = Iterate(Seperator).Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            var tagElement = (ITag) result[0];
            Assert.AreEqual("a", tagElement.Name);
        }
        
  

 


    }
}