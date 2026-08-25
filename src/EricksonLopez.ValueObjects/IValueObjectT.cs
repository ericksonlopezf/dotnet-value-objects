// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Defines a strongly-typed contract for value objects enforcing value-based equality.
/// </summary>
/// <typeparam name="TSelf">The concrete value object type implementing this interface.</typeparam>
public interface IValueObject<TSelf> : IValueObject, IEquatable<TSelf>
    where TSelf : IValueObject<TSelf>;
