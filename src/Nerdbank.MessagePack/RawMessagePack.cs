﻿// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft;

namespace Nerdbank.MessagePack;

/// <summary>
/// Represents a sequence of raw msgpack bytes.
/// </summary>
/// <remarks>
/// <para>
/// This struct is useful as the type for some field or property that should be serialize or deserialized separately from its surrounding data.
/// For example an RPC protocol may have an envelope around user data, such that the envelope and the user data should have distinct serialization rules.
/// The envelope could use this <see cref="RawMessagePack"/> in order to facilitate this by allowing pre-serialization and deferred deserialization of user data.
/// </para>
/// <para>
/// The <see cref="MessagePackConverter{T}"/> for this type will always copy the memory from the buffers being read so that this struct has
/// an independent lifetime.
/// </para>
/// </remarks>
[MessagePackConverter(typeof(RawMessagePackConverter))]
public struct RawMessagePack : IEquatable<RawMessagePack>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RawMessagePack"/> struct.
	/// </summary>
	/// <param name="msgpack">The actual sequence of msgpack bytes.</param>
	public RawMessagePack(ReadOnlySequence<byte> msgpack)
	{
		this.MsgPack = msgpack;
	}

	/// <summary>
	/// Gets a sequence of raw MessagePack bytes.
	/// </summary>
	public ReadOnlySequence<byte> MsgPack { get; private set; }

	/// <summary>
	/// Gets a value indicating whether the <see cref="MsgPack"/> is owned by this instance,
	/// and thus can be relied on to be immutable and not recycled by others.
	/// </summary>
	public bool IsOwned { get; internal set; }

	/// <summary>
	/// Implicitly converts a <see cref="ReadOnlySequence{T}"/> of bytes to a <see cref="RawMessagePack"/>.
	/// </summary>
	/// <param name="msgpack">The sequence of MessagePack bytes.</param>
	/// <returns>A new instance of <see cref="RawMessagePack"/> containing the provided bytes.</returns>
	public static implicit operator RawMessagePack(ReadOnlySequence<byte> msgpack) => new(msgpack);

	/// <summary>
	/// Implicitly converts a <see cref="ReadOnlyMemory{T}"/> of bytes to a <see cref="RawMessagePack"/>.
	/// </summary>
	/// <param name="msgpack">The memory containing MessagePack bytes.</param>
	/// <returns>A new instance of <see cref="RawMessagePack"/> containing the provided bytes.</returns>
	public static implicit operator RawMessagePack(ReadOnlyMemory<byte> msgpack) => new(new ReadOnlySequence<byte>(msgpack));

	/// <summary>
	/// Implicitly converts a <see cref="Memory{T}"/> of bytes to a <see cref="RawMessagePack"/>.
	/// </summary>
	/// <param name="msgpack">The memory containing MessagePack bytes.</param>
	/// <returns>A new instance of <see cref="RawMessagePack"/> containing the provided bytes.</returns>
	public static implicit operator RawMessagePack(Memory<byte> msgpack) => new(new ReadOnlySequence<byte>(msgpack));

	/// <summary>
	/// Implicitly converts an array of bytes to a <see cref="RawMessagePack"/>.
	/// </summary>
	/// <param name="msgpack">The memory containing MessagePack bytes.</param>
	/// <returns>A new instance of <see cref="RawMessagePack"/> containing the provided bytes.</returns>
	public static implicit operator RawMessagePack(byte[] msgpack) => new(new ReadOnlySequence<byte>(msgpack));

	/// <summary>
	/// Implicitly converts a <see cref="RawMessagePack"/> to a <see cref="ReadOnlySequence{T}"/> of bytes.
	/// </summary>
	/// <param name="msgpack">The <see cref="RawMessagePack"/> instance.</param>
	/// <returns>The <see cref="ReadOnlySequence{T}"/> of bytes contained in the <see cref="RawMessagePack"/>.</returns>
	public static implicit operator ReadOnlySequence<byte>(RawMessagePack msgpack) => msgpack.MsgPack;

	/// <inheritdoc/>
	public readonly bool Equals(RawMessagePack other) => SequenceEqual(this.MsgPack, other.MsgPack);

	/// <summary>
	/// Produces a self-sustaining copy of this struct that will outlive whatever the original source buffer was from which this was created.
	/// </summary>
	/// <returns>
	/// A copy of the data that is guaranteed to be immutable.
	/// When <see cref="IsOwned"/> is already <see langword="true" />, <see langword="this"/> will be returned instead of making a redundant copy.
	/// </returns>
	/// <remarks>
	/// <para>
	/// This is useful when its owner wants the data to live longer than the underlying buffers from which it was created.
	/// </para>
	/// <para>
	/// This struct mutates itself as well to retain possession of the cloned data, so that multiple calls to this method do not result in redundant copies.
	/// This will only be effective when the caller stores the struct in a mutable location.
	/// If this struct is stored in a <see langword="readonly" /> field for instance, the local copy cannot be mutated, though the returned copy
	/// will actually be self-owned.
	/// </para>
	/// </remarks>
	public RawMessagePack ToOwned()
	{
		if (this.IsOwned)
		{
			return this;
		}

		this.MsgPack = this.MsgPack.Clone();
		this.IsOwned = true;

		return this;
	}

	private static bool SequenceEqual<T>(in ReadOnlySequence<T> a, in ReadOnlySequence<T> b)
#if !NET
		where T : IEquatable<T>
#endif
	{
		if (a.Length != b.Length)
		{
			return false;
		}

		if (a.IsSingleSegment && b.IsSingleSegment)
		{
#if NET
			return a.FirstSpan.SequenceEqual(b.FirstSpan);
#else
			return a.First.Span.SequenceEqual(b.First.Span);
#endif
		}

		ReadOnlySequence<T>.Enumerator aEnumerator = a.GetEnumerator();
		ReadOnlySequence<T>.Enumerator bEnumerator = b.GetEnumerator();

		ReadOnlySpan<T> aCurrent = default;
		ReadOnlySpan<T> bCurrent = default;
		while (true)
		{
			bool aNext = TryGetNonEmptySpan(ref aEnumerator, ref aCurrent);
			bool bNext = TryGetNonEmptySpan(ref bEnumerator, ref bCurrent);
			if (!aNext && !bNext)
			{
				// We've reached the end of both sequences at the same time.
				return true;
			}
			else if (aNext != bNext)
			{
				// One ran out of bytes before the other.
				// We don't anticipate this, because we already checked the lengths.
				throw Assumes.NotReachable();
			}

			int commonLength = Math.Min(aCurrent.Length, bCurrent.Length);
			if (!aCurrent[..commonLength].SequenceEqual(bCurrent[..commonLength]))
			{
				return false;
			}

			aCurrent = aCurrent.Slice(commonLength);
			bCurrent = bCurrent.Slice(commonLength);
		}

		static bool TryGetNonEmptySpan(ref ReadOnlySequence<T>.Enumerator enumerator, ref ReadOnlySpan<T> span)
		{
			while (span.Length == 0)
			{
				if (!enumerator.MoveNext())
				{
					return false;
				}

				span = enumerator.Current.Span;
			}

			return true;
		}
	}
}
