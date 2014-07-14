﻿using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
    /// <summary>
    /// The compound value provider is a <see cref="ITestDataGenerator"/> that can fill compound types such as classes
    /// based on a domain decleration, where various PropertyChains, TypeChains and AttributeChains are declared
    /// </summary>
    internal class CompoundValueProvider : ITestDataGenerator
    {private readonly InstanceFactory _instanceFactory;
        private ModificationFactory _modificationFactory;

        public Random Random
        {
            get; private set;
        }

        internal CompoundValueProvider (ValueProviderDictionary valueProviderDictionary, Random random, IList<IInstanceModifier> instanceModifiers)
        {
            Random = random;
            _instanceFactory = new InstanceFactory (this, valueProviderDictionary);
            _modificationFactory = new ModificationFactory (instanceModifiers, random);
        }

        public TValue Create<TValue> (int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo = null)
        {
            return CreateMany<TValue> (1, maxRecursionDepth, propertyInfo).Single();
        }

        public IReadOnlyList<TValue> CreateMany<TValue> (
                int numberOfObjects,
                int maxRecursionDepth = 2,
                IFastPropertyInfo propertyInfo = null)
        {
            var rootKey = propertyInfo == null
                    ? (IKey) new TypeKey (typeof (TValue))
                    : new ChainedKey (typeof (TValue), propertyInfo);

            var instances = CreateMany (rootKey, numberOfObjects, maxRecursionDepth);

            return instances == null ? CreateManyDefaultObjects<TValue> (numberOfObjects) : instances.CastOrDefault<TValue>().ToList();
        }

        private static IReadOnlyList<TValue> CreateManyDefaultObjects<TValue> (int numberOfObjects)
        {
            return EnumerableExtensions.Repeat (() => default(TValue), numberOfObjects).ToList();
        }

        ///Note: The create many method is optimized in many ways
        ///1: If you create 100 Dog objects it will first created 100 instances of Dog (with a fast version of the Activator)
        ///   Then it searches the value provider for each property and generates 100 values per property for each instance of Dog
        /// 2: It uses a cached but thread safe version of reflection <see cref="FastReflectionUtility"/>
        private IList<object> CreateMany (IKey currentKey, int numberOfObjects, int maxRecursionDepth)
        {
            //we check for recursion depth in order to avoid endless recursions (e.g. a->b->a->b->a->...)
            if (currentKey.RecursionDepth >= maxRecursionDepth)
                return null;

            //Here we create the actual instances of the type (these are values created by either the fast activator or the registered value providers)
            var instances = _instanceFactory.CreateInstances (currentKey, numberOfObjects);

            //in case the instances were not created (no value provider, not instantiable, we just return null values, that will later be casted to default(T))
            if (instances == null)
                return null;

            //now that we have valid instances we can modify them all in a batch by the registered instance modifiers (example: a null modifier, that makes 30% of the instances null)
            instances = _modificationFactory.ModifyInstances(currentKey, instances);

            //check if type injection has taken place and split the base type into the correct sub types (important for base class properties)
            var typeToInstances = SubTypeInstanceHolder.SplitUpSubTypes (currentKey, instances);

            //now we iterate over all instances for each sub type (this is more efficient)
            foreach (var instancesForType in typeToInstances.Values)
            {
                //if the sub type cannot be instantiated, we just skip it, it will not be filled (maybe it is a value type)
                if (!instancesForType.Key.PropertyType.IsCompoundType())
                    continue;

                //now we reflect the properties of the concrete sub type (note:this is cached in a concurrent dictionary) 
                var properties = FastReflectionUtility.GetTypeInfo (instancesForType.Key.PropertyType).Properties;

                //now we fill each property
                foreach (var property in properties)
                {
                    //first we need to create the key that the property has in our creator chain
                    var nextKey = instancesForType.Key.CreateKey (property);

                    //next we will recursively call this function (in case the property is compound/complex)
                    //Note: we create many values, in order to execute the logic of the method less often
                    var propertyValues = CreateMany (nextKey, instancesForType.Instances.Count, maxRecursionDepth);

                    //in case we could not fill the property we just skip filling it, leave it to it's current (probably default) value (e.g. because it was not supported) 
                    if (propertyValues == null)
                        continue;

                    //now we iterate over all created values and set them for the corresponding previously created instance
                    //Example: if you call CreatMany<Dog>(100) - all dog properties will be created 100 times and filled into the 100 dog instances
                    for (var i = 0; i < instancesForType.Instances.Count; ++i)
                    {
                        //Of course some of the instances might be null (due to modifieres or sub types or sub type value providers or randomness of value providers)
                        //we can safely skip them
                        if (instancesForType.Instances[i] == null)
                            continue;

                        //in case we got a value we set the property
                        property.SetValue (instancesForType.Instances[i], propertyValues[i]);
                    }
                }
            }

            //here we return the instances that were created and filled (with all properties and sub properties)
            return instances;
        }        
    }
}