﻿using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class ChainValueProvider:IChainValueProvider
  {
    private readonly RandomGeneratorProvider _randomGeneratorProvider;
    private IValueProvider _valueProvider;
    private readonly Dictionary<ChainKey, IChainValueProvider> _nextProviders;

    public ChainValueProvider(RandomGeneratorProvider randomGeneratorProvider, IValueProvider valueProvider=null)
    {
      _nextProviders = new Dictionary<ChainKey, IChainValueProvider>(new ChainKeyComparer());
      _randomGeneratorProvider = randomGeneratorProvider;
      _valueProvider = valueProvider;
    }

    public void SetProvider(IValueProvider valueProvider)
    {
      _valueProvider = valueProvider;
    }

    public IChainValueProvider SetChainProvider(IValueProvider valueProvider, Type providerType, string nameFilter=null)
    {
      IChainValueProvider chainValueProvider = null;
      var key = GetKey(providerType, nameFilter);
      if(HasChainProvider(providerType, nameFilter))
      {
        chainValueProvider = GetChainProvider(providerType, nameFilter);
        chainValueProvider.SetProvider(valueProvider);
      }
      else
      {
        chainValueProvider = new ChainValueProvider(_randomGeneratorProvider, valueProvider);
        _nextProviders.Add(key,chainValueProvider );
      }

      return chainValueProvider;
    }

    public bool HasChainProvider(Type providerType, string nameFilter=null)
    {
      return _nextProviders.ContainsKey(GetKey(providerType, nameFilter));
    }

    public IChainValueProvider GetChainProvider(Type providerType, string nameFilter)
    {
      return _nextProviders[GetKey(providerType, nameFilter)];
    }

    public Random Random
    {
      get { return _randomGeneratorProvider.Random; }
    }

    public bool HasValue()
    {
      return _valueProvider != null;
    }

    public object GetValue()
    {
      return !HasValue() ? null : _valueProvider.GetObjectValue(_randomGeneratorProvider);
    }

    private static ChainKey GetKey (Type providerType, string nameFilter)
    {
      return new ChainKey(providerType, nameFilter);
    }
  }
}