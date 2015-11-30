﻿using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class AttributeProviderConfigurator<TMember, TAttribute> :ChainConfigurator, IAttributeProviderAndChainConfigurator<TMember, TAttribute>
      where TAttribute : Attribute
  {
    internal AttributeProviderConfigurator (Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
        : base(lazyValueProviderBuilder)
    {
    }

    public IAttributeProviderAndChainConfigurator<TMember, TAttribute> AddProvider (AttributeBasedValueProvider<TMember, TAttribute> attributeValueProvider)
    {
      _lazyValueProviderBuilder().AddProvider(attributeValueProvider);
      return this;
    }
  }
}