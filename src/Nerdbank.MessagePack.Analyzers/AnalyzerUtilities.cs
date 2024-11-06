﻿// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Nerdbank.MessagePack.Analyzers;

internal static class AnalyzerUtilities
{
	internal static string GetHelpLink(string diagnosticId) => $"https://aarnott.github.io/Nerdbank.MessagePack/analyzers/{diagnosticId}.html";

	internal static IEnumerable<AttributeData> FindAttributes(this ISymbol symbol, string name, ImmutableArray<string> containingNamespaces)
	{
		foreach (AttributeData att in symbol.GetAttributes())
		{
			if (att.AttributeClass?.Name == name && IsInNamespace(att.AttributeClass, containingNamespaces.AsSpan()))
			{
				yield return att;
			}
		}
	}

	internal static IEnumerable<AttributeData> FindAttributes(this ISymbol symbol, INamedTypeSymbol attributeSymbol)
	{
		foreach (AttributeData att in symbol.GetAttributes())
		{
			INamedTypeSymbol? attClass = att.AttributeClass;
			if (attClass?.IsGenericType is true && attributeSymbol.IsUnboundGenericType)
			{
				attClass = attClass.ConstructUnboundGenericType();
			}

			if (SymbolEqualityComparer.Default.Equals(attClass, attributeSymbol))
			{
				yield return att;
			}
		}
	}

	internal static IEnumerable<INamedTypeSymbol> EnumerateBaseTypes(this ITypeSymbol symbol)
	{
		while (symbol.BaseType is not null)
		{
			yield return symbol.BaseType;
			symbol = symbol.BaseType;
		}
	}

	internal static bool IsAssignableTo(this ITypeSymbol subType, ITypeSymbol baseTypeOrInterface)
	{
		if (IsDerivedFrom(subType, baseTypeOrInterface))
		{
			return true;
		}

		return baseTypeOrInterface.TypeKind == TypeKind.Interface
			&& subType.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, baseTypeOrInterface));
	}

	internal static bool IsDerivedFrom(this ITypeSymbol subType, ITypeSymbol baseType)
	{
		ITypeSymbol? current = subType;
		while (current != null)
		{
			if (SymbolEqualityComparer.Default.Equals(current, baseType))
			{
				return true;
			}

			current = current.BaseType;
		}

		return false;
	}

	internal static bool IsInNamespace(ISymbol? symbol, ReadOnlySpan<string> namespaces)
	{
		if (symbol is null)
		{
			return false;
		}

		ISymbol? targetSymbol = symbol;
		for (int i = namespaces.Length - 1; i >= 0; i--)
		{
			if (targetSymbol.ContainingNamespace.Name != namespaces[i])
			{
				return false;
			}

			targetSymbol = targetSymbol.ContainingNamespace;
		}

		return targetSymbol.ContainingNamespace.IsGlobalNamespace;
	}
}
