﻿using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Farada.TestDataGeneration.Extensions;

namespace Farada.TestDataGeneration.FastReflection
{
  /// <summary>
  /// Provides a faster way to create a class then the standard <see cref="Activator"/>
  /// </summary>
  public static class FastActivator
  {
    private static readonly ConcurrentDictionary<Type, Func<object>> s_creatorCache = new ConcurrentDictionary<Type, Func<object>>();

    ///<summary>
    /// Create an object that will used as a 'factory' to the specified type T  
    /// </summary>
    /// <returns>the factory the creates a new object of type T</returns>
    public static Func<object> GetFactory (Type t)
    {
      return s_creatorCache.GetOrAdd(t, CreateFactoryMethod);
    }

    private static Func<object> CreateFactoryMethod (Type type)
    {
      if (type.IsValueType)
        throw new ArgumentException("Value types cannot be instantiated");

      if (!type.CanBeInstantiated())
      {
        if (type.CanBeInstantiated(nonPublic: true))
        {
          throw new NotSupportedException("Classes with non-public constructors are not supported");
        }

        throw new NotSupportedException("Classes without default constructors are not supported");
      }

      var expression = Expression.Lambda<Func<object>>(Expression.New(type));
      return expression.Compile();
    }
  }
}
