﻿using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// The non type safe context which contains all values used in the type safe context <see cref="ValueProviderContext{TProperty}"/>
  /// </summary>
  public class ValueProviderObjectContext
  {
    internal Random Random { get; private set; }
    internal Func<object> GetPreviousValue { get; private set; }
    internal Type TargetValueType { get; private set; }
    internal IFastPropertyInfo PropertyInfo { get; private set; }
    internal ITestDataGenerator TestDataGenerator { get; private set; }

    internal ValueProviderObjectContext (
        ITestDataGenerator testDataGenerator,
        Random random,
        Func<object> getPreviousValue,
        Type targetValueType,
        IFastPropertyInfo fastPropertyInfo)
    {
      TestDataGenerator = testDataGenerator;
      Random = random;
      GetPreviousValue = getPreviousValue;
      TargetValueType = targetValueType;
      PropertyInfo = fastPropertyInfo;
    }
  }
}