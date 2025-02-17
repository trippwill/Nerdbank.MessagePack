// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Sample1
{
    #region Farm
    public class Farm
    {
        public List<Animal>? Animals { get; set; }
    }
    #endregion

#if NET
    #region FarmAnimalsNET
    [KnownSubType<Cow>(1)]
    [KnownSubType<Horse>(2)]
    [KnownSubType<Dog>(3)]
    public class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    public partial class Cow : Animal { }
    [GenerateShape]
    public partial class Horse : Animal { }
    [GenerateShape]
    public partial class Dog : Animal { }
    #endregion
#else
    #region FarmAnimalsNETFX
    [KnownSubType(typeof(Cow), 1)]
    [KnownSubType(typeof(Horse), 2)]
    [KnownSubType(typeof(Dog), 3)]
    public class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    public partial class Cow : Animal { }
    [GenerateShape]
    public partial class Horse : Animal { }
    [GenerateShape]
    public partial class Dog : Animal { }
    #endregion
#endif

    #region HorsePen
    public class HorsePen
    {
        public List<Horse>? Horses { get; set; }
    }
    #endregion

#if NET
    #region HorseBreedsNET
    [KnownSubType<QuarterHorse>(1)]
    [KnownSubType<Thoroughbred>(2)]
    public partial class Horse : Animal { }

    [GenerateShape]
    public partial class QuarterHorse : Horse { }
    [GenerateShape]
    public partial class Thoroughbred : Horse { }
    #endregion
#else
    #region HorseBreedsNETFX
    [KnownSubType(typeof(QuarterHorse), 1)]
    [KnownSubType(typeof(Thoroughbred), 2)]
    public partial class Horse : Animal { }

    [GenerateShape]
    public partial class QuarterHorse : Horse { }
    [GenerateShape]
    public partial class Thoroughbred : Horse { }
    #endregion
#endif
}

namespace GenericSubTypes
{
#if NET
    #region ClosedGenericSubTypesNET
    [KnownSubType<Horse>(1)]
    [KnownSubType<Cow<SolidHoof>, Witness>(2)]
    [KnownSubType<Cow<ClovenHoof>, Witness>(3)]
    class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    partial class Cow<THoof> : Animal { }

    [GenerateShape<Cow<SolidHoof>>]
    [GenerateShape<Cow<ClovenHoof>>]
    partial class Witness;

    class SolidHoof { }

    class ClovenHoof { }
    #endregion
#else
    #region ClosedGenericSubTypesNETFX
    [KnownSubType(typeof(Horse), 1)]
    [KnownSubType(typeof(Cow<SolidHoof>), 2)]
    [KnownSubType(typeof(Cow<ClovenHoof>), 3)]
    class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    partial class Cow<THoof> : Animal { }

    [GenerateShape<Cow<SolidHoof>>]
    [GenerateShape<Cow<ClovenHoof>>]
    partial class Witness;

    class SolidHoof { }

    class ClovenHoof { }
    #endregion
#endif
}

namespace StringAliasTypes
{
#if NET
    #region StringAliasTypesNET
    [GenerateShape]
    [KnownSubType<Horse>("Horse")]
    [KnownSubType<Cow>("Cow")]
    partial class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }
    #endregion
#else
    #region StringAliasTypesNETFX
    [GenerateShape]
    [KnownSubType(typeof(Horse), "Horse")]
    [KnownSubType(typeof(Cow), "Cow")]
    partial class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }
    #endregion
#endif
}

namespace MixedAliasTypes
{
#if NET
    #region MixedAliasTypesNET
    [GenerateShape]
    [KnownSubType<Horse>(1)]
    [KnownSubType<Cow>("Cow")]
    partial class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }
    #endregion
#else
    #region MixedAliasTypesNETFX
    [GenerateShape]
    [KnownSubType(typeof(Horse), 1)]
    [KnownSubType(typeof(Cow), "Cow")]
    partial class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }
    #endregion
#endif
}

namespace InferredAliasTypes
{
#if NET
    #region InferredAliasTypesNET
    [GenerateShape]
    [KnownSubType<Horse>]
    [KnownSubType<Cow>]
    partial class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }
    #endregion
#else
    #region InferredAliasTypesNETFX
    [GenerateShape]
    [KnownSubType(typeof(Horse))]
    [KnownSubType(typeof(Cow))]
    partial class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }
    #endregion
#endif
}

namespace RuntimeSubTypes
{
#if NET
    #region RuntimeSubTypesNET
    class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }

    class SerializationConfigurator
    {
        internal void ConfigureAnimalsMapping(MessagePackSerializer serializer)
        {
            KnownSubTypeMapping<Animal> mapping = new();
            mapping.Add<Horse>(1);
            mapping.Add<Cow>(2);

            serializer.RegisterKnownSubTypes(mapping);
        }
    }
    #endregion
#else
    #region RuntimeSubTypesNETFX
    class Animal
    {
        public string? Name { get; set; }
    }

    [GenerateShape]
    partial class Horse : Animal { }

    [GenerateShape]
    partial class Cow : Animal { }

    [GenerateShape<Horse>]
    [GenerateShape<Cow>]
    partial class Witness;

    class SerializationConfigurator
    {
        internal void ConfigureAnimalsMapping(MessagePackSerializer serializer)
        {
            KnownSubTypeMapping<Animal> mapping = new();
            mapping.Add<Horse>(1, Witness.ShapeProvider);
            mapping.Add<Cow>(2, Witness.ShapeProvider);

            serializer.RegisterKnownSubTypes(mapping);
        }
    }
    #endregion
#endif
}
