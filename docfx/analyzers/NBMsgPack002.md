# NBMsgPack002: Avoid `[Key]` on non-serialized members

@Nerdbank.MessagePack.KeyAttribute should not be applied to members that are not serialized.

## Resolution

There are two fixes for this condition:

- Remove the attribute.
- Serialize the member by applying the @PolyType.PropertyShapeAttribute, without setting @PolyType.PropertyShapeAttribute.Ignore to `true`.
