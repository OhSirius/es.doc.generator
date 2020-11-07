using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Morphology.Padeg
{
    public class Declension : IDeclension
    {
        static object _locker = new object();

        public DeclensionGender DefineByName(string FIO)
        {
            lock (_locker)
            {
                return new BG.Infrastructure.Morphology.Native.Declension().DefineDeclensionGender(FIO);
            }
        }

        public string GetApproimentDeclension(string approiment, DeclensionCase declensionCase)
        {
            lock (_locker)
                return BLL.Declension.DeclensionBLL.GetAppointmentDeclension(approiment, (BLL.Declension.DeclensionCase)(int)declensionCase);
        }

        public string GetPersonNameBy(string FIO)
        {
            lock (_locker)
            {
                string name = null;
                string surname = null;
                string patronymic = null;
                BLL.Declension.DeclensionBLL.GetSNM(FIO, out surname, out name, out patronymic);
                string InitialseName = name.Substring(0, 1).ToUpper() + ".";
                string InitialsePatronymic = patronymic.Substring(0, 1).ToUpper() + ".";
                string totalName = name + " " + patronymic;
                return totalName;
            }
        }

        public string GetPersonNameDeclension(string FIO, DeclensionCase declensionCase)
        {
            lock (_locker)
            {
                string name = null;
                string surname = null;
                string patronymic = null;
                BLL.Declension.DeclensionBLL.GetSNM(FIO, out surname, out name, out patronymic);
                string totalName = BLL.Declension.DeclensionBLL.GetSNPDeclension(surname, name, patronymic, (BLL.Declension.DeclensionCase)(int)declensionCase);
                return totalName;
            }
        }

        public string GetPersonNameDeclension(string FIO, DeclensionCase declensionCase, DeclensionGender gender, string part, int z5)
        {
            throw new NotImplementedException();
        }
    }
}
