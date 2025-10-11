namespace ChatApp.Common.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
	public abstract IEnumerable<object> GetEqualityComponents();

	public static bool operator ==(ValueObject left, ValueObject right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(ValueObject left, ValueObject right)
	{
		return !Equals(left, right);
	}

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;

		if (obj.GetType() != GetType()) return false;

		var valueObject = (ValueObject)obj!;

		return GetEqualityComponents()
			.SequenceEqual(valueObject.GetEqualityComponents());
	}

	public bool Equals(ValueObject? other)
	{
		return Equals((object?)other);
	}

	public override int GetHashCode()
	{
		return GetEqualityComponents()
			.Select(x => x?.GetHashCode() ?? 0)
			.Aggregate((x, y) => x ^ y);
	}
}