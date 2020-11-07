using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Extensions
{
    public static class MonadicExtensions
    {
        #region With
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }
        #endregion

        #region Return

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TInput : class
        {
            return Return(o, evaluator, default(TResult));
        }


        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            if (o == null) return failureValue;
            /* !!! временно закоммичено. Для json.net
            if (o.GetType() == typeof(long))
            {
                long l = Convert.ToInt64(o);
                if (l >= int.MinValue && l <= int.MaxValue)
                {
                    o = (TInput)(object)(Convert.ToInt32(o));
                }
            }*/
            
            return evaluator(o);
        }
        #endregion

        #region If
        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator) where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }
        #endregion

        #region If
        public static TInput IfThen<TInput>(this TInput o, Func<TInput, bool> evaluator, Action<TInput> action) where TInput : class
        {
            if (o == null) return null;

            if (evaluator(o))
                action(o);

            return o;
        }
        #endregion

        #region Unless
        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator) where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }
        #endregion

        #region Do
        public static TInput Do<TInput>(this TInput o, Action<TInput> action)  where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }
        #endregion

        #region Do
        public static TInput Do<TInput>(this TInput o, Func<TInput, bool> evaluator, Action<TInput> action) where TInput : class
        {
            if (o == null) return null;
            if(evaluator(o)) action(o);
            return o;
        }
        #endregion
    }
}
