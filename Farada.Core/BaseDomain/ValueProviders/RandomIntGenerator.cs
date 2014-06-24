﻿using System;
using Farada.Core.BaseDomain.Constraints;
using Farada.Core.ValueProvider;

namespace Farada.Core.BaseDomain.ValueProviders
{
  public class RandomIntGenerator:ValueProvider<int, RangeConstrainedValueProviderContext<int>>
  {
    protected override RangeConstrainedValueProviderContext<int> CreateContext (ValueProviderObjectContext objectContext)
    {
      var rangeContstraints = RangeContstraints<int>.FromProperty(objectContext.PropertyInfo)
                              ?? new RangeContstraints<int>(int.MinValue, int.MaxValue);

      return new RangeConstrainedValueProviderContext<int>(objectContext, rangeContstraints);
    }

    protected override int CreateValue (RangeConstrainedValueProviderContext<int> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }

 
}