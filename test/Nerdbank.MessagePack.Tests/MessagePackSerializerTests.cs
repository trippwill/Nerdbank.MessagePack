// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Nerdbank.MessagePack;
using Nerdbank.Streams;
using TypeShape;
using Xunit;
using Xunit.Abstractions;

public partial class MessagePackSerializerTests(ITestOutputHelper logger)
{
	private readonly MessagePackSerializer serializer = new();

	[Fact]
	public void SimpleNull() => this.AssertRoundtrip<Fruit>(null);

	[Fact]
	public void SimplePoco() => this.AssertRoundtrip(new Fruit { Seeds = 18 });

	[Fact]
	public void AllIntTypes() => this.AssertRoundtrip(new IntRichPoco { Int8 = -1, Int16 = -2, Int32 = -3, Int64 = -4, UInt8 = 1, UInt16 = 2, UInt32 = 3, UInt64 = 4 });

	[Fact]
	public void SimpleRecordClass() => this.AssertRoundtrip(new RecordClass(42) { Weight = 5, ChildNumber = 2 });

	[Fact]
	public void ClassWithDefaultCtorWithInitProperty() => this.AssertRoundtrip(new DefaultCtorWithInitProperty { Age = 42 });

	[Fact]
	public void RecordWithOtherPrimitives() => this.AssertRoundtrip(new OtherPrimitiveTypes("hello", true, 0.1f, 0.2));

	[Fact]
	public void NullableStruct_Null() => this.AssertRoundtrip(new RecordWithNullableStruct(null));

	[Fact]
	public void NullableStruct_NotNull() => this.AssertRoundtrip(new RecordWithNullableStruct(3));

	[Fact]
	public void Dictionary() => this.AssertRoundtrip(new ClassWithDictionary { StringInt = new() { { "a", 1 }, { "b", 2 } } });

	[Fact]
	public void Dictionary_Null() => this.AssertRoundtrip(new ClassWithDictionary { StringInt = null });

	[Fact]
	public void ImmutableDictionary() => this.AssertRoundtrip(new ClassWithImmutableDictionary { StringInt = ImmutableDictionary<string, int>.Empty.Add("a", 1) });

	protected void AssertRoundtrip<T>(T? value)
		where T : IShapeable<T>
	{
		T? roundtripped = this.Roundtrip(value);
		Assert.Equal(value, roundtripped);
	}

	protected T? Roundtrip<T>(T? value)
		where T : IShapeable<T>
	{
		Sequence<byte> sequence = new();
		this.serializer.Serialize(sequence, value);
		logger.WriteLine(MessagePack.MessagePackSerializer.ConvertToJson(sequence, MessagePack.MessagePackSerializerOptions.Standard));
		return this.serializer.Deserialize<T>(sequence);
	}

	[GenerateShape]
	public partial class Fruit : IEquatable<Fruit>
	{
		public int Seeds { get; set; }

		public bool Equals(Fruit? other) => other is not null && this.Seeds == other.Seeds;
	}

	[GenerateShape]
	public partial class IntRichPoco : IEquatable<IntRichPoco>
	{
		public byte UInt8 { get; set; }

		public ushort UInt16 { get; set; }

		public uint UInt32 { get; set; }

		public ulong UInt64 { get; set; }

		public sbyte Int8 { get; set; }

		public short Int16 { get; set; }

		public int Int32 { get; set; }

		public long Int64 { get; set; }

		public bool Equals(IntRichPoco? other)
			=> other is not null
			&& this.UInt8 == other.UInt8
			&& this.UInt16 == other.UInt16
			&& this.UInt32 == other.UInt32
			&& this.UInt64 == other.UInt64
			&& this.Int8 == other.Int8
			&& this.Int16 == other.Int16
			&& this.Int32 == other.Int32
			&& this.Int64 == other.Int64;
	}

	[GenerateShape]
	public partial record OtherPrimitiveTypes(string AString, bool ABoolean, float AFloat, double ADouble);

	[GenerateShape]
	public partial record RecordClass(int Seeds)
	{
		public int Weight { get; set; }

		public int ChildNumber { get; init; }
	}

	[GenerateShape]
	public partial class DefaultCtorWithInitProperty : IEquatable<DefaultCtorWithInitProperty>
	{
		public int Age { get; init; }

		public bool Equals(DefaultCtorWithInitProperty? other) => other is not null && this.Age == other.Age;
	}

	[GenerateShape]
	public partial record RecordWithNullableStruct(int? Value);

	[GenerateShape]
	public partial class ClassWithDictionary : IEquatable<ClassWithDictionary>
	{
		public Dictionary<string, int>? StringInt { get; set; }

		public bool Equals(ClassWithDictionary? other) => other is not null && ByValueEquality.Equal(this.StringInt, other.StringInt);
	}

	[GenerateShape]
	public partial class ClassWithImmutableDictionary : IEquatable<ClassWithImmutableDictionary>
	{
		public ImmutableDictionary<string, int>? StringInt { get; set; }

		public bool Equals(ClassWithImmutableDictionary? other) => other is not null && ByValueEquality.Equal(this.StringInt, other.StringInt);
	}
}
