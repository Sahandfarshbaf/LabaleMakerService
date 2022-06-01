namespace Sina_Bp.Tools
{
    public class Sms
    {
        public static string SuccessDealInsert(string trackingCode,string fullName,string brand, string model,string tip)
        {
            var text = " آقای /خانم ";
            text += fullName;
            text += System.Environment.NewLine;
            text += $"با سلام ، معامله مربوط به خودروی {brand} {model} {tip} ";
            text += $"درسامانه معاملات خودرو با کد رهگیری {trackingCode} ثبت شده است";
            text += System.Environment.NewLine;
            text += "هنگام مراجعه به مراکز تعویض پلاک ارائه کد رهگیری الزامی است .";
            text += "جهت بررسی صحت اطلاعات به آدرس Khodro.ntsw.ir مراجعه کنید.";
            return text;
        }
    }
}
