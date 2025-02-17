// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/* THIS (.cs) FILE IS GENERATED. DO NOT CHANGE IT.
 * CHANGE THE .tt FILE INSTEAD. */

#pragma warning disable SA1121 // Simplify type syntax
#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single class

namespace Nerdbank.MessagePack;

/// <summary>Serializes the primitive integer type <see cref="SByte"/> as a MessagePack integer.</summary>
internal class SByteConverter : MessagePackConverter<SByte>
{
	/// <inheritdoc/>
	public override SByte Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadSByte();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in SByte value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}

/// <summary>Serializes the primitive integer type <see cref="Int16"/> as a MessagePack integer.</summary>
internal class Int16Converter : MessagePackConverter<Int16>
{
	/// <inheritdoc/>
	public override Int16 Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadInt16();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in Int16 value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}

/// <summary>Serializes the primitive integer type <see cref="Int32"/> as a MessagePack integer.</summary>
internal class Int32Converter : MessagePackConverter<Int32>
{
	/// <inheritdoc/>
	public override Int32 Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadInt32();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in Int32 value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}

/// <summary>Serializes the primitive integer type <see cref="Int64"/> as a MessagePack integer.</summary>
internal class Int64Converter : MessagePackConverter<Int64>
{
	/// <inheritdoc/>
	public override Int64 Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadInt64();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in Int64 value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}

/// <summary>Serializes the primitive integer type <see cref="Byte"/> as a MessagePack integer.</summary>
internal class ByteConverter : MessagePackConverter<Byte>
{
	/// <inheritdoc/>
	public override Byte Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadByte();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in Byte value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}

/// <summary>Serializes the primitive integer type <see cref="UInt16"/> as a MessagePack integer.</summary>
internal class UInt16Converter : MessagePackConverter<UInt16>
{
	/// <inheritdoc/>
	public override UInt16 Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadUInt16();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in UInt16 value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}

/// <summary>Serializes the primitive integer type <see cref="UInt32"/> as a MessagePack integer.</summary>
internal class UInt32Converter : MessagePackConverter<UInt32>
{
	/// <inheritdoc/>
	public override UInt32 Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadUInt32();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in UInt32 value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}

/// <summary>Serializes the primitive integer type <see cref="UInt64"/> as a MessagePack integer.</summary>
internal class UInt64Converter : MessagePackConverter<UInt64>
{
	/// <inheritdoc/>
	public override UInt64 Read(ref MessagePackReader reader, SerializationContext context) => reader.ReadUInt64();

	/// <inheritdoc/>
	public override void Write(ref MessagePackWriter writer, in UInt64 value, SerializationContext context) => writer.Write(value);

	/// <inheritdoc/>
	public override System.Text.Json.Nodes.JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape) => new() { ["type"] = "integer" };
}
