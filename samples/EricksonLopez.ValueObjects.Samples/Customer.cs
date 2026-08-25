// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Samples;

public sealed class Customer
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }

    public Customer(Guid id, string name, Email email, Address address)
    {
        Id = id;
        Name = name;
        Email = email;
        Address = address;
    }
}
