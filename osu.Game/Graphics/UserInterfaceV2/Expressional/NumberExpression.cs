// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Numerics;
using AutoMapper.Internal;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public partial class NumberExpression<T> : Expressional<T>
        where T : INumber<T>
    {
        public static ExpressionalFeature[] Features =>
        [
            ..Constants
        ];

        public static ExpressionalFeature[] Constants =>
        [
            ExpressionalFeature.Constant(Math.PI, "pi", "π"),
            ExpressionalFeature.Constant(Math.Tau, "tau", "τ"),
            ExpressionalFeature.Constant(Math.E, "e"),
            ExpressionalFeature.Constant((1 + Math.Sqrt(5)) / 2, "phi", "ϕ", "φ"),
            ExpressionalFeature.Constant(Math.Sqrt(2), "sqrt2", "√2"),
            ExpressionalFeature.Constant(Math.Sqrt(3), "sqrt3", "√3"),

            ExpressionalFeature.Constant(Math.PI / 180d, "deg"),
            ExpressionalFeature.Constant(180d / Math.PI, "rad"),

            ExpressionalFeature.Constant(512d, "playfieldWidth"),
            ExpressionalFeature.Constant(384d, "playfieldHeight"),
            ExpressionalFeature.Constant(256d, "centerX"),
            ExpressionalFeature.Constant(192d, "centerY"),
        ];

        private readonly Func<object?, T> coalesce;

        public NumberExpression(string? defaultExpression = null,
                                Func<object?, T>? coalesce = null,
                                ExpressionalFeature[]? features = null)
            : base(defaultExpression,
                Features.Concat(features ?? []).ToArray()
            )
        {
            this.coalesce = coalesce ?? Expressional.Coalesce.Cast<T>();
        }

        public override T Coalesce(object? result) => coalesce(result);

        /// <summary>
        /// Unsure if this is necessary but im keeping it for future
        /// </summary>
        public class Float() : NumberExpression<float>();

        public class Double() : NumberExpression<double>();

        public class Integer() : NumberExpression<int>();
    }
}
