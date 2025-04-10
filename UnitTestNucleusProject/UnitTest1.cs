using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NucleusProject;

namespace UnitTestNucleusProject
{
    [TestClass]
    public class TimeDurationTests
    {
        private void checkStartOfDay(DateTimeOffset date)
        {
            Assert.AreEqual(date.Hour, 0);
            Assert.AreEqual(date.Minute, 0);
            Assert.AreEqual(date.Second, 0);
        }
        private void checkEndOfDay(DateTimeOffset date)
        {
            Assert.AreEqual(23, date.Hour);
            Assert.AreEqual(59, date.Minute);
            Assert.AreEqual(59, date.Second);
        }
        [TestMethod]
        public void TestDefaultConstructor()
        {
            DateTimeOffset start = DateTimeOffset.Now;
            DateTimeOffset end = DateTimeOffset.Now.AddDays(5);

            TimeDuration duration = new TimeDuration(start, end);

            Assert.AreEqual(start, duration.start);
            Assert.AreEqual(end, duration.end);
        }
        [TestMethod]
        public void TestMonthSpan()
        {
            DateTimeOffset refDate = DateTimeOffset.Now;

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Month);

            // Same month
            Assert.AreEqual(duration.start.Month, refDate.Month);
            Assert.AreEqual(duration.end.Month, refDate.Month);

            // Date is 1 (start) and last day of month (end)
            Assert.AreEqual(duration.start.Day, 1);
            Assert.AreEqual(duration.end.Day, DateTime.DaysInMonth(refDate.Year, refDate.Month));

            checkStartOfDay(duration.start);
            checkEndOfDay(duration.end);
        }
        [TestMethod]
        public void TestDaySpan()
        {
            DateTimeOffset refDate = DateTimeOffset.Now;

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Day);

            Console.WriteLine(refDate.Day);

            // Same month
            Assert.AreEqual(refDate.Month,duration.start.Month);
            Assert.AreEqual(refDate.Month,duration.end.Month);

            // Date is current in both cases
            Assert.AreEqual(duration.start.Day, refDate.Day);
            Assert.AreEqual(refDate.Day, duration.end.Day);

            checkStartOfDay(duration.start);
            checkEndOfDay(duration.end);
        }
        [TestMethod]
        public void TestWeekSpan()
        {
            DateTimeOffset refDate = DateTimeOffset.Now;

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Week);

            // Same month
            Assert.AreEqual(refDate.Month, duration.start.Month);
            Assert.AreEqual(refDate.Month, duration.end.Month);

            // Day is Sunday (start) and Saturday (end)
            Assert.AreEqual(DayOfWeek.Sunday, duration.start.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Saturday, duration.end.DayOfWeek);

            checkStartOfDay(duration.start);
            checkEndOfDay(duration.end);
        }
    }
}
