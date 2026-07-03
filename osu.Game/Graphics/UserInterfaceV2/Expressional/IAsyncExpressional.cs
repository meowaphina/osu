// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Threading;
using System.Threading.Tasks;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public interface IAsyncExpressional<T>
    {
        Task<T> ExpressAsync(CancellationToken cancellationToken = default);
    }
}
