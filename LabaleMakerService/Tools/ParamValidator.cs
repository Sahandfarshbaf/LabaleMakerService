using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LabaleMakerService.Tools

{
    public class ParamValidator
    {
        private List<string> errorList { get; set; }

        public ParamValidator()
        {
            errorList = new List<string>();
        }

        public ParamValidator Add(string message)
        {
            if (!string.IsNullOrEmpty(message))
                errorList.Add(message);
            return this;
        }

        public ParamValidator Clear()
        {
            errorList = new List<string>();
            return this;
        }

        //public ParamValidator Throw(int errorCode)
        //{
        //    if (errorList.Count != 0)
        //    {
        //      //  throw new BusinessException(new XError(errorCode, string.Join(Environment.NewLine, errorList)));
        //    }
        //    else
        //        return this;
        //}

        #region Project Custom Validations

        public ParamValidator ValidateShamsiDate(string date, string fieldName)
        {
            string message = fieldName + " به درستی ثبت نشده است ";
            if (string.IsNullOrEmpty(date) || date.Length != 10) return this;
            else
            {
                int year = Convert.ToInt32(date.Substring(0, 4));
                int month = Convert.ToInt32(date.Substring(5, 2));
                int day = Convert.ToInt32(date.Substring(8, 2));

                if (year > 1499 || month > 12 || day > 31) errorList.Add(message);

            }

            return this;
        }

        public ParamValidator ValidateCarTag(long? freeZoneId, long? freeZoneTag, long? freeZoneTwoDigit, long? irTagPart1, long? irTagPart2, long? irTagCharacter, long? irTagPart3)
        {
            var message = "شماره پلاک ناوگان صحیح نیست";
            var freeZoneFieldCount = 0;
            var irFieldCount = 0;
            var motorFieldCount = 0;

            freeZoneFieldCount += freeZoneId.HasValue ? 1 : 0;
            freeZoneFieldCount += freeZoneTag.HasValue ? 1 : 0;

            irFieldCount += irTagPart1.HasValue ? 1 : 0;
            irFieldCount += irTagPart2.HasValue ? 1 : 0;
            irFieldCount += irTagCharacter.HasValue ? 1 : 0;
            irFieldCount += irTagPart3.HasValue ? 1 : 0;

            motorFieldCount += irTagPart2.HasValue ? 1 : 0;
            motorFieldCount += irTagPart3.HasValue ? 1 : 0;


            if (freeZoneFieldCount == 0)
            {
                if ((motorFieldCount == 2 || irFieldCount == 4))
                {

                }
                else
                {

                    errorList.Add(message);
                    return this;
                }

                if ((!(irTagPart1 < 10) && !(irTagPart1 > 99) && !(irTagPart2 < 10) && !(irTagPart2 > 99) &&
                     !(irTagCharacter < 1) && !(irTagCharacter > 32) && !(irTagPart3 < 100) && !(irTagPart3 > 999)) ||
                    irFieldCount != 4) return this;
                errorList.Add(message);
                return this;
            }
            else if (irFieldCount == 0)
            {
                if (freeZoneFieldCount != 2)
                {
                    errorList.Add(message);
                    return this;
                }
                else
                {
                    if (((!(freeZoneId < 1) && !(freeZoneId > 7)) || freeZoneId == 99) && !(freeZoneTag < 10000) &&
                        !(freeZoneTag > 99999) &&
                        (!freeZoneTwoDigit.HasValue || (!(freeZoneTwoDigit < 10) && !(freeZoneTwoDigit > 99))))
                        return this;
                    errorList.Add(message);
                    return this;
                }
            }
            else
            {
                errorList.Add(message);
                return this;
            }
            
        }



        #endregion


        #region General Validations
        public ParamValidator ValidateRegex(string obj, string regex, string message)
        {
            if (string.IsNullOrEmpty(obj) || string.IsNullOrWhiteSpace(obj)) return this;
            if (!Regex.IsMatch(obj, regex)) errorList.Add(message);

            return this;
        }

        public ParamValidator ValidateLength(string obj, int minLength, int maxLength, string message)
        {
            if (obj.Length < minLength || obj.Length > maxLength)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateLength(long obj, long min, long max, string message)
        {
            if (obj < min || obj > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateLength(IList objList, long min, long max, string message)
        {
            if (objList.Count < min || objList.Count > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateRange(DateTime obj, DateTime min, DateTime max, string message)
        {
            if (obj < min || obj > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateMaxRange(DateTime obj, DateTime max, string message)
        {
            if (obj > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateMinRange(DateTime obj, DateTime min, string message)
        {
            if (obj < min)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateEmail(string obj, string message = "فیلد ایمیل نامعتبر است")
        {
            if (string.IsNullOrEmpty(obj) || string.IsNullOrWhiteSpace(obj))
                return this;
            try
            {
                System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(obj);
            }
            catch (Exception)
            {
                errorList.Add(message);
            }
            return this;
        }

        public ParamValidator ValidateMobile(long obj, string message = "فیلد شماره موبایل نامعتبر است")
        {
            if (obj < 9000000000 || obj >= 10000000000)
            {
                errorList.Add(message);
            }
            return this;
        }

        public ParamValidator ValidateMobileWithZero(string obj, string message = "فیلد شماره موبایل نامعتبر است")
        {
            if (obj.Length != 11)
            {
                errorList.Add(message);
            }
            if (!obj.Substring(0, 2).Equals("09"))
            {
                errorList.Add(message);
            }
            return this;
        }
        public ParamValidator ValidateMobileWithOutZero(string obj, string message = "فیلد شماره موبایل نامعتبر است")
        {
            if (obj.Length != 10)
            {
                errorList.Add(message);
            }
            if (!obj.Substring(0, 1).Equals("9"))
            {
                errorList.Add(message);
            }
            return this;
        }
        public ParamValidator ValidateTelPhoneNumber(long obj, string message = "شماره تلفن ثابت نامعتبر است") {

            if (obj.ToString().Length != 10)
                errorList.Add(message);
            return this;
        }
        #endregion


        #region Null Validations

        public ParamValidator ValidateNull(int? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(long? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(byte? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(DateTime? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(string obj, string message)
        {
            if (string.IsNullOrEmpty(obj) || string.IsNullOrWhiteSpace(obj))
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(IList objList, string message)
        {
            if (objList == null || objList.Count == 0)
                errorList.Add(message);
            return this;
        }

        #endregion


    }
}
