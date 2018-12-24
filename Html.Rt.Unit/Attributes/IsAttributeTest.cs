using System.Linq;
using Html.Rt.Seperator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Html.Rt.Unit.Attributes
{
    [TestClass]
    public class IsAttributeTest
    {
        public Html.Rt.Seperator.AttributeSeperator Seperator = new AttributeSeperator();
        
        [TestMethod]
        public void double_quotes_empty_value()
        {
            var testCode = " value=\"\"";
            Assert.IsTrue(Seperator.CanParse(testCode));
            var result = Seperator.Parse(testCode).ToArray();
            Assert.AreEqual(result.Length, 1);
            var resultAttributes = result.Cast<IAttribute>().ToArray();
            Assert.AreEqual(resultAttributes.Length, 1);
            var attribute = resultAttributes.First();
            Assert.AreEqual(attribute.Key, "value");
            Assert.AreEqual(attribute.Value, string.Empty);
        }
        

        [TestMethod]
        public void double_quotes_single()
        {
            var testCode = "value=\"1233\"";
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
            var testCode = "value=\"1233\" name=\"hakan 1234\"";
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
            var testCode = "value=\"1233\" value=\"8888\"";
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
            var testCode = @"value=""\""12344""";
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
            var testCode = "value";
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
            var testCode = "value checked selected";
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
            var testCode = "value='hakan'";
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
            var testCode = "value='27' name='hakan'";
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
            var testCode = @"value='\'hakan'";
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
            var testCode = "value='''";
            Assert.IsTrue(Seperator.CanParse(testCode));
        }

        [TestMethod]
        public void single_quotes_with_double_quotes()
        {
            var testCode = "value='27' name=\"hakan\"";
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
            var testCode = "value='27' name=\"hakan\" class='popup'";
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
        
        

     
    }
}