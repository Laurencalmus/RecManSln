using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resource;
using RecMan.Models;

namespace ResourcesTest
{
    [TestClass]
    public class ResourceTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // arrange  
            String Source = "700 Classroom Activities";
            String Topic = "Be/get used to";
            String Content = "Hey Mom Blabla";
            Resource resource = new Resource("Write Home", Source, Topic, Content);

            // act  
            //resource.make();

            // assert  
            //double actual = account.Balance;
            //Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
        }
    }
}
