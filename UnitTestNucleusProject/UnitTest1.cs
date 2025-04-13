using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NucleusProject;

namespace TimeDurationTests
{
    public class TimeDurationTestUtils
    {
        public static void CheckStartOfDay(DateTimeOffset date)
        {
            Assert.AreEqual(date.Hour, 0);
            Assert.AreEqual(date.Minute, 0);
            Assert.AreEqual(date.Second, 0);
        }
        public static void CheckEndOfDay(DateTimeOffset date)
        {
            Assert.AreEqual(23, date.Hour);
            Assert.AreEqual(59, date.Minute);
            Assert.AreEqual(59, date.Second);
        }
    }
    [TestClass]
    public class CustomDurationTests {
        // yyyy,MM,dd
        static DateTime start = new DateTime(2025, 04, 07);
        static DateTime end = new DateTime(2025, 04, 09);
        // hh,mm,ss
        static TimeSpan offset = new TimeSpan(05, 30, 00);
        [TestMethod]
        public void TestDefaultConstructor() {
            // Verifying base behaviour
            TimeDuration duration = new TimeDuration(new DateTimeOffset(start,offset), new DateTimeOffset(end,offset));

            Assert.AreEqual(start,duration.start.DateTime);
            Assert.AreEqual(offset, duration.start.Offset);

            Assert.AreEqual(end, duration.end.DateTime);
            Assert.AreEqual(offset, duration.end.Offset);
        }
        [TestMethod]
        public void TestCastInDefaultConstructor() {
            // Verifying behavoiur when casting from `DateTime`
            // If not specified, the server offset is used
            TimeDuration duration = new TimeDuration(start,end);

            Assert.AreEqual(start, duration.start.DateTime);
            Assert.AreEqual(DateTimeOffset.Now.Offset, duration.start.Offset);

            Assert.AreEqual(end, duration.end.DateTime);
            Assert.AreEqual(DateTimeOffset.Now.Offset, duration.end.Offset);
        }
    }
    [TestClass]
    public class MonthDurationTests { 
        [TestMethod]
        public void TestBasicMonthDurationConstructor()
        {
            TimeSpan offset = new TimeSpan(05, 30, 00);
            DateTimeOffset refDate = new DateTimeOffset(new DateTime(2025,1,15),offset);

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Month);

            // Same month
            Assert.AreEqual(duration.start.Month, refDate.Month);
            Assert.AreEqual(duration.end.Month, refDate.Month);

            // Date is 1 (start) and last day of month (end)
            Assert.AreEqual(duration.start.Day, 1);
            Assert.AreEqual(duration.end.Day, DateTime.DaysInMonth(refDate.Year, refDate.Month));

            // Offset is propogated properly
            Assert.AreEqual(offset,duration.start.Offset);
            Assert.AreEqual(offset,duration.end.Offset);

            TimeDurationTestUtils.CheckStartOfDay(duration.start);
            TimeDurationTestUtils.CheckEndOfDay(duration.end);
        }
        [TestMethod]
        public void TestFirstDayOfMonth()
        {
            TimeSpan offset = new TimeSpan(05, 30, 00);
            DateTimeOffset refDate = new DateTimeOffset(new DateTime(2025, 1, 1), offset);

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Month);

            // Same month
            Assert.AreEqual(duration.start.Month, refDate.Month);
            Assert.AreEqual(duration.end.Month, refDate.Month);

            // Date is 1 (start) and last day of month (end)
            Assert.AreEqual(duration.start.Day, 1);
            Assert.AreEqual(duration.end.Day, DateTime.DaysInMonth(refDate.Year, refDate.Month));

            // Offset is propogated properly
            Assert.AreEqual(offset, duration.start.Offset);
            Assert.AreEqual(offset, duration.end.Offset);

