using System;
using System.Globalization;

namespace LabaleMakerService.Tools

{
  public  class DateTimeFunc
    {


        public static string MiladiToShamsi(DateTime? miladiTarikh)
        {
            var pc = new System.Globalization.PersianCalendar();
            if (miladiTarikh == null)
            {
                var current = DateTime.Now;
                var shamsiTarikh =
                    $"{pc.GetYear(current)}/{Convert.ToString(pc.GetMonth(current)).PadLeft(2, '0')}/{Convert.ToString(pc.GetDayOfMonth(current)).PadLeft(2, '0')}";
                return shamsiTarikh;

            }
            else {
                var shamsiTarikh =
                    $"{pc.GetYear(miladiTarikh.Value)}/{Convert.ToString(pc.GetMonth(miladiTarikh.Value)).PadLeft(2, '0')}/{Convert.ToString(pc.GetDayOfMonth(miladiTarikh.Value)).PadLeft(2, '0')}";
                return shamsiTarikh;

            }
               


         

        }

        public static DateTime ShamsiToMiladi(string shamsiTarikh)
        {

            var p = new PersianCalendar();
            var a = shamsiTarikh.Split("/");
            var b = p.ToDateTime(short.Parse(a[0]), short.Parse(a[1]), short.Parse(a[2]), 0, 0, 0, 0);
               
            return b;

        }

        public static string TimeTickToMiladi(long ticks)
        {

            var myDate = new DateTime(ticks);

            return myDate.ToString("MM/dd/yyyy");

        }

        public static string TimeTickToShamsi(long ticks)
        {

            var myDate = new DateTime(ticks);


            return MiladiToShamsi(myDate);

        }

        public static long ShamsiToTimeTick(string shamsiTarikh)
        {
            var a = shamsiTarikh.Split("/");
            var pc = new PersianCalendar();
            var thisDate = pc.ToDateTime(short.Parse(a[0]), short.Parse(a[1]), short.Parse(a[2]), 0, 0, 0, 0);
            return thisDate.Ticks;

        }

        public static long? MiladiToTimeTick(DateTime? miladiTarikh)
        {
            return miladiTarikh?.Ticks;
        }

        public static string AddDayToShamsi(int day, string shamsiTarikh)
        {

            var p = new PersianCalendar();
            var a = shamsiTarikh.Split("/");
            var b = p.ToDateTime(short.Parse(a[0]), short.Parse(a[1]), short.Parse(a[2]), 0, 0, 0, 0);
            b = b.AddDays(day);
            return MiladiToShamsi(b);

        }

        public static long TwoShamsiDateDiffAsDay(string shamsiBegin, string shamsiEnd)

        {
            var p1 = new PersianCalendar();
            var a1 = shamsiBegin.Split("/");
            var b1 = p1.ToDateTime(short.Parse(a1[0]), short.Parse(a1[1]), short.Parse(a1[2]), 0, 0, 0, 0);

            var p2 = new PersianCalendar();
            var a2 = shamsiEnd.Split("/");
            var b2 = p2.ToDateTime(short.Parse(a2[0]), short.Parse(a2[1]), short.Parse(a2[2]), 0, 0, 0, 0);

            var span = b2 - b1;
            return span.Days;

        }




    }
}
