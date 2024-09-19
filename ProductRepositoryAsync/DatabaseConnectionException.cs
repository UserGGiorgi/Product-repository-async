using System.Runtime.Serialization;

namespace ProductRepositoryAsync;

/// <summary>
/// The exception is thrown when a connection to a database fails.
/// </summary>
[Serializable]
public class DatabaseConnectionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class.
    /// </summary>
    public DatabaseConnectionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class with a specified error message.
    /// </summary>
    public DatabaseConnectionException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    public DatabaseConnectionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    [Obsolete("GetObjectData is obsolete and should not be used. Consider alternative serialization methods.")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}
