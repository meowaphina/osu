// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Bindables;
using Expression = NCalc.Expression;

namespace osu.Game.Graphics.UserInterfaceV2.Expressional
{
    public abstract class Expressional<T> : IExpressional<T>
    {
        public Bindable<Expression> Expression { get; } = new Bindable<Expression>();
        public Bindable<string> Raw { get; } = new Bindable<string>();

        private readonly ExpressionalFeature[] features;

        protected Expressional(
            string? defaultExpression = null,
            params ExpressionalFeature[] features)
        {
            this.features = features;

            if (defaultExpression != null)
                Parse(defaultExpression!);
        }

        public T Express()
        {
            T? result = expression()
                .Evaluate<T>();

            return result ?? expressAndCoalesce();
        }

        private Expression expression() => Expression.Value;

        private T expressAndCoalesce()
        {
            object? result = expression()
                .Evaluate();

            EnsureCoalescent(result);

            return Coalesce(result!);
        }

        public bool CanCoalesce(object? result) => result != null;

        protected void EnsureCoalescent(object? result)
        {
            if (!CanCoalesce(result))
            {
                throw new InvalidOperationException(
                    """
                    Expression failed to coalesce to requested type due to result being null.
                    """);
            }
        }

        public void Parse(string contents) =>
            Update(createExpression(contents), contents);

        public void Update(Expression expression, string contents)
        {
            Expression.Value = expression;
            Raw.Value = contents;
        }

        private Expression createExpression(string contents)
        {
            Expression expression = new Expression(contents);

            populateFeatures(expression);

            return expression;
        }

        private void populateFeatures(Expression expression)
        {
            foreach (ExpressionalFeature feature in features)
                feature.Populate(expression);
        }

        public abstract T Coalesce(object? result);

        public abstract class Async : Expressional<T>, IAsyncExpressional<T>
        {
            public Task<T> ExpressAsync(CancellationToken cancellationToken = default) => expressAsync(cancellationToken);

            private async Task<T> expressAsync(CancellationToken cancellationToken)
            {
                T? result = await expression()
                                  .EvaluateAsync<T>(cancellationToken)
                                  .ConfigureAwait(false);

                return result ?? await expressAndCoalesce(cancellationToken)
                    .ConfigureAwait(false);
            }

            private async Task<T> expressAndCoalesce(CancellationToken cancellationToken = default)
            {
                object? result = await expression()
                                       .EvaluateAsync(cancellationToken)
                                       .ConfigureAwait(false);

                EnsureCoalescent(result);

                return Coalesce(result!);
            }
        }
    }
}
