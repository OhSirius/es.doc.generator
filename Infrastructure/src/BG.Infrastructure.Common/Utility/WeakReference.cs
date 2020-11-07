namespace BG.Infrastructure.Common.Utility
{
    using System;

    public class WeakReference<T>
    {
        private readonly Func<T> builder;
        private readonly WeakReference reference;

        public WeakReference(Func<T> builder)
        {
            this.builder = builder;
            this.reference = new WeakReference(this.builder());
        }

        public T Value
        {
            get
            {
                if (!this.reference.IsAlive)
                {
                    this.reference.Target = this.builder();
                }

                return (T)this.reference.Target;
            }
        }
    }
}
