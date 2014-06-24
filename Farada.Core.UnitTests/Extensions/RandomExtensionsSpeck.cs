﻿using System;
using Farada.Core.Extensions;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.Core.UnitTests.Extensions
{
  [Subject (typeof (RandomExtensions))]
  public class RandomExtensionsSpeck:Specs<Random>
  {
    static Context<Random> DefaultRandomContext ()
    {
      return c => c.GivenSubject ("default random", x => new Random ());
    }

    [Group]
    void Next ()
    {
      Specify (x => x.Next (byte.MinValue, byte.MaxValue))
          .Elaborate ("returns number in byte range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in byte range", x => x.Result.Should ().BeInRange (byte.MinValue, byte.MaxValue)));

      Specify (x => x.Next (decimal.MinValue, decimal.MaxValue))
          .Elaborate ("returns number in decimal range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in decimal range", x => x.Result.Should ().BeInRange (decimal.MinValue, decimal.MaxValue)));

      Specify (x => x.Next (double.MinValue, double.MaxValue))
          .Elaborate ("returns number in double range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in double range", x => x.Result.Should ().BeInRange (double.MinValue, double.MaxValue)));

      Specify (x => x.Next (float.MinValue, float.MaxValue))
          .Elaborate ("returns number in float range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in float range", x => x.Result.Should ().BeInRange (float.MinValue, float.MaxValue)));

      Specify (x => x.Next(long.MinValue, long.MaxValue))
          .Elaborate ("returns number in long range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in long range", x => x.Result.Should ().BeInRange (long.MinValue, long.MaxValue)));

      Specify (x => x.Next(uint.MinValue, uint.MaxValue))
          .Elaborate ("returns number in uint range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in uint range", x => x.Result.Should ().BeInRange (uint.MinValue, uint.MaxValue)));

      Specify (x => x.Next(ulong.MinValue, ulong.MaxValue))
          .Elaborate ("returns number in ulong range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in ulong range", x => x.Result.Should ().BeInRange (ulong.MinValue, ulong.MaxValue)));

      Specify (x => x.Next(short.MinValue, short.MaxValue))
          .Elaborate ("returns number in short range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in short range", x => x.Result.Should ().BeInRange (short.MinValue, short.MaxValue)));

      Specify (x => x.Next(ushort.MinValue, ushort.MaxValue))
          .Elaborate ("returns number in ushort range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in ushort range", x => x.Result.Should ().BeInRange (ushort.MinValue, ushort.MaxValue)));

      Specify (x => x.Next (sbyte.MinValue, sbyte.MaxValue))
          .Elaborate ("returns number in sbyte range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in sbyte range", x => x.Result.Should ().BeInRange (sbyte.MinValue, sbyte.MaxValue)));
    }

    static Context<Random> SeededRandomContext (int seed)
    {
      return c => c.GivenSubject ("random with seed " + seed, x => new Random (seed));
    }

    [Group]
    void NextSeeded ()
    {
      Specify (x => x.Next (long.MinValue, long.MaxValue))
          .Elaborate ("returns number in byte range", _ => _
              .Given (SeededRandomContext (0))
              .It ("result is always 10", x => x.Result.Should ().Be(2086725849749066753)));
    }
  }
}