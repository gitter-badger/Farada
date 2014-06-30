﻿using System;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.UnitTests.Extensions
{
  [Subject (typeof (TestDataGeneration.Extensions.TypeExtensions))]
  public class TypeExtensionsSpeck : Specs<Type>
  {
    bool IncludeNonPublicConstructor;

    [Group]
    void IsNullable()
    {
      Specify (x => x.IsNullableType ())
          .Elaborate ("simple nullable", _ => _
              .GivenSubject ("int?", x => typeof (int?))
              .It ("should be nullable", x => x.Result.Should ().BeTrue ()))
          .Elaborate ("complex nullable", _ => _
              .GivenSubject ("Nullable<>", x => typeof (Nullable<>))
              .It ("should be nullable", x => x.Result.Should ().BeTrue ()))
              .Elaborate ("non nullable", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should not be nullable", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("nullable class", _ => _
              .GivenSubject ("string", x => typeof (string))
              .It ("should not be nullable", x => x.Result.Should ().BeFalse ()));
    }
    
    [Group]
    void GetTypeOfNullable()
    {
      Specify (x => x.GetTypeOfNullable ())
          .Elaborate ("simple nullable", _ => _
              .GivenSubject ("int?", x => typeof (int?))
              .It ("should be int", x => x.Result.Should ().Be (typeof (int))))
          .Elaborate ("complex nullable", _ => _
              .GivenSubject ("Nullable<>", x => typeof (Nullable<>))
              .It ("should not be empty", x => x.Result.Should ().NotBeNull()))
          .Elaborate ("non nullable", _ => _
              .GivenSubject ("int", x => typeof (int))
              .ItThrows (typeof (ArgumentException)))
          .Elaborate ("nullable class", _ => _
              .GivenSubject ("string", x => typeof (string))
              .ItThrows (typeof (ArgumentException)));
    }

    [Group]
    void IsDerived ()
    {
      Specify (x => x.IsDerivedFrom<Base> ()).
          Elaborate ("simple derived", _ => _
              .GivenSubject ("Derived", x => typeof (Derived))
              .It ("should be derived", x => x.Result.Should ().BeTrue ())).
          Elaborate ("complex derived", _ => _
              .GivenSubject ("DerivedDerived", x => typeof (DerivedDerived))
              .It ("should be derived", x => x.Result.Should ().BeTrue ()))
          .Elaborate ("not derived", _ => _
              .GivenSubject ("NotDerived", x => typeof (NotDerived))
              .It ("should not be derived", x => x.Result.Should ().BeFalse ()));
    }

    [Group]
    void IsCompoundType ()
    {
      Specify (x => x.IsCompoundType ()).
          Elaborate ("value type", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ())).
          Elaborate ("complex type without default constructor", _ => _
              .GivenSubject ("ComplexTypeWithoutDefaultConstructor", x => typeof (ComplexTypeWithoutDefaultConstructor))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with private default constructor", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with default constructor", _ => _
              .GivenSubject ("ComplexTypeWithDefaultConstructor", x => typeof (ComplexTypeWithDefaultConstructor))
              .It ("should be compound type", x => x.Result.Should ().BeTrue ()));
    }

    [Group]
    void CanBeInstantiated ()
    {
      Specify (x => x.CanBeInstantiated (IncludeNonPublicConstructor)).
          Elaborate ("value type", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ())).
          Elaborate ("complex type without default constructor", _ => _
              .GivenSubject ("ComplexTypeWithoutDefaultConstructor", x => typeof (ComplexTypeWithoutDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with private default constructor", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with default constructor", _ => _
              .GivenSubject ("ComplexTypeWithDefaultConstructor", x => typeof (ComplexTypeWithDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeTrue ()))
          .Elaborate ("complex type with private default constructor including non-public constructors", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .Given ("Include non-public constructors", x => IncludeNonPublicConstructor = true)
              .It ("should be instatiantiable", x => x.Result.Should ().BeTrue ()));
    }

    [Group]
    void GetPropertyInfo()
    {
      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.Name))
          .Elaborate ("Get Property Info works on properties", _ => _
              .GivenSubject("SimpleDTO", x=>typeof(SimpleDTO))
              .It ("PropertyInfo matches", x => x.Result.Should ().BeSameAs (typeof (SimpleDTO).GetProperty ("Name"))));

      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.SomeField))
          .Elaborate ("Get Property Info throws on methods", _ => _
              .GivenSubject ("SimpleDTO", x => typeof (SimpleDTO))
              .ItThrows (typeof (ArgumentException), "Expression 'y => y.SomeField' refers to a field, not a property."));

      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.GetSomething ()))
          .Elaborate ("Get Property Info throws on fields", _ => _
              .GivenSubject ("SimpleDTO", x => typeof (SimpleDTO))
              .ItThrows (typeof (ArgumentException), "Expression 'y => y.GetSomething()' refers to a method, not a property."));

      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, SimpleDTO> (y => y))
          .Elaborate ("Get Property Info throws on classes", _ => _
              .GivenSubject ("SimpleDTO", x => typeof (SimpleDTO))
              .ItThrows (typeof (ArgumentException), "Expression 'y => y' refers to a method, not a property."));
    }

    class SimpleDTO
    {
      public string Name { get; set; }

      public string SomeField;

      public string GetSomething()
      {
        return "";
      }
    }

    class Base
    {

    }

    class Derived : Base
    {

    }

    class DerivedDerived : Derived
    {

    }

    class NotDerived
    {

    }

    class ComplexTypeWithoutDefaultConstructor
    {
      public ComplexTypeWithoutDefaultConstructor (int something)
      {

      }
    }

    class ComplexTypeWithPrivateDefaultConstructor
    {
      ComplexTypeWithPrivateDefaultConstructor ()
      {

      }
    }

    class ComplexTypeWithDefaultConstructor
    {
      public ComplexTypeWithDefaultConstructor ()
      {

      }
    }
  }
}