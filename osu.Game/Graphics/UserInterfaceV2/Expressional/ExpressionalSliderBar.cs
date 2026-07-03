// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Numerics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public partial class ExpressionalSliderBar<T> : FormSliderBar<T>
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        public NumberExpression<T> Expression { get; } = new NumberExpression<T>();

        protected override T? InternallyUpdate(string contents)
        {
            try
            {
                Expression.Parse(contents);

                return Expression
                    .Express();
            }
            catch
            {
                return null;
            }
        }

        internal override FormTextBox.InnerTextBox CreateTextBox(FormControlBackground background, CompositeDrawable? tabbableContentContainer) =>
            new FormTextBox.InnerTextBox
            {
                RelativeSizeAxes = Axes.X,

                // the textbox is hidden when the control is unfocused,
                // but clicking on the label should reach the textbox,
                // therefore make it always present.
                AlwaysPresent = true,
                CommitOnFocusLost = true,
                SelectAllOnFocus = true,
                OnInputError = background.FlashOnInputError,
                TabbableContentContainer = tabbableContentContainer,
            };
    }
}
