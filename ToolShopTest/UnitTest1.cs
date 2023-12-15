using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ToolShop;
using ToolShop.Pages;
using TestClass;

namespace ToolShopTest
{
    [TestClass()]
    public class UnitTest1
    {
        bool answer;
        string login;
        string password;
        string intNumber;
        string doubleNumber;

        [TestMethod()]
        public void AuthorizationTest1()
        {
            login = "";
            password = "";
            answer = TestClass.TestClass.authorization(login, password);
            Assert.IsFalse(answer);
        }
        [TestMethod()]
        public void AuthorizationTest2()
        {
            login = "1";
            password = "";
            answer = TestClass.TestClass.authorization(login, password);
            Assert.IsFalse(answer);
        }
        [TestMethod()]
        public void AuthorizationTest3()
        {
            login = "1";
            password = "1";
            answer = TestClass.TestClass.authorization(login, password);
            Assert.IsTrue(answer);
        }
        [TestMethod()]
        public void CorrectIntNumber1()
        {
            intNumber = "12";
            Assert.AreEqual(12, TestClass.TestClass.intNumber(intNumber));
        }
        [TestMethod()]
        public void CorrectIntNumber2()
        {
            intNumber = "12,1";
            Assert.AreEqual(-1, TestClass.TestClass.intNumber(intNumber));
        }
        [TestMethod()]
        public void CorrectIntNumber3()
        {
            intNumber = "0";
            Assert.AreEqual(0, TestClass.TestClass.intNumber(intNumber));
        }
        [TestMethod()]
        public void CorrectDoubleNumber1()
        {
            doubleNumber = "12.3";
            Assert.AreEqual(-1.0, TestClass.TestClass.doubleNumber(doubleNumber));
        }
        [TestMethod()]
        public void CorrectDoubleNumber2()
        {
            doubleNumber = "12,3";
            Assert.AreEqual(12.3, TestClass.TestClass.doubleNumber(doubleNumber));
        }
        [TestMethod()]
        public void CorrectDoubleNumber3()
        {
            doubleNumber = "asdas";
            Assert.AreEqual(-1.0, TestClass.TestClass.doubleNumber(doubleNumber));
        }
    }
}
