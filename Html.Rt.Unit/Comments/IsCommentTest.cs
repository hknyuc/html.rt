
using System.Linq;
using Html.Rt.Seperator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Html.Rt.Unit.Comments
{
    [TestClass]
    public class IsCommentTest
    {
        public IHtmlSeperator Seperator { get; } = new CommentSeperator();


        [TestMethod]
        public void is_comment_in_text()
        {
            this.is_comment_with("this is text");
        }

        [TestMethod]
        public void is_comment_in_html_element()
        {
            this.is_comment_with("<div>this is html element</div>");
        }


        [TestMethod]
        public void is_comment_in_comment()
        {
            this.is_comment_with("<!--this text comment-->");
        }

        [TestMethod]
        public void is_not_comment_1()
        {
            var testCode = "<-- hakan -->";
            Assert.IsFalse(Seperator.CanParse(testCode),"Seperator.CanParse(testCode) must false");
        }

        
        private void is_comment_with(string content)
        {
             var testCode = new HtmlContent($"<!-- {content} -->");
            testCode.NextTo("<!--");
            Assert.IsTrue(Seperator.CanParse(testCode), "Seperator.CanParse(testCode)");
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1,result.Length);
            var element = result.First();
            Assert.IsInstanceOfType(element, typeof(Comment));
            var comment = (Comment) element;
            Assert.AreEqual(testCode.Content,comment.Content);
        }
        
        
    }
}