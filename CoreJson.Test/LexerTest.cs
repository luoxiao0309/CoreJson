using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreJson.Test
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void ReadNumberTest()
        {
            Assert.AreEqual(TestNumber("123").Value, "123");
            Assert.AreEqual(TestNumber("123.0").Value, "123.0");
            Assert.AreEqual(TestNumber("123.01").Value, "123.01");
            Assert.AreEqual(TestNumber("-123.01").Value, "-123.01");
            Assert.AreEqual(TestNumber("-123.01.012").Value, "-123.01");
            Assert.ThrowsException<JsonParseException>(() => TestNumber("-"));
            Assert.ThrowsException<JsonParseException>(() => TestNumber("-0"));
            Assert.ThrowsException<JsonParseException>(() => TestNumber("-."));
        }

        private Token TestNumber(string expr)
        {
            var i = 1;
            return Lexer.ReadNumber(expr.AsSpan(), ref i);
        }

        [TestMethod]
        public void ReadStringTest()
        {
            Assert.AreEqual(TestString("123").Value, "123");
            Assert.AreEqual(TestString("abcd").Value, "abcd");
            Assert.AreEqual(TestString("�����").Value, "�����");
            Assert.AreEqual(TestString("12\\\"3").Value, @"12\""3");
        }

        private Token TestString(string expr)
        {
            expr = $"\"{expr}\"";
            var i = 1;
            return Lexer.ReadString(expr.AsSpan(), ref i);
        }

        [TestMethod]
        public void TokenizeTest()
        {
            Assert.AreEqual(Lexer.Analyzer("{\n    \"name\" : \"����\",\n    \"age\": 18\n}").Count, 10);
        }


    }
}