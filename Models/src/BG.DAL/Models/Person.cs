using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.DAL.Attributes;
using BG.DAL.Interfaces;
using BG.Infrastructure.Morphology;

namespace BG.DAL.Models
{
    public class Person : IDisplayEntity
    {
        //ФИО
        [Filter(FilterType.PersonDeclension, DeclensionCase.Datel)]
        [Filter(FilterType.PersonDeclension, DeclensionCase.Imenit)]
        [Filter(FilterType.OnlyPersonName)]
        [Filter(FilterType.Welcome)]
        [Template("<%ФИО клиента%>")]
        [Source("ФИО клиента")]
        public string TotalName { set; get; }

        //Должность
        [Template("<%Должность%>")]
        [Filter(FilterType.AppointmentDeclension, DeclensionCase.Datel)]
        [Source("Должность")]
        public string Position { set; get; }

        //Название организации
        [Template("<%Организация%>")]
        [Source("Организация")]
        public string CompanyName { set; get; }

        //Сфера деятельности организации
        [Template("<%Деятельность%>")]
        [Source("Деятельность")]
        public string Domain { set; get; }

        [Template("<%ФИО менеджера%>")]
        [Filter(FilterType.PersonDeclension, DeclensionCase.Datel)]
        //[Filter(FilterType.None)]
        [Filter(FilterType.PersonDeclension, DeclensionCase.Imenit)]
        [Source("ФИО менеджера")]
        public string ManagerTotalName { set; get; }

        [Template("<%Телефон менеджера%>")]
        [Source("Телефон менеджера")]
        public string ManagerPhone { set; get; }

        [Template("<%e-mail менеджера%>")]
        [Source("e-mail менеджера")]
        public string ManagerEmail{ set; get; }

        [Template("<%Исх.номер%>")]
        [Source("Исх.номер")]
        public string Number { set; get; }

        [Template("<%Дата%>")]
        [Source("Дата")]
        public string Date { set; get; }

        public string Display => $"{CompanyName}";
    }
}
