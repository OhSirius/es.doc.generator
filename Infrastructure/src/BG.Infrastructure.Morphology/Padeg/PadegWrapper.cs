using BLL.Declension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropPadeg
{
    public class PadegWrapper
    {

        public string ConvertRoditelnyiCaseName(string obj)
        {
            //Guard.Against<ArgumentException>(obj.GetType() != typeof(string), "Ошибка операции конвертации падежей: тип сущности не является типом string");
            string name = DeclensionBLL.GetSNPDeclension("", (string)obj, "", DeclensionCase.Rodit);
            return name;
        }

        public string ConvertRoditelnyiCaseSurname(string obj)
        {
            //Guard.Against<ArgumentException>(obj.GetType() != typeof(string), "Ошибка операции конвертации падежей: тип сущности не является типом string");
            string name = DeclensionBLL.GetSNPDeclension((string)obj, "", "", DeclensionCase.Rodit);
            return name;
        }

        public string ConvertToRoditelnyiCasePatronymic(string obj)
        {
            //Guard.Against<ArgumentException>(obj.GetType() != typeof(string), "Ошибка операции конвертации падежей: тип сущности не является типом string");

            string name = DeclensionBLL.GetSNPDeclension("", "", (string)obj, DeclensionCase.Rodit);
            return name;
        }

        //public string ConvertToRoditelnyiCaseTotalName(string obj)
        //{
        //    //Guard.Against<ArgumentException>(obj.GetType() != typeof(string), "Ошибка операции конвертации падежей: тип сущности не является типом string");

        //    string name = null;
        //    string surname = null;
        //    string patronymic = null;
        //    DeclensionBLL.GetSNM((string)obj, out surname, out name, out patronymic);

        //    string totalName = DeclensionBLL.GetSNPDeclension(surname, name, patronymic, DeclensionCase.Rodit);
        //    return totalName;
        //}

        public string ConvertToRoditelnyiCaseApproiment(string obj)
        {
            //Guard.Against<ArgumentException>(obj.GetType() != typeof(string), "Ошибка операции конвертации падежей: тип сущности не является типом string");
            string name = DeclensionBLL.GetAppointmentDeclension((string)obj, DeclensionCase.Rodit);
            return name;
        }

        public string ConvertFIOInitialse(string obj)
        {
            //Guard.Against<ArgumentException>(obj.GetType() != typeof(string), "Ошибка операции конвертации инициалов: тип сущности не является типом string");
            string name = null;
            string surname = null;
            string patronymic = null;
            DeclensionBLL.GetSNM((string)obj, out surname, out name, out patronymic);
            string InitialseName = name.Substring(0, 1).ToUpper() + ".";
            string InitialsePatronymic = patronymic.Substring(0, 1).ToUpper() + ".";
            string totalName = surname + " " + InitialseName + " " + InitialsePatronymic;
            return totalName;
        }

        public string ConvertToRoditelnyiCaseTotalName(string obj)
        {
            //Guard.Against<ArgumentException>(obj.GetType() != typeof(string), "Ошибка операции конвертации падежей: тип сущности не является типом string");
            string name = null;
            string surname = null;
            string patronymic = null;
            DeclensionBLL.GetSNM((string)obj, out surname, out name, out patronymic);
            string totalName = DeclensionBLL.GetSNPDeclension(surname, name, patronymic, DeclensionCase.Rodit);
            return totalName;
        }

    }
}
