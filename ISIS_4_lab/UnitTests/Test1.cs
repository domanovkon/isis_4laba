using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using ISIS_1_lab;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    class Test1
    {
        private Form2 form2 = null;
        private IniFiles ini = null;
        [SetUp]
        public void Setup()
        {
           form2 = new Form2();
           ini = new IniFiles("C:\\Users\\Константин\\Desktop\\isis1lab_2.0.ini");
        }
        [Test]
        public void MyFirstTest()
        {
            var num = 10;
            Assert.AreEqual(num, 10);
        }
        [Test]
        public void zagr_test()
        {
            int result = form2.zagr();
            Assert.That(result, Is.EqualTo(0));
        }
        [Test]
        public void GetCountWordsByLenght_test()
        {
            int count = form2.GetCountWordsByLength("абв где", 3);
            Assert.That(count, Is.EqualTo(2));
        }
        [Test]
        public void ochist_test()
        {
            int count = form2.ochist();
            Assert.That(count, Is.EqualTo(-1));
        }
        [Test]
        public void prov_test()
        {
            string a = form2.prov();
            Assert.That(a, Is.Null);
        }
        [Test]
        public void ini_test()
        {
            string b = "C:\\Users\\Константин\\Desktop\\isis1lab_2.0.ini";
            string result = form2.ini_read(b);
            Assert.That(result, Does.Contain("Загр").IgnoreCase);
        }
        [Test]
        public void delete_section_test()
        {
            int result = ini.DeleteSection();
            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public void delete_key_test()
        {
            int result = ini.DeleteKey(null);
            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public void getprivatsection_test()
        {
            string []a = new string[] { };
            bool result = ini.GetPrivateProfileSection(null, null, out a);
            Assert.That(result, Is.False);
        }
        [Test]
        public void key_test()
        {
            bool result = ini.KeyExists(null, null);
            Assert.That(result, Is.True);
        }
        [Test]
        public void READINI_test()
        {
            string result = ini.ReadINI("App", "Value_button1");
            Assert.That(result, Is.EqualTo("Старт"));
        }
    }
}
