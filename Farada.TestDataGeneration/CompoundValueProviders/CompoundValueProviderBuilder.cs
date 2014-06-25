﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class CompoundValueProviderBuilder : ICompoundValueProviderBuilder
  {
    private readonly Random _random;
    private readonly ValueProviderDictionary _valueProviderDictionary;
    private readonly IList<IInstanceModifier> _modifierList;

    internal CompoundValueProviderBuilder(Random random)
    {
      _random = random;
      _valueProviderDictionary = new ValueProviderDictionary();
      _modifierList = new List<IInstanceModifier>();
    }

    public void AddProvider<TProperty, TAttribute, TContainer, TContext> (
        Expression<Func<TContainer, TAttribute, TProperty>> chainExpression,
        AttributeBasedValueProvider<TProperty, TAttribute, TContext> attributeBasedValueProvider) where TAttribute : Attribute where TContext : IValueProviderContext
    {
      if(chainExpression.ToChain().Any())
      {
        throw new ArgumentException("Expression chain is not supported for attributes at the moment");
      }

      var key = new AttributeKey(typeof (TProperty), typeof (TAttribute));
      _valueProviderDictionary.AddValueProvider(key, attributeBasedValueProvider);
    }

    public void AddProvider<TProperty, TContainer, TContext> (Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty, TContext> valueProvider) where TContext : ValueProviderContext<TProperty>
    {
      var declaringType = chainExpression.GetParameterType();
      var expressionChain = chainExpression.ToChain().ToList();

      IKey key = expressionChain.Count == 0 ? (IKey) new TypeKey(typeof (TProperty)) : new ChainedKey(declaringType, expressionChain);
      _valueProviderDictionary.AddValueProvider(key, valueProvider);
    }

    public void AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _modifierList.Add(instanceModifier);
    }

    internal CompoundValueProvider Build()
    {
      return new CompoundValueProvider(_valueProviderDictionary, _random, _modifierList);
    }
  }
}