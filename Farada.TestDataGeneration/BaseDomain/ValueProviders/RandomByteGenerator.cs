﻿using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomByteGenerator:ValueProvider<byte>
  {
    protected override byte CreateValue (ValueProviderContext<byte> context)
    {
      var randomBytes = new byte[1];
      context.Random.NextBytes(randomBytes);

      return randomBytes[0];
    }
  }
}