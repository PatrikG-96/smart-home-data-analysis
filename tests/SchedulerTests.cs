using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iMotionsImportTools.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class SchedulerTests
    {

        private int timesExecuted = 0;

        [TestMethod]
        public void PreemptiveFrequencyTest()
        {
            const int expectedFrequency = 10;
            int intervalTime = 1000 / expectedFrequency;
            int worstCaseExecutionTime = 50;
            int cycles = 100;

            double errorMargin = 0.2;
            int errorMarginIntervalTime = (int) (1000 / (expectedFrequency - errorMargin));

            var scheduler = new PreemptiveScheduler(intervalTime, worstCaseExecutionTime)
            {
                Debug = true,
                CycleBreakpoint = cycles
            };
            scheduler.Start();
            Thread.Sleep(cycles * errorMarginIntervalTime);

            double resultingFrequency = (double)scheduler.Cycles / scheduler.fullTest.ElapsedMilliseconds * 1000;

            Assert.AreEqual(false, scheduler.IsRunning);
            Assert.IsTrue(resultingFrequency >= expectedFrequency - errorMargin && resultingFrequency <= expectedFrequency + errorMargin);
        }

        [TestMethod]
        public void PreemptiveExecutionTest()
        {
            const int expectedFrequency = 10;
            int intervalTime = 1000 / expectedFrequency;
            int worstCaseExecutionTime = 50;
            int cycles = 100;

            double errorMargin = 0.2;
            int errorMarginIntervalTime = (int)(1000 / (expectedFrequency - errorMargin));

            var scheduler = new PreemptiveScheduler(intervalTime, worstCaseExecutionTime)
            {
                Debug = true,
                CycleBreakpoint = cycles
            };
            scheduler.ControllerEvents += Execution;
            scheduler.Start();
            Thread.Sleep(cycles * errorMarginIntervalTime);

            Assert.AreEqual(cycles, scheduler.Cycles);
            Assert.AreEqual(timesExecuted, cycles);
        }

        private void Execution(object sender, SchedulerEventArgs args)
        {
            timesExecuted++;
        }
    }
}
