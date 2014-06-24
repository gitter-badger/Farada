﻿using System;
using Farada.Core.FastReflection;

namespace Farada.Core.Modifiers
{
  public class ModificationContext
  {
    public Type PropertyType { get; private set; }
    public IFastPropertyInfo Property { get; private set; }
    public Random Random { get; private set; }

    internal ModificationContext (Type propertyType, IFastPropertyInfo property, Random random)
    {
      PropertyType = propertyType;
      Property = property;
      Random = random;
    }
  }
}