using System;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.DSL
{
    //public static class Should
    //{
    //    public static readonly IConstraint NotBeNullOrEmpty = new StringNotNullOrEmptyConstraint();
    //    public static readonly IConstraint NotBeNull = new NotNullConstraint();
    //    public static readonly IConstraint BeNull = new NullConstraint();
    //    public static readonly IConstraint BeTrue = new IsTrueConstraint();
    //    public static readonly IConstraint PersonNotBeDuplicated = new PersonNotBeDuplicatedConstraint();

    //    public static readonly IConstraint BeRussiaINNStandart1 = new RussiaINNStandart1Constraint();
    //    public static readonly IConstraint BeBelorussiaINNStandart1 = new BelorussiaINNStandart1Constraint();

    //    public static readonly IConstraint BeSnils = new StringRegExConstraint("\\d{11}");

    //    public static IConstraint NotBeLongerThan(int maxLength)
    //    {
    //        return new StringMaxLengthConstraint(maxLength);
    //    }

    //    public static IConstraint DefaultINNStandartForCountry(int country)
    //    {
    //        return new DefaultINNStandartForCountry(country);
    //    }

    //    public static IConstraint BeConstraintFrom(Func<object, bool> satisfied)
    //    {
    //        return new Constraint(satisfied);
    //    }

    //}

    //public class Constraint : IConstraint
    //{
    //    readonly Func<object, bool> _satisfied;
    //    public Constraint(Func<object, bool> satisfied)
    //    {
    //        _satisfied = satisfied;
    //    }

    //    public bool SatisfiedBy(object value)
    //    {
    //        Guard.Against<ArgumentNullException>(_satisfied == null, "Ошибка проверки условия: не задано выражение проверки");

    //        return _satisfied(value);
    //    }
    //}
}
