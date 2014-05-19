﻿using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  public class RandomNumberGenerator:ValueProvider<int>
  {
    protected override int CreateValue (ValueProviderContext<int> context)
    {
      return context.Random.Next(int.MinValue, int.MaxValue);
    }
  }
}
