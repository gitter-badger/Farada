﻿using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider.Keys;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class ValueProviderDictionary
  {
    private readonly Dictionary<IKey, ValueProviderLink> _valueProviders;

    public ValueProviderDictionary ()
    {
      _valueProviders = new Dictionary<IKey, ValueProviderLink>();
    }

    internal void AddValueProvider (IKey key, IValueProvider valueProvider)
    {
      if (!_valueProviders.ContainsKey(key))
      {
        _valueProviders[key] = new ValueProviderLink(valueProvider, key, () => GetLink(key.PreviousKey));
      }
      else
      {
        var previousKey = _valueProviders[key];
        _valueProviders[key] = new ValueProviderLink(valueProvider, key, () => previousKey);
      }
    }

    private ValueProviderLink GetOrDefault (IKey key)
    {
      return key!=null&&_valueProviders.ContainsKey(key) ? _valueProviders[key] : null;
    }

    internal ValueProviderLink GetLink (IKey key)
    {
      ValueProviderLink link = null;
      while (link == null && key != null)
      {
        link = GetOrDefault(key);
      
        // TODO: Differentiate fixed and subtype value providers
        key = key.PreviousKey;
      }

      return link;
    }
  }

  internal class ValueProviderLink
  {
    internal IValueProvider Value { get; private set; }
    internal IKey Key { get; private set; }
    internal Func<ValueProviderLink> Previous { get; private set; }

    internal ValueProviderLink (IValueProvider value, IKey key, Func<ValueProviderLink> previous)
    {
      Value = value;
      Key = key;
      Previous = previous;
    }
  }
}