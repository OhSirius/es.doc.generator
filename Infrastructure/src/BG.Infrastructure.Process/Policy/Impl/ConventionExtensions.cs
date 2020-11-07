using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.BusinessProcess.Policy.Impl
{
    ///<summary>
    /// Extension methods for <see cref="ISpecification{T}"/>.
    ///</summary>
    public static class ConventionExtensions
    {
        /// <summary>
        /// Retuns a new specification adding this one with the passed one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rightHand">The right hand.</param>
        /// <param name="leftHand">The left hand.</param>
        /// <returns></returns>
        public static IConvention<TEntity> And<TEntity>(this IConvention<TEntity> rightHand, IConvention<TEntity> leftHand)
        {
            return new ComposeConvention<TEntity>(rightHand, leftHand, ComposeType.And);
        }


        /// <summary>
        /// Retuns a new specification or'ing this one with the passed one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rightHand">The right hand.</param>
        /// <param name="leftHand">The left hand.</param>
        /// <returns></returns>
        public static IConvention<TEntity> Or<TEntity>(this IConvention<TEntity> rightHand, IConvention<TEntity> leftHand)
        {
            return new ComposeConvention<TEntity>(rightHand, leftHand, ComposeType.Or);
        }


        /// <summary>
        /// Retuns a new specification or'ing this one with the passed one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rightHand">The right hand.</param>
        /// <param name="leftHand">The left hand.</param>
        /// <returns></returns>
        public static IConvention<TEntity> Not<TEntity>(this IConvention<TEntity> convension)
        {
            return new NotConvention<TEntity>(convension);
        }
    }

    public static class StaticConventions
    {
        /// <summary>
        /// Retuns a new specification adding this one with the passed one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rightHand">The right hand.</param>
        /// <param name="leftHand">The left hand.</param>
        /// <returns></returns>

        public static IConvention<TEntity> And<TEntity>(IConvention<TEntity> rightHand, IConvention<TEntity> leftHand)
        {
            return new ComposeConvention<TEntity>(rightHand, leftHand, ComposeType.And);
        }

        /// <summary>
        /// Retuns a new specification or'ing this one with the passed one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rightHand">The right hand.</param>
        /// <param name="leftHand">The left hand.</param>
        /// <returns></returns>
        public static IConvention<TEntity> Or<TEntity>(IConvention<TEntity> rightHand, IConvention<TEntity> leftHand)
        {
            return new ComposeConvention<TEntity>(rightHand, leftHand, ComposeType.Or);
        }

        /// <summary>
        /// Retuns a new specification or'ing this one with the passed one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rightHand">The right hand.</param>
        /// <param name="leftHand">The left hand.</param>
        /// <returns></returns>
        public static IConvention<TEntity> Not<TEntity>(IConvention<TEntity> convension)
        {
            return new NotConvention<TEntity>(convension);
        }
    }

}
