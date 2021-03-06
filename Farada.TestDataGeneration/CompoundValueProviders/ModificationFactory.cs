﻿using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
    /// <summary>
    /// The ModificationFactory modifys the previously created instances based on the registered <see cref="IInstanceModifier"/>s in the domain.
    /// </summary>
    internal class ModificationFactory
    {
        private readonly IList<IInstanceModifier> _instanceModifiers;
        private readonly IRandom _random;

        internal ModificationFactory(IList<IInstanceModifier> instanceModifiers, IRandom random)
        {
            _instanceModifiers = instanceModifiers;
            _random = random;
        }

        internal IList<object> ModifyInstances(IKey currentKey, IList<object> instances)
        {
            return _instanceModifiers.Aggregate(
                    instances,
                    (current, instanceModifier) =>
                            instanceModifier.Modify(new ModificationContext(currentKey.Type, currentKey.Member, _random), current));
        }
    }
}