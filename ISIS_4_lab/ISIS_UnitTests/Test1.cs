using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ISIS_UnitTests
{
    [TestFixture]
    class Test1
    {
        [Test]
        public void MyFirstTest()
        {
            var num = 10;
            Assert.AreEqual(num, 10);
        }

    }
}
