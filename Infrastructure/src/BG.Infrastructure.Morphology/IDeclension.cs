using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Morphology
{

    /// <summary>
    /// Род
    /// </summary>
    public enum DeclensionGender
    {
        /// <summary>
        /// Род неопределен
        /// </summary>
        None = 0,

        /// <summary>
        /// Род неопределен
        /// </summary>
        NotDefind = 3,

        /// <summary>
        /// Мужской род
        /// </summary>
        MasculineGender = 1,

        /// <summary>
        /// Женский род
        /// </summary>
        FeminineGender = 2
    }

    /// <summary>
    /// Падежи русского языка
    /// </summary>
    public enum DeclensionCase
    {
        /// <summary>
        /// Падеж не определен
        /// </summary>
        NotDefind = 0,

        /// <summary>
        /// Именительный падеж (Кто? Что?)
        /// </summary>
        [Description("Именительный")]
        Imenit = 1,

        /// <summary>
        /// Родительный падеж (Кого? Чего?)
        /// </summary>
        [Description("Родительный")]
        Rodit = 2,

        /// <summary>
        /// Дательный падеж (Кому? Чему?)
        /// </summary>
        [Description("Дательный")]
        Datel = 3,

        /// <summary>
        /// Винительный падеж (Кого? Что?)
        /// </summary>
        [Description("Винительный")]
        Vinit = 4,

        /// <summary>
        /// Творительный падеж (Кем? Чем?)
        /// </summary>
        [Description("Творительный")]
        Tvorit = 5,

        /// <summary>
        /// Предложный падеж (О ком? О чём?)
        /// </summary>
        [Description("Предложный")]
        Predl = 6
    }

    public interface IDeclension
    {
        string GetPersonNameDeclension(string FIO, DeclensionCase declensionCase);

        string GetPersonNameBy(string FIO);

        string GetPersonNameDeclension(string FIO, DeclensionCase declensionCase /*=2*/, DeclensionGender gender /*=3*/, /*Знач*/ string part /*="123"*/, int z5 /*=1*/);

        string GetApproimentDeclension(string approiment, DeclensionCase declensionCase);

        DeclensionGender DefineByName(string FIO);
    }
}
