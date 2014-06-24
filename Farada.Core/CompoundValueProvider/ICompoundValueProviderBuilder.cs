﻿using System;
using System.Linq.Expressions;
using Farada.Core.Modifiers;
using Farada.Core.ValueProvider;

namespace Farada.Core.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  public interface ICompoundValueProviderBuilder
  {
    /// <summary>
    /// TODO
    /// </summary>
    void AddProvider<TProperty, TAttribute, TContainer, TContext> (
        Expression<Func<TContainer, TAttribute, TProperty>> chainExpression,
        AttributeBasedValueProvider<TProperty, TAttribute, TContext> attributeBasedValueProvider) where TAttribute : Attribute where TContext : IValueProviderContext;

    /// <summary>
    /// TODO
    /// </summary>
    void AddProvider<TProperty, TContainer, TContext> (Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty, TContext> valueProvider) where TContext : ValueProviderContext<TProperty>;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="instanceModifier"></param>
    void AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}