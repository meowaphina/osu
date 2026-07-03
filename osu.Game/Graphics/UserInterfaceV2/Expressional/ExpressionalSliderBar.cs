// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using System.Numerics;
using AutoMapper.Internal;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public partial class ExpressionalSliderBar<T> : FormSliderBar<T>
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        public ExpressionalFeature[] Features =>
        [
            ExpressionalFeature.Variable(() => Previous, "previous", "Previous")
        ];

        public NumberExpression<T> Expression { get; }
        public T Previous { get; private set; }

        public ExpressionalSliderBar(params ExpressionalFeature[] features)
        {
            Expression = new NumberExpression<T>(null, null,
                features
                    .Concat(Features)
                    .ToArray());

            Previous = Current.Value;

            Current.ValueChanged += @event =>
            {
                Previous = @event.OldValue;
            };
        }

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
