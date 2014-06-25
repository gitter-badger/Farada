﻿using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random enum for any enum type (<see cref="SubTypeValueProvider{TProperty}"/>
  /// </summary>
  internal class RandomEnumGenerator:SubTypeValueProvider<Enum>
  {
    protected override Enum CreateValue (ValueProviderContext<Enum> context)
    {
      var enumNames = Enum.GetNames(context.PropertyType);
      if (enumNames.Length == 0)
        return default(Enum);

      var randomValue = enumNames[context.Random.Next(enumNames.Length)];

      return (Enum) Enum.Parse(context.PropertyType, randomValue);
    }
  }
}