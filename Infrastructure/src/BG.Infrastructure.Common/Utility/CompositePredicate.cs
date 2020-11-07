namespace BG.Infrastructure.Common.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CompositePredicate<T>
    {
        private readonly List<Predicate<T>> list = new List<Predicate<T>>();

        private Predicate<T> matchesAll = x => true;

        private Predicate<T> matchesAny = x => true;

        private Predicate<T> matchesNone = x => false;

        public static CompositePredicate<T> operator +(CompositePredicate<T> predicate, Predicate<T> filter)
        {
            Guard.AssertNotNull(predicate, "predicate");
            Guard.AssertNotNull(filter, "filter");

            predicate.Add(filter);
            return predicate;
        }

        public void Add(Predicate<T> filter)
        {
            Guard.AssertNotNull(filter, "filter");

            this.matchesAll = x => this.list.All(predicate => predicate(x));
            this.matchesAny = x => this.list.Any(predicate => predicate(x));
            this.matchesNone = x => !this.MatchesAny(x);

            this.list.Add(filter);
        }

        public bool MatchesAll(T target)
        {
            return this.matchesAll(target);
        }

        public bool MatchesAny(T target)
        {
            return this.matchesAny(target);
        }

        public bool MatchesNone(T target)
        {
            return this.matchesNone(target);
        }

        public bool DoesNotMatchAny(T target)
        {
            return this.list.Count == 0 || !this.MatchesAny(target);
        }
    }
}
