using System.Runtime.Serialization;

namespace ProductRepositoryAsync;

/// <summary>
/// The exception is thrown when there is an invalid attempt to find a collection.
/// </summary>
[Serializable]
public class CollectionNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class.
    /// </summary>
    public CollectionNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class with a specified error message.
    /// </summary>
    public CollectionNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    public CollectionNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [Obsolete("GetObjectData is obsolete and should not be used. Consider alternative serialization methods.")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}
