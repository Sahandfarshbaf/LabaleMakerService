namespace Logger
{
    public class XError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public XError() { }
        public XError(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static XError GeneralError => new XError(1, "خطا در سامانه");

        public class AuthenticationErrors_
        { // starts from 2000
            public static XError UserNameOrPasswordIsWrong() => new XError(2001, "نام کاربری یا کلمه عبور صحیح نیست.");
            public static XError NeedLoginAgain() => new XError(2002, "لطفا جهت ادامه کار مجددا وارد شوید");
            public static XError NotHaveRequestedRole() => new XError(2003, "نقش انتخابی جزو نقش های شما نمی باشد");
            public static XError PasswordIsWrong() => new XError(2004, " کلمه عبور صحیح نیست.");

        }

        public class GetDataErrors_
        { // starts from 3000
            public static XError NotFound() => new XError(3000, "رکورد مورد نظر یافت نشد");
            public static XError FileNotFound() => new XError(3001, "،فایل مورد نظر یافت نشد");
            public static XError InvalidPageNumber() => new XError(3002, "شماره صفحه باید بزرگتر از 0 باشد.");
            public static XError InvalidPageSize() => new XError(3003, "اندازه صفحه باید بزرگتر از 0 باشد.");


        }

        public class InsertDataErrors_
        { // starts from 3000
            public static XError AlreadyExist() => new XError(1000, "رکورد باطالاعات وارد شده در سیستم وجود دارد.");


        }

        public class BusinessErrors_
        { // starts from 4000
            public static XError FileMaxSizeReach() => new XError(4000, "حجم فایل بیش از مقدار مجاز می باشد.");
            public static XError PDFFileMaxSize() => new XError(4001, "حجم فایل بیش از مقدار مجاز می باشد.");
            public static XError UserAlreadyExists() => new XError(4002, "کاربر با مشخصات وارد شده در سامانه موجود می باشد");
            public static XError UserRoleNotSelected() => new XError(4003, "نقشی برای کاربر انتخاب نشده است");
            public static XError DocumnetMachineryNotUpload() => new XError(4004, "لطفا ابتداتمامی اطلاعات و فایل های درخواستی را ثبت و بارگذاری نمایید.");
            public static XError StatusMachinerNotComplateFields() => new XError(4005, "امکان تایید این دستگاه وجود ندارد لطفا ابتدا از قسمت «ویرایش»تمامی اطلاعات را ثبت و فایل های درخواستی بارگذاری گردد.");
            public static XError FileNotFound() => new XError(4006, "فایل یافت نشد.");
            public static XError MunipolicityNotSelected() => new XError(4007, "شهرداری برای کاربر انتخاب نشده است");
            public static XError RoleFormNotSelected() => new XError(4008, "فرمی برای نقش انتخاب نشده است");
            public static XError RoleAlreadyExists() => new XError(4009, "نقش با مشخصات وارد شده در سامانه موجود می باشد");
            public static XError MunicipalityNotFound() => new XError(4010, "شهرداری انتخابی یافت نشد.");
            public static XError ShahkarCheckFailed() => new XError(4011, "شماره موبایل وارد شده با کدملی مطابقت ندارد.");
            public static XError InvalidResetPasswordCode() => new XError(4012, "کد وارد شده صحیح نمی باشد.");
            public static XError FileCountShortage() => new XError(4013, "تمامی فایل های اجباری بارگزاری نشده است..");
            public static XError InputsCannotBeEmpty() => new XError(4014, "ورودی ها نمیتوانند خالی باشند");


            //
        }

        public class ServiceErrors_ {

            public static XError SabteAhvalError() => new XError(5001, "خطا در دریافت اطلاعات کد ملی");       
            public static XError SherkatHaError() => new XError(5002, "خطا در دریافت اطلاعات شناسه ملی");
            public static XError PostServiceError() => new XError(5003, "خطا در دریافت اطلاعات کدپستی");
            public static XError InquiryServiceError() => new XError(5003, "خطا در دریافت اطلاعات شرکت ها");
            public static XError SabteAhvalBirthDateError() => new XError(5004, "کدملی با تاریخ تولد مطابقت ندارد.");
            public static XError SherkatHaEstablishDateError() => new XError(5005, "شناسه ملی با تاریخ ثبت مطابقت ندارد.");
            public static XError SmsServiceConnectionError() => new XError(5006, "خطا در ارتباط با سرویس پیامک.");

            public static XError ServiceIsDown() { return new XError(1140, "سرویس شاهکار در دسترس نمی‌باشد.لطفا دقایقی بعد اقدام کنید."); }
            public static XError UnknownError() { return new XError(1141, "خطایی در سرویس شاهکار رخ داده است لطفا با پستیبانی تماس بگیرید."); }
            public static XError InqiryNotFound() { return new XError(1142, "شرکتی با این مشخصات یافت نشد"); }
            public static XError InqiryInformationMismatch() { return new XError(1143, "کدملی مدیر عامل با اطلاعات شرکت مطابقت ندارد."); }
            public static XError InqiryFailed() { return new XError(1144, "خطا در فراخوانی سرویس شرکت ها."); }
            public static XError AsnafNotFound() { return new XError(1145, "برای اطلاعات ارسالی اطلاعاتی یافت نشد."); }
            public static XError AsnafServiceError() { return new XError(1146, "سرویس اطلاعات اصناف در دسترس نمی باشد."); }
            public static XError AsnafNotValidLicenceFound() { return new XError(1146, "مجوز نمایشگاه معتبری یافت نشد."); }


        }
    }
}
