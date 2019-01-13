using System;
using System.Collections.Generic;
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
        public IHtmlSeperator Seperator { get; } = new StandartHtmlSeperator();
        

        /*
        [TestMethod]
        public void get_child_nodes()
        {
            const string testCode = "<div id='title' class=\"title\"><!--this is comment-->This is text <div>this is content of element</div></div>";
            Assert.IsTrue(Seperator.CanParse(testCode),"Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            var element = result.FirstOrDefault();
            Assert.IsNotNull(element);
            Assert.IsInstanceOfType(element, typeof(ITag));
            var htmlElement = (ITag) element;
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
            Assert.AreEqual("This is text", ((Text) childTwo).Markup);
            var childThree = childNodes[2];
            Assert.IsInstanceOfType(childThree, typeof(ITag));
            var divElement = (ITag) childThree;
            var divInsides = divElement.ToArray();
            Assert.AreEqual(1, divInsides.Length);
            var textElement = divInsides.First();
            Assert.IsInstanceOfType(textElement, typeof(Text));
            var text = (Text) textElement;
            Assert.AreEqual("this is content of element", text.Markup);
        }


        [TestMethod]
        public void get_elements_sequence_1()
        {
            const string testCode = "<div></div><span></span>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.AreEqual(4,result.Length);
            var elementOne = result[0];
            Assert.IsInstanceOfType(elementOne, typeof(ITag));
            var divElement = (ITag) elementOne;
            Assert.AreEqual("div", divElement.Name);
            var elementTwo = result[1];
            Assert.IsInstanceOfType(elementTwo, typeof(EndTag));
            var endDivElement = (EndTag) elementTwo;
            Assert.AreEqual("div",endDivElement.Name);
            var elementThree = result[2];
            Assert.IsInstanceOfType(elementThree, typeof(ITag));
            var spanElement = (ITag) elementThree;
            Assert.AreEqual("span", spanElement.Name);
            var elementFour = result[3];
            var endSpanElement = (EndTag) elementFour;
            Assert.AreEqual("span", endSpanElement.Name);
        }
        */

        [TestMethod]
        public void get_end_tags()
        {
            const string testCode = "</span></div>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
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
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.AreEqual(4, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(EndTag));
            Assert.IsInstanceOfType(result[1], typeof(EndTag));
            Assert.IsInstanceOfType(result[2], typeof(ITag));
            Assert.IsInstanceOfType(result[3], typeof(EndTag));
            var oneEndTag = (EndTag) result[0];
            var twoEndTag = (EndTag) result[1];
            var htmlElement = (ITag) result[2];
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
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            Assert.IsInstanceOfType(result[1], typeof(Comment));
            var htmlElement = (ITag) result[0];
            var comment = (Comment) result[1];
            Assert.AreEqual("div", htmlElement.Name);
            Assert.AreEqual("</div>", comment.Content);
        }

        
        [TestMethod]
        public void script_basic_works()
        {
            const string scriptContent = @"function () {
             return '</div>';
            }";
            
            const string testCode = "<script>"+scriptContent+"</script>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            Assert.IsInstanceOfType(result[1], typeof(RawText));
            Assert.IsInstanceOfType(result[2], typeof(EndTag));
            var script = (ITag) result[0];
            var content = (RawText) result[1];
            var endTag = (EndTag) result[2];
            Assert.AreEqual("script", script.Name);
            Assert.AreEqual(scriptContent,content.Markup);
            Assert.AreEqual("script", endTag.Name);
  
        }


 
        
        [TestMethod]
        public void style_basic_works()
        {
            const string styleContent = @"    .name {
              display:none;
              margin:10px;
              padding:10px;
              }";
            const string testCode = @"<style type='text/css'>"+styleContent+"</style>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            Assert.IsInstanceOfType(result[1], typeof(RawText));
            Assert.IsInstanceOfType(result[2], typeof(EndTag));
            var htmlElement = (ITag) result[0];
            var textContent = (RawText) result[1];
            var endTag = (EndTag) result[2];
            Assert.AreEqual("style", htmlElement.Name);
            Assert.AreEqual(styleContent.Length, textContent.Markup.Length);
            Assert.AreEqual(styleContent, textContent.Markup);
            Assert.AreEqual("style", endTag.Name);
        }

        [TestMethod]
        public void style_content_have_html()
        {
            const string styleContent = @".name {
            content:'</div>',
            margin:10px,
            padding:10px
            }";
            const string testCode = @"<style type='text/css'>"+styleContent+"</style>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            Assert.IsInstanceOfType(result[1], typeof(RawText));
            Assert.IsInstanceOfType(result[2], typeof(EndTag));
            var htmlElement = (ITag) result[0];
            var textContent = (RawText) result[1];
            var endTag = (EndTag) result[2];
            Assert.AreEqual("style", htmlElement.Name);
            Assert.AreEqual(styleContent, textContent.Markup);
            Assert.AreEqual("style", endTag.Name);
            


        }
        
        [TestMethod]
        public void get_tag_element_when_attributes_have_tag_element()
        {
            const string testCode = "<div innerHtml='<div name=\"title\"></div>'>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            var htmlElement = (ITag) result[0];
            Assert.AreEqual("div", htmlElement.Name);
            var attributes = htmlElement.Attributes.ToArray();
            Assert.AreEqual(1, attributes.Length);
            var attribute = attributes[0];
            Assert.AreEqual("innerHtml",attribute.Key);
            Assert.AreEqual("<div name=\"title\"></div>", attribute.Value);
            
        }
        [TestMethod]
        public void get_tag_element_when_attributes_have_tag_element2()
        {
            const string testCode = "<div innerHtml='<div name=\\'title\\'></div>'>";
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.ParseFromOrigin(testCode).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            var htmlElement = (ITag) result[0];
            Assert.AreEqual("div", htmlElement.Name);
            var attributes = htmlElement.Attributes.ToArray();
            Assert.AreEqual(1, attributes.Length);
            var attribute = attributes[0];
            Assert.AreEqual("innerHtml",attribute.Key);
            Assert.AreEqual("<div name=\\'title\\'></div>", attribute.Value);
        }

        [TestMethod]
        public void first_text_after_script()
        {
            var testCode = new HtmlContent("this is text area <script>function test(){console.log('hi')}</script>");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(4, result.Length);
            var textElement = result[0];
            Assert.IsInstanceOfType(textElement, typeof(Text));
            var tagElement = result[1];
            Assert.IsInstanceOfType(tagElement, typeof(ITag));
            var scriptText = result[2];
            Assert.IsInstanceOfType(scriptText, typeof(RawText));
            var endElement = result[3];
            Assert.IsInstanceOfType(endElement, typeof(EndTag));

        }

        [TestMethod]
        public void tag_contents_empty_text()
        {
            var testCode = new HtmlContent("<i></i>");
            Assert.IsTrue(Seperator.CanParse(testCode),"Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ITag));
            Assert.IsInstanceOfType(result[1],typeof(EndTag));
            var firstElement = (ITag) result[0];
            var endElement = (EndTag) result[1];
            Assert.AreEqual("i", firstElement.Name);
            Assert.AreEqual("i", endElement.Name);
        }



      

    


      
    }
}