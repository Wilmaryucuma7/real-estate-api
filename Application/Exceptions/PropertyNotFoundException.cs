namespace RealEstateAPI.Application.Exceptions;

/// <summary>
/// Exception thrown when a requested property is not found.
/// </summary>
public sealed class PropertyNotFoundException : Exception
{
    public PropertyNotFoundException(string propertyId)
        : base($"Property with ID '{propertyId}' was not found.")
    {
        PropertyId = propertyId;
    }

    public string PropertyId { get; }
}
