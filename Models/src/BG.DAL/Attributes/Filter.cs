using BG.Infrastructure.Morphology;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.DAL.Attributes
{
    public enum FilterType {
        None,
        [Description("Склонение")]
        PersonDeclension,
        [Description("Склонение")]
        AppointmentDeclension,
        [Description("Имя_Отчество")]
        OnlyPersonName,
        [Description("Обращение")]
        Welcome }

    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class FilterAttribute : Attribute
    {
        public FilterAttribute(FilterType type)
        {
            Type = type;
        }
        public FilterAttribute(FilterType type, DeclensionCase declensionCase) : this(type)
        {
            Declension = declensionCase;
        }

        public FilterType Type { get; }
        public DeclensionCase Declension { set; get; }
    }
}
