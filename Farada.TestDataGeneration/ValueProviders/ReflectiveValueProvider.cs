﻿using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Represents a value provider that is based on a reflected <see cref="Type"/>.
  /// </summary>
  public abstract class ReflectiveValueProvider : ValueProvider<object>
  {
    private readonly Type _reflectedType;

    public ReflectiveValueProvider(Type reflectedType)
    {
      _reflectedType = reflectedType;
    }

    protected override ValueProviderContext<object> CreateContext(ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<object>(objectContext);
    }

    //we explicitly handle / fill only the reflective type..
    public override bool CanHandle(Type memberType)
    {
      return _reflectedType==memberType || _reflectedType == memberType.UnwrapIfNullable();
    }

    public override bool FillsType(Type memberType)
    {
      return _reflectedType == memberType || _reflectedType == memberType.UnwrapIfNullable();
    }
  }
}