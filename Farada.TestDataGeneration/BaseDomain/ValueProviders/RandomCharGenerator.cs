﻿using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomCharGenerator:ValueProvider<char>
  {
    protected override char CreateValue (ValueProviderContext<char> context)
    {
      return context.Random.NextDouble() >= 0.2d ? (char) context.Random.Next(33, 126) : (char) context.Random.Next(161, 591);
    }
  }
}