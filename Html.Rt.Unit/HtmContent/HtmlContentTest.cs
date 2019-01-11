using Html.Rt.Seperator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Html.Rt.Unit.HtmContent
{
    [TestClass]
    public class HtmlContentTest
    {

        [TestMethod]
        public void first_value_default_char()
        {
            var str = "my name is hakan";
            var testCode = CreateContent(str);
            Assert.AreEqual(testCode.CurrentChar, default(char));
        }
        
        [TestMethod]
        public void next_works()
        {
            var testCode = CreateContent("my name is hakan");
            Assert.AreEqual(testCode.CurrentChar, default(char));
            testCode.Next();
            Assert.AreEqual(testCode.CurrentChar, 'm');
            testCode.Next();
            Assert.AreEqual(testCode.CurrentChar, 'y');
            Assert.AreEqual("my", testCode.Content);
        }

        private IHtmlContent CreateContent(string content)
        {
            return new HtmlContent(content);
        }

        [TestMethod]
        public void next_to_index()
        {
            var testCode = CreateContent("my name is hakan");
            testCode.NextTo(6);
            Assert.AreEqual("my name", testCode.Content, testCode.Content);
        }

        [TestMethod]
        public void next_to_times_eq_next_to()
        {
            var str = "my name is hakan";
            var testCode = CreateContent(str);
            7.Times(() => testCode.Next());
            var testCode2 = CreateContent(str);
            testCode2.NextTo(6);
            Assert.AreEqual(testCode.Content, testCode2.Content, testCode.Content + ":" + testCode2.Content);
        }

        [TestMethod]
        public void jump_to_last()
        {
            const string str = "my name is hakan";
            var testCode = CreateContent(str);
            Assert.AreEqual(string.Empty, testCode.Content);
            testCode.JumpLast();
            Assert.AreEqual("my name is hakan", testCode.Content);
            Assert.AreEqual(default(char), testCode.CurrentChar);
            Assert.AreEqual(str.Length, testCode.Index);
        }

        [TestMethod]
        public void next_find_text()
        {
            const string str = "my name is hakan";
            var testCode = CreateContent(str);
            testCode.NextTo("hakan");
            Assert.AreEqual(str, testCode.Content);
        }


        [TestMethod]
        public void outstrip()
        {
            const string str = "my name is hakan";
            var testCode = CreateContent(str);
            testCode.NextTo(10);
            Assert.AreEqual("my name is ", testCode.Content);
            testCode.Outstrip();
            Assert.AreEqual("h", testCode.Content);
            testCode.NextTo(15);
            Assert.AreEqual("hakan", testCode.Content);
        }


        [TestMethod]
        public void done_work()
        {
             const string str = "my name is hakan";
             var testCode = CreateContent(str);
            str.Length.Times(() => testCode.Next());
            Assert.AreEqual(str, testCode.Content);
            Assert.IsFalse(testCode.Next());
            Assert.AreEqual(default(char), testCode.CurrentChar);
        }

        
    }
}