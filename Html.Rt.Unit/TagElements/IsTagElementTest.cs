using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Html.Rt.Seperator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Html.Rt.Unit.TagElements
{
    [TestClass]
    public class IsTagElementTest
    {
        public IHtmlSeperator Seperator { get; } = new ElementSeperator();
        
        
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
        public void get_child_nodes()
        {
            const string testCode = "<div id='title' class=\"title\"><!--this is comment-->This is text <div>this is content of element</div></div>";
            Assert.IsTrue(Seperator.CanParse(testCode),"Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(IHtmlElement));
            var htmlElement = (IHtmlElement) element;
            var attributes = htmlElement.Attributes.ToArray();
            var attributeOne = attributes[0];
            var attributeTwo = attributes[1];
            Assert.AreEqual("id", attributeOne.Key);
            Assert.AreEqual("title", attributeOne.Value);
            Assert.AreEqual("class", attributeTwo.Key);
            Assert.AreEqual("title", attributeTwo.Value);
            var childNodes = htmlElement.ToArray();
            var childOne = childNodes[0];
            Assert.IsInstanceOfType(childOne, typeof(Comment));
            var comment = (Comment) childOne;
            Assert.AreEqual("this is comment", comment.Content);
            var childTwo = childNodes[1];
            Assert.IsInstanceOfType(childTwo, typeof(Text));
            Assert.AreEqual("This is text", ((Text) childTwo).Content);
            var childThree = childNodes[2];
            Assert.IsInstanceOfType(childThree, typeof(IHtmlElement));
            var divElement = (IHtmlElement) childThree;
            var divInsides = divElement.ToArray();
            Assert.AreEqual(1, divInsides.Length);
            var textElement = divInsides.First();
            Assert.IsInstanceOfType(textElement, typeof(Text));
            var text = (Text) textElement;
            Assert.AreEqual("this is content of element", text.Content);
        }


        [TestMethod]
        public void get_elements_sequence_1()
        {
            const string testCode = "<div></div><span></span>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(4,result.Length);
            var elementOne = result[0];
            Assert.IsInstanceOfType(elementOne, typeof(IHtmlElement));
            var divElement = (IHtmlElement) elementOne;
            Assert.AreEqual("div", divElement.Name);
            var elementTwo = result[1];
            Assert.IsInstanceOfType(elementTwo, typeof(EndTag));
            var endDivElement = (EndTag) elementTwo;
            Assert.AreEqual("div",endDivElement.Name);
            var elementThree = result[2];
            Assert.IsInstanceOfType(elementThree, typeof(IHtmlElement));
            var spanElement = (IHtmlElement) elementThree;
            Assert.AreEqual("span", spanElement.Name);
            var elementFour = result[3];
            var endSpanElement = (EndTag) elementFour;
            Assert.AreEqual("span", endSpanElement.Name);
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
        public void get_end_tags()
        {
            const string testCode = "</span></div>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0],typeof(EndTag));
            Assert.IsInstanceOfType(result[1], typeof(EndTag));
            var endOne = (EndTag) result[0];
            var endTwo = (EndTag) result[1];
            Assert.AreEqual("span", endOne.Name);
            Assert.AreEqual("div", endTwo.Name);
        }

        [TestMethod]
        public void get_elements_first_end_tags()
        {
            const string testCode = "</span></div><div id='end'></div>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(4, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(EndTag));
            Assert.IsInstanceOfType(result[1], typeof(EndTag));
            Assert.IsInstanceOfType(result[2], typeof(IHtmlElement));
            Assert.IsInstanceOfType(result[3], typeof(IHtmlElement));
            var oneEndTag = (EndTag) result[0];
            var twoEndTag = (EndTag) result[1];
            var htmlElement = (IHtmlElement) result[2];
            var threeEndTag = (EndTag) result[3];
            Assert.AreEqual("span",oneEndTag.Name);
            Assert.AreEqual("div", twoEndTag.Name);
            Assert.AreEqual("div", htmlElement.Name);
            Assert.AreEqual("div", threeEndTag.Name);
        }

        [TestMethod]
        public void tag_element_with_content_of_comment_which_have_tag_element()
        {
            const string testCode = "<div><!--</div>-->";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(IHtmlElement));
            Assert.IsInstanceOfType(result[1], typeof(Comment));
            var htmlElement = (IHtmlElement) result[0];
            var comment = (Comment) result[1];
            Assert.AreEqual("div", htmlElement.Name);
            Assert.AreEqual("</div>", comment.Content);
        }

        [TestMethod]
        public void get_tag_element_when_attributes_have_tag_element()
        {
            const string testCode = "<div innerHtml='<div name=\"title\"></div>'>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(IHtmlElement));
            var htmlElement = (IHtmlElement) result[0];
            Assert.AreEqual("div", htmlElement.Name);
            var attributes = htmlElement.Attributes.ToArray();
            Assert.AreEqual(1, attributes.Length);
            var attribute = attributes[0];
            Assert.AreEqual("innerHtml",attribute.Key);
            Assert.AreEqual("<div name=\"title\"></div>", attribute.Value);
            
        }


        [TestMethod]
        public void style_basic_works()
        {
            const string styleContent = @"    .name {
              display:none;
              margin:10px;
              padding:10px;
              }";
            const string testCode = @"<style ='text/css'>"+styleContent+"</style>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(IHtmlElement));
            Assert.IsInstanceOfType(result[1], typeof(Text));
            Assert.IsInstanceOfType(result[2], typeof(EndTag));
            var htmlElement = (IHtmlElement) result[0];
            var textContent = (Text) result[1];
            var endTag = (EndTag) result[2];
            Assert.AreEqual("style", htmlElement.Name);
            Assert.AreEqual(styleContent, textContent.Content);
            Assert.AreEqual("style", endTag.Name);
        }


        [TestMethod]
        public void script_basic_works()
        {
            const string scriptContent = @"function () {
             return '</div>';
            }";
            
            const string testCode = "<script>"+scriptContent+"</script>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.IsInstanceOfType(result[0], typeof(IHtmlElement));
            Assert.IsInstanceOfType(result[1], typeof(Text));
            Assert.IsInstanceOfType(result[2], typeof(EndTag));
            var script = (IHtmlElement) result[0];
            var content = (Text) result[1];
            var endTag = (EndTag) result[2];
            Assert.AreEqual("script", script.Name);
            Assert.AreEqual(content.Content, scriptContent);
            Assert.AreEqual("script", endTag.Name);
        }
    }
}