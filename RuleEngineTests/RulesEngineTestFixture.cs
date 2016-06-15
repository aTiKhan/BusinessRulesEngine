﻿using System;
using System.Diagnostics;
using NUnit.Framework;

namespace RuleEngineTests
{
    [TestFixture]
    public class RulesEngineTestFixture
    {
        /// <summary>
        ///     Runs en action multiple times and count the time
        /// </summary>
        /// <param name="action"></param>
        /// <param name="runs"></param>
        /// <returns></returns>
        private long TimeInMilliseconds(Action<int> action, int runs)
        {
            // run once without counter to force JIT
            action(0);
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            try
            {
                for (var i = 0; i < runs; i++)
                {
                    action(i);
                }
            }
            finally
            {
                stopWatch.Stop();
            }

            return stopWatch.ElapsedMilliseconds;
        }

        [Test]
        public void Cascading_perf_test()
        {
            var abcd = new Abcd();
            var abcdRules = new AbcdRules(abcd);

            var time0 = TimeInMilliseconds(i =>
            {
                abcdRules.SetProperty("A", abcd, 1);
                Assert.AreEqual(100, abcd.A);
            }, 100);

            Console.WriteLine("Cascade rules {0} ms", time0);

            Assert.Less(time0, 100);
        }

        [Test]
        public void Check_cascading_by_rule_engine()
        {
            var bingo = new Bingo();
            var rules = new BingoRules(bingo);

            {
                // setting Message property does not trigger any rule
                var modified = rules.SetProperty("Message", bingo, "test");
                Assert.AreEqual(1, modified.Count);
                Assert.IsTrue(modified.Contains("Message"));
                Assert.AreEqual("test", bingo.Message);
            }

            {
                var modified = rules.SetProperty("X", bingo, 3);

                Assert.AreEqual(3, modified.Count);
                Assert.IsTrue(modified.Contains("X"));
                Assert.IsTrue(modified.Contains("Y"));
                Assert.IsTrue(modified.Contains("Message"));

                Assert.AreEqual("BINGO", bingo.Message);

                var x = bingo.X;

                // setting the samevalue should not trigger any rule
                modified = rules.SetProperty("X", bingo, x);
                Assert.AreEqual(0, modified.Count);
            }

            {
                var abcd = new Abcd();
                var abcdRules = new AbcdRules(abcd);

                var modified = abcdRules.SetProperty("A", abcd, 1);
                Assert.AreEqual(4, modified.Count);
                Assert.IsTrue(modified.Contains("A"));
                Assert.IsTrue(modified.Contains("B"));
                Assert.IsTrue(modified.Contains("C"));
                Assert.IsTrue(modified.Contains("D"));
                Assert.AreEqual(100, abcd.A);
            }
        }
    }
}