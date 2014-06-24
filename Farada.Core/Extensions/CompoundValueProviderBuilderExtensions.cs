﻿using System;
using System.Linq.Expressions;
using Farada.Core.BaseDomain.ValueProviders;
using Farada.Core.CompoundValueProvider;
using Farada.Core.ValueProvider;

namespace Farada.Core.Extensions
{
  /// <summary>
  /// TODO
  /// </summary>
  public static class CompoundValueProviderBuilderExtensions
  {
    public static void AddProvider<TProperty, TAttribute, TContainer> (this ICompoundValueProviderBuilder builder,  Expression<Func<TContainer, TAttribute, TProperty>> chainExpression, Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> attributeValueProviderFunc) where TAttribute : Attribute
    {
      builder.AddProvider(chainExpression, new FuncProviderForAttributeBased<TProperty, TAttribute>(attributeValueProviderFunc));
    }

    public static void AddProvider<TProperty, TContainer> (this ICompoundValueProviderBuilder builder, Expression<Func<TContainer, TProperty>> chainExpression, Func<ValueProviderContext<TProperty>, TProperty> valueProviderFunc)
    {
      builder.AddProvider(chainExpression, new FuncProvider<TProperty>(valueProviderFunc));
    }
  }
}