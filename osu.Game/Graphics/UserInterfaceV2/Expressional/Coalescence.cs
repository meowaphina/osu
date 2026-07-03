// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public static class Coalescence
    {
        public static Func<object?, T> Cast<T>() =>
            result => result is T value
                ? value
                : throw new InvalidCastException($"Cannot cast '{result?.GetType().Name ?? "null"}' to '{typeof(T).Name}'.");
    }
}