            TimeDurationTestUtils.CheckStartOfDay(duration.start);
            TimeDurationTestUtils.CheckEndOfDay(duration.end);
        }
        [TestMethod]
        public void TestLastDayOfMonth()
        {
            TimeSpan offset = new TimeSpan(05, 30, 00);
            DateTimeOffset refDate = new DateTimeOffset(new DateTime(2025, 1, 31), offset);

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Month);

            // Same month
            Assert.AreEqual(duration.start.Month, refDate.Month);
            Assert.AreEqual(duration.end.Month, refDate.Month);

            // Date is 1 (start) and last day of month (end)
            Assert.AreEqual(duration.start.Day, 1);
            Assert.AreEqual(duration.end.Day, DateTime.DaysInMonth(refDate.Year, refDate.Month));

            // Offset is propogated properly
            Assert.AreEqual(offset, duration.start.Offset);
            Assert.AreEqual(offset, duration.end.Offset);

            TimeDurationTestUtils.CheckStartOfDay(duration.start);
            TimeDurationTestUtils.CheckEndOfDay(duration.end);
        }
        [TestMethod]
        public void TestLeapDayOfMonth()
        {
            TimeSpan offset = new TimeSpan(05, 30, 00);
            DateTimeOffset refDate = new DateTimeOffset(new DateTime(2024, 2, 29), offset);

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Month);

            // Same month
            Assert.AreEqual(duration.start.Month, refDate.Month);
            Assert.AreEqual(duration.end.Month, refDate.Month);

            // Date is 1 (start) and last day of month (end)
            Assert.AreEqual(duration.start.Day, 1);
            Assert.AreEqual(duration.end.Day, DateTime.DaysInMonth(refDate.Year, refDate.Month));

            // Offset is propogated properly
            Assert.AreEqual(offset, duration.start.Offset);
            Assert.AreEqual(offset, duration.end.Offset);

            TimeDurationTestUtils.CheckStartOfDay(duration.start);
            TimeDurationTestUtils.CheckEndOfDay(duration.end);
        }
    }
    [TestClass]
    public class WeekDurationTests
    {
        [TestMethod]
        public void TestBasicWeekDurationConstructor()
        {
            TimeSpan offset = new TimeSpan(5, 30, 0);
            DateTimeOffset refDate = new DateTimeOffset(new DateTime(2025, 4, 2), offset);

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Week);

            // Same month
            Assert.AreEqual(3, duration.start.Month);
            Assert.AreEqual(4, duration.end.Month);

            // Day is Sunday (start) and Saturday (end)
            Assert.AreEqual(DayOfWeek.Sunday, duration.start.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Saturday, duration.end.DayOfWeek);

            // Offset is propogated properly
            Assert.AreEqual(offset, duration.start.Offset);
            Assert.AreEqual(offset, duration.end.Offset);

            TimeDurationTestUtils.CheckStartOfDay(duration.start);
            TimeDurationTestUtils.CheckEndOfDay(duration.end);
        }
        [TestMethod]
        public void TestFirstDayOfWeek()
        {
            TimeSpan offset = new TimeSpan(05, 30, 00);
            DateTimeOffset refDate = new DateTimeOffset(new DateTime(2025, 4, 3), offset);

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Week);

            // Same month
            Assert.AreEqual(3, duration.start.Month);
            Assert.AreEqual(4, duration.end.Month);

            // Day is Sunday (start) and Saturday (end)
            Assert.AreEqual(DayOfWeek.Sunday, duration.start.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Saturday, duration.end.DayOfWeek);

            // Offset is propogated properly
            Assert.AreEqual(offset, duration.start.Offset);
            Assert.AreEqual(offset, duration.end.Offset);

            TimeDurationTestUtils.CheckStartOfDay(duration.start);
            TimeDurationTestUtils.CheckEndOfDay(duration.end);
        }
        [TestMethod]
        public void TestLastDayOfWeek()
        {
            TimeSpan offset = new TimeSpan(05, 30, 00);
            DateTimeOffset refDate = new DateTimeOffset(new DateTime(2025, 4, 5), offset);

            TimeDuration duration = TimeDuration.AroundDateTime(refDate, ViewSpan.Week);

            // Same month
            Assert.AreEqual(3, duration.start.Month);
            Assert.AreEqual(4, duration.end.Month);

            // Day is Sunday (start) and Saturday (end)
            Assert.AreEqual(DayOfWeek.Sunday, duration.start.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Saturday, duration.end.DayOfWeek);

            // Offset is propogated properly
            Assert.AreEqual(offset, duration.start.Offset);
            Assert.AreEqual(offset, duration.end.Offset);

            TimeDurationTestUtils.CheckStartOfDay(duration.start);
            TimeDurationTestUtils.CheckEndOfDay(duration.end);
        }
    }
    [TestClass]
    public class TimeDurationTests
    {
        // Generic tests on current date
        // Tests for basic properties
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
