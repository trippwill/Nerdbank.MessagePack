﻿// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !NET
#pragma warning disable CS1574 // unresolvable cref
#endif

using System.Runtime.InteropServices;

namespace Nerdbank.MessagePack.SecureHash;

/// <summary>
/// Provides a default implementation of <see cref="IEqualityComparer{T}"/> for blittable structs.
/// </summary>
/// <typeparam name="T">The type that may be compared.</typeparam>
/// <remarks>
/// This implementation takes the raw byte representation of a blittable struct as the hash input.
/// This will typically be correct behavior, but in cases where multiple unique binary data
/// represent values that are considered equivalent (specifically where <see cref="object.Equals(object?)"/>
/// would return <see lanword="true"/>), this method will not return the same hash code for those values
/// and would violate the invariant that hash codes must be equal between two values that are themselves considered equal.
/// An example of two equivalent values that have different binary representations are <see cref="double.NegativeZero"/>
/// and 0.
/// For such structs, it is important to normalize the data to be hashed first.
/// </remarks>
internal class CollisionResistantHasherUnmanaged<T> : SecureEqualityComparer<T>
	where T : unmanaged
{
	/// <inheritdoc/>
	public override bool Equals(T x, T y)
	{
#if NET
		Span<T> ySpan = new(ref y);
		Span<T> xSpan = new(ref x);
#else
		Span<T> ySpan = stackalloc T[1] { y };
		Span<T> xSpan = stackalloc T[1] { x };
#endif
		return MemoryMarshal.Cast<T, byte>(xSpan).SequenceEqual(MemoryMarshal.Cast<T, byte>(ySpan));
	}

	/// <inheritdoc/>
	public override long GetSecureHashCode(T value)
	{
#if NET
		Span<T> span = new(ref value);
#else
		Span<T> span = stackalloc T[1] { value };
#endif
		return SipHash.Default.Compute(MemoryMarshal.Cast<T, byte>(span));
	}
}
