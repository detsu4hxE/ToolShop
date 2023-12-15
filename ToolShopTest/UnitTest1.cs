using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ToolShop;
using ToolShop.Pages;

namespace ToolShopTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGooLogin()
        {
            AuthorizationPage authorizationPage = new AuthorizationPage();
            authorizationPage.loginBox.Text = "1";
            authorizationPage.passwordBox.Password = "1";

            authorizationPage.loginButton_Click(null, null);

            Assert.IsNotNull(App.CurrentUser);

            Assert.AreEqual(true, oUser.Login("admin", "admin"), "admin");
        }
    }
}
