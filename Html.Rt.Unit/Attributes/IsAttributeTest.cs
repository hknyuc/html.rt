using System.Linq;
using System.Text;
using Html.Rt.Seperator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Html.Rt.Unit.Attributes
{
    [TestClass]
    public class IsAttributeTest
    {
        public IHtmlSeperator Seperator =  new SeperatorIterator(new AttributeSeperator2());
        
        [TestMethod]
        public void double_quotes_empty_value()
        {
            var testCode = new HtmlContent(" value=\"\"");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(1,result.Length);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 1);
            var attribute = resultAttributes.First();
            Assert.AreEqual(attribute.Key, "value");
            Assert.AreEqual(attribute.Value, string.Empty);
        }
        

        [TestMethod]
        public void double_quotes_single()
        {
            var testCode = new HtmlContent("value=\"1233\"");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 1);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 1);
            var attribute = resultAttributes.First();
            Assert.AreEqual(attribute.Key, "value");
            Assert.AreEqual(attribute.Value, "1233");
        }

        [TestMethod]
        public void double_quotes_more()
        {
            var testCode = new HtmlContent("value=\"1233\" name=\"hakan 1234\"");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 2);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 2);
            var attributeValue = resultAttributes[0];
            var attributeName = resultAttributes[1];
            Assert.AreEqual(attributeValue.Key, "value");
            Assert.AreEqual(attributeValue.Value, "1233");
            Assert.AreEqual(attributeName.Key, "name");
            Assert.AreEqual(attributeName.Value, "hakan 1234");
        }

        [TestMethod]
        public void double_quotes_doubled_key_attribute()
        {
            var testCode = new HtmlContent("value=\"1233\" value=\"8888\"");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 2);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 2);
            var attribute1 = resultAttributes[0];
            var attribute2 = resultAttributes[1];
            Assert.AreEqual(attribute1.Key, "value");
            Assert.AreEqual(attribute1.Value, "1233");
            Assert.AreEqual(attribute2.Key, "value");
            Assert.AreEqual(attribute2.Value, "8888");
        }

        [TestMethod]
        public void double_quotes_in_double_quotes_string()
        {
            var testCode = new HtmlContent(@"value=""\""12344""");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 1);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 1);
            var attribute = resultAttributes.First();
            Assert.AreEqual(attribute.Key, "value");
            Assert.AreEqual(attribute.Value, "\\\"12344");
        }

        [TestMethod]
        public void no_value_in_attribute_single()
        {
            var testCode = new HtmlContent("value");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 1);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(result.Length, 1);
            var attribute = resultAttributes[0];
            Assert.AreEqual(attribute.Key, "value");
            Assert.AreEqual(attribute.Value, string.Empty);
        }

        [TestMethod]
        public void no_value_in_attribute_more()
        {
            var testCode = new HtmlContent("value checked selected");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 3);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 3);
            var attributeOne = resultAttributes[0];
            var attributeTwo = resultAttributes[1];
            var attributeThree = resultAttributes[2];
            Assert.AreEqual(attributeOne.Key, "value");
            Assert.AreEqual(attributeOne.Value, string.Empty);
            Assert.AreEqual(attributeTwo.Key, "checked");
            Assert.AreEqual(attributeTwo.Value, string.Empty);
            Assert.AreEqual(attributeThree.Key, "selected");
            Assert.AreEqual(attributeThree.Value, string.Empty);
        }

        [TestMethod]
        public void single_quotes()
        {
            var testCode = new HtmlContent("value='hakan'");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 1);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 1);
            var attribute = resultAttributes[0];
            Assert.AreEqual(attribute.Key, "value");
            Assert.AreEqual(attribute.Value, "hakan");
        }
        
        [TestMethod]
        public void single_quotes_more()
        {
            var testCode = new HtmlContent("value='27' name='hakan'");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 2);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 2);
            var attributeOne = resultAttributes[0];
            Assert.AreEqual(attributeOne.Key, "value");
            Assert.AreEqual(attributeOne.Value, "27");
            var attributeTwo = resultAttributes[1];
            Assert.AreEqual(attributeTwo.Key, "name");
            Assert.AreEqual(attributeTwo.Value, "hakan");
        }


        [TestMethod]
        public void single_quotes_in_single_quotes()
        {
            var testCode = new HtmlContent(@"value='\'hakan'");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 1);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 1);
            var attribute = resultAttributes[0];
            Assert.AreEqual(attribute.Key, "value");
            Assert.AreEqual(attribute.Value, @"\'hakan");
        }

        [TestMethod]
        public void single_quotes_in_invalid_quotes()
        {
            var testCode = new HtmlContent("value='''");
            Assert.IsTrue(Seperator.CanParse(testCode));
        }

        [TestMethod]
        public void single_quotes_with_double_quotes()
        {
            var testCode = new HtmlContent("value='27' name=\"hakan\"");
            Assert.IsNotNull(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 2);
            var resulAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resulAttributes.Length, 2);
            var attributeOne = resulAttributes[0];
            var attributeTwo = resulAttributes[1];
            Assert.AreEqual(attributeOne.Key, "value");
            Assert.AreEqual(attributeOne.Value, "27");
            Assert.AreEqual(attributeTwo.Key, "name");
            Assert.AreEqual(attributeTwo.Value, "hakan");
        }


        [TestMethod]
        public void single_quotes_with_double_quotes2()
        {
            var testCode = new HtmlContent("value='27' name=\"hakan\" class='popup'");
            Assert.IsNotNull(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 3);
            var resulAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resulAttributes.Length, 3);
            var attributeOne = resulAttributes[0];
            var attributeTwo = resulAttributes[1];
            var attributeThree = resulAttributes[2];
            Assert.AreEqual(attributeOne.Key, "value");
            Assert.AreEqual(attributeOne.Value, "27");
            Assert.AreEqual(attributeTwo.Key, "name");
            Assert.AreEqual(attributeTwo.Value, "hakan");
            Assert.AreEqual(attributeThree.Key, "class");
            Assert.AreEqual(attributeThree.Value, "popup");
        }

        [TestMethod]
        public void single_with_key_value()
        {
            var testCode = new HtmlContent("value='27' checked test=\"\\\"true\"");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(3,result.Length);
            var resulAttributes = result.Cast<IAttribute>().ToArray();
            var attributeOne = resulAttributes[0];
            var attributeTwo = resulAttributes[1];
            var attributeThree = resulAttributes[2];
            Assert.AreEqual(attributeOne.Key, "value");
            Assert.AreEqual(attributeOne.Value, "27");
            Assert.AreEqual(attributeTwo.Key, "checked");
            Assert.AreEqual(attributeThree.Key, "test");
            Assert.AreEqual(attributeThree.Value, "\\\"true");
        }

        [TestMethod]
        public void fourth_type_attribute()
        {
            var testCode = new HtmlContent("Value='52' checked name=\"10\" border=0");
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(4, result.Length);
            var resulAttributes = result.Cast<IAttribute>().ToArray();
            var attributeOne = resulAttributes[0];
            var attributeTwo = resulAttributes[1];
            var attributeThree = resulAttributes[2];
            var attributeFourth = resulAttributes[3];
            Assert.AreEqual(attributeOne.Key, "Value");
            Assert.AreEqual(attributeOne.Value, "52");
            Assert.AreEqual(attributeTwo.Key, "checked");
            Assert.AreEqual(attributeThree.Key, "name");
            Assert.AreEqual(attributeThree.Value, "10");
            Assert.AreEqual(attributeFourth.Key, "border");
            Assert.AreEqual(attributeFourth.Value,"0");
        }

        [TestMethod]
        public void invalid_attributes_value()
        {
            var testCode = new HtmlContent(TestUtility.GetFile("invalidCh_attribute.txt"));
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
        }

        [TestMethod]
        public void concat_test()
        {

            var  file = TestUtility.GetFile("invalidCh_attribute.txt");
            var str = string.Empty;
            var strBuilder = new StringBuilder();
            for (var i = 0; i < file.Length; i++)
            {
                strBuilder.Append(file[i]);
            }

            str = strBuilder.ToString();
        }


        [TestMethod]
        public void concat_test2()
        {
            var file = TestUtility.GetFile("invalidCh_attribute.txt");
            var str = file.Substring(0, file.Length - 1);
        }
   
    }
}