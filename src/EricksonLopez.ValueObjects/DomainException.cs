// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents errors caused by value object invariant violations and invalid domain states resulting from caller programming errors.
/// </summary>
/// <remarks>
/// Reserved for programmer errors at the domain boundary rather than standard operational or business validation failures.
/// </remarks>
public sealed class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DomainException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Throws a <see cref="DomainException"/> if the specified condition is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="message">The message to include in the exception if thrown.</param>
    /// <exception cref="DomainException"><paramref name="condition"/> is <see langword="true"/></exception>
    public static void ThrowIf(bool condition, string message)
    {
        if (condition) throw new DomainException(message);
    }
}

