using NUnit.Framework;
using System;
using System.Collections.Generic;
namespace EcoTribe.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            int x = 2;
            int y = 8;
            int z = x + y;
            Assert.That(z, Is.EqualTo(10));
        }

        [Test]
        public void Test2()
        {
            string? a = null;
            Assert.That(a, Is.Null);
        }

        [Test]
        public void Test3()
        {
            string b = "SomethingViewModel";
            Assert.That(b, Does.EndWith("ViewModel"));
        }

        [Test]
        public void Test4()
        {
            string str = "A very good eco friendly power plant";
            Assert.That(str, Does.Contain("Eco"));
        }

        [Test]
        public void Test5()
        {
            int num = 5;
            Assert.That(num, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void Test6()
        {
            List<string> cities = new List<string> { "Plovdiv", "Pazardzhik", "Sofia" };
            Assert.That(cities, Has.Count.EqualTo(3));
        }

        [Test]
        public void Test7()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            List<int> numbers2 = new List<int> { 5, 4, 3, 2, 1 };
            Assert.That(numbers, Is.EquivalentTo(numbers2));
        }

        [Test]
        public void Test8()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            bool result = numbers.Remove(6);
            Assert.That(result, Is.False);
        }

        [Test]
        public void Test9()
        {
            List<string> actualNames = new List<string> { "Niki", "Slavi", "Pesho" };
            List<string> expectedNames = new List<string> { "Pesho", "Niki", "Slavi" };    
            Assert.That(actualNames, Is.EquivalentTo(expectedNames));
        }

        class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void Test10()
        {
            List<Person> people1 = new List<Person>
            {
                new Person { Id = 1, Name = "Niki" },
                new Person { Id = 2, Name = "Slavi" },
                new Person { Id = 3, Name = "Pesho" }
            };
            List<Person> people2 = new List<Person>
            {
                new Person { Id = 1, Name = "Ivan" },
                new Person { Id = 2, Name = "Mitko" },
                new Person { Id = 3, Name = "Georgi" }
            };
            Assert.That(people1.Select(p => p.Id), Is.EquivalentTo(people2.Select(p => p.Id)));
        }
    }
}
