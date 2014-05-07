﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.Reflection
{
  public class FastPropertyInfo:IFastPropertyInfo
  {
    private readonly Func<object, object> _getFunction;
    private readonly Action<object, object> _setAction;

    public string Name { get; private set; }
    public Type PropertyType { get; private set; }

    public FastPropertyInfo (PropertyInfo propertyInfo)
    {
      var targetType = propertyInfo.DeclaringType;
      if(targetType==null)
      {
        throw new ArgumentException("PropertyInfo.DeclaringType was null");
      }

      Name = propertyInfo.Name;
      PropertyType = propertyInfo.PropertyType;

       _getFunction = CreateGetFunction(propertyInfo, targetType);
      _setAction=CreateSetAction(propertyInfo, targetType);
    }

    private static Func<object, object> CreateGetFunction (PropertyInfo propertyInfo, Type targetType)
    {
      var methodInfo = propertyInfo.GetGetMethod();
      var exTarget = Expression.Parameter(typeof (object), "t");

      var exBody = Expression.Convert(Expression.Call(Expression.Convert(exTarget, targetType), methodInfo), typeof (object));
      var lambda = Expression.Lambda<Func<object, object>>(exBody, exTarget);

      return lambda.Compile();
    }

    private static Action<object, object> CreateSetAction (PropertyInfo propertyInfo, Type targetType)
    {
      var methodInfo = propertyInfo.GetSetMethod();

      var exTarget = Expression.Parameter(typeof (object), "t");
      var exValue = Expression.Parameter(typeof (object), "p");

      var exBody = Expression.Call(
          Expression.Convert(exTarget, targetType),
          methodInfo,
          new Expression[] { Expression.Convert(exValue, propertyInfo.PropertyType) });

      var lambda = Expression.Lambda<Action<object, object>>(exBody, exTarget, exValue);
      return lambda.Compile();
    }

    public object GetValue (object instance)
    {
      return _getFunction(instance);
    }

    public void SetValue (object instance, object value)
    {
      _setAction(instance, value);
    }
  }

  public interface IFastPropertyInfo
  {
    object GetValue (object instance);
    void SetValue (object instance, object value);
    string Name { get; }
    Type PropertyType { get; }
  }
}