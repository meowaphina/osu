// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NCalc;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public partial class ExpressionalFeature(Action<Expression> populator)
    {
        public virtual void Populate(Expression expression) => populator(expression);

        public static ExpressionalFeature Constant<T>(string identifier, T value) =>
            new ExpressionalFeature(expression =>
                expression.Parameters[identifier] = value
            );

        public static ExpressionalFeature Constant<T>(T value, params string[] identifiers) =>
            new ExpressionalFeature(expression =>
                identifiers
                    .ToList()
                    .ForEach(i => expression.Parameters[i] = value)
            );

        public static ExpressionalFeature Variable<T>(string identifier, Func<T> supplier) =>
            new ExpressionalFeature(expression =>
                expression.DynamicParameters[identifier] = _ => supplier()
            );

        public static ExpressionalFeature Variable<T>(Func<T> supplier, params string[] identifiers) =>
            new ExpressionalFeature(expression =>
                identifiers
                    .ToList()
                    .ForEach(i => expression.DynamicParameters[i] = _ => supplier())
            );
    }
}
