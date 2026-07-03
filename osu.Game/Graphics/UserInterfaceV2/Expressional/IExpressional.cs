// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NCalc;
using osu.Framework.Bindables;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public interface IExpressional<out T>
    {
        Bindable<Expression> Expression { get; }

        T Express();

        T Coalesce(object? result);
    }
}
