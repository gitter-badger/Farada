﻿using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomLongGenerator:ValueProvider<long>
  {
    protected override long CreateValue (ValueProviderContext<long> context)
    {
      return context.Random.Next(long.MinValue);
    }
  }
}