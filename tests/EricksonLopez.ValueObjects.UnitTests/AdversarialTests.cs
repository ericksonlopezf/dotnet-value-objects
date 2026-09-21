// Copyright © Erickson Lopez. MIT License.
using System;
using System.Reflection;
using System.Text.Json;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public class AdversarialTests
{
    [Fact]
    public void StructDefault_BypassesMoneyInvariants()
    {
        // Attack 1: default(T) on readonly record struct
        var defaultMoney = default(Money);

        // Verification: The currency is null/uninitialized, which is an invalid domain state.
        Assert.False(defaultMoney.IsInitialized);
        Assert.True(string.IsNullOrEmpty(defaultMoney.Currency.Value));
        Assert.Equal(0m, defaultMoney.Amount);

        // Attempting to do arithmetic on this invalid state throws DomainException
        var validMoney = Money.Create(100, CurrencyCode.USD).Value;

        Assert.Throws<DomainException>(() => _ = defaultMoney + validMoney);
    }

    [Fact]
    public void Reflection_CanBypassStringValueObjectInvariants()
    {
        // Attack 2: Reflection on record class
        var codeResult = CustomerCode.Create("CUST-123");
        Assert.True(codeResult.IsSuccess);

        var code = codeResult.Value;

        // Forcefully mutate the underlying compiler-generated backing field using Reflection
        var fieldInfo = typeof(SingleValueObject<CustomerCode, string>).GetField("<Value>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(code, "invalid code !!");
        }

        // The property getter will now return the invalid state
        var mutatedValue = code.Value;
        Assert.Equal("invalid code !!", mutatedValue);
    }

    [Fact]
    public void NullAttacks_ShouldBeCaught()
    {
        // Attempt to create string value object with null
        Assert.Throws<ArgumentNullException>(() =>
        {
            // We use reflection to invoke the constructor inherited from StringValueObject
            var ctor = typeof(CustomerCode).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
            try
            {
                ctor!.Invoke(new object[] { null! });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException!;
            }
        });
    }

    [Fact]
    public void Serialization_DeserializingInvalidState_Fails()
    {
        // Attempt to deserialize invalid state bypassing Create()
        var json = "{\"Value\": \"not-an-email\"}";

        // When using System.Text.Json without custom converters, it cannot populate private read-only state.
        var options = JsonSerializerOptions.Default;
        var email = JsonSerializer.Deserialize<Email>(json, options);
        Assert.Null(email.Value);
        Assert.False(email.IsInitialized);
    }
}
