using System.Runtime.Serialization;

namespace ProductRepositoryAsync;

/// <summary>
/// The exception is thrown when there is an error while working with the product repository.
/// </summary>
[Serializable]
public class RepositoryException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryException"/> class.
    /// </summary>
    public RepositoryException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message.
    /// </summary>
    public RepositoryException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    public RepositoryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [Obsolete("GetObjectData is obsolete and should not be used. Consider alternative serialization methods.")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}
