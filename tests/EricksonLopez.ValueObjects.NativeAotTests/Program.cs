// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.Fiscal.Peru;

Console.WriteLine("======================================================");
Console.WriteLine(" EricksonLopez.ValueObjects NativeAOT Smoke Test Suite");
Console.WriteLine("======================================================");

int passed = 0;
void Assert(bool condition, string testName)
{
    if (!condition)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[FAIL] {testName}");
        Console.ResetColor();
        throw new InvalidOperationException($"Assertion failed for: {testName}");
    }
    passed++;
    Console.WriteLine($"[PASS] {testName}");
}

// 1. Email Value Object
var emailRes = Email.Create("erickson@domain.com");
Assert(emailRes.IsSuccess, "Email.Create succeeds with valid email");
Assert(emailRes.Value.Value == "erickson@domain.com", "Email value preserved");

// 2. Money Value Object & Fowler Allocation
var moneyRes = Money.Create(150.50m, CurrencyCode.USD);
Assert(moneyRes.IsSuccess, "Money.Create succeeds");
Assert(moneyRes.Value.Amount == 150.50m, "Money amount is correct");
Assert(moneyRes.Value.Currency == CurrencyCode.USD, "Money currency is correct");
var allocated = moneyRes.Value.Allocate([1, 1]);
Assert(allocated.Length == 2 && allocated[0].Amount + allocated[1].Amount == 150.50m, "Money.Allocate operates under NativeAOT");

// 3. Single Value Object Equality & Normalization
var firstName1 = FirstName.Create("Erickson");
var firstName2 = FirstName.Create("Erickson");
Assert(firstName1.Value.Value == firstName2.Value.Value, "FirstName instances match by value");
Assert(firstName1.Value.Equals(firstName2.Value), "FirstName equality works");

// 4. Quantity & Percentage
var qtyRes = Quantity.Create(10);
Assert(qtyRes.IsSuccess, "Quantity.Create succeeds");
Assert(qtyRes.Value.Value == 10, "Quantity creates valid instance");

var percentageRes = Percentage.Create(25.5m);
Assert(percentageRes.IsSuccess, "Percentage.Create succeeds");
Assert(percentageRes.Value.Value == 25.5m, "Percentage creates valid instance");

// 5. SKU & PasswordHash (Sensitive Data)
var skuRes = SKU.Create("PROD-12345");
Assert(skuRes.IsSuccess, "SKU.Create succeeds");
Assert(skuRes.Value.Value == "PROD-12345", "SKU creates valid instance");

var passHashRes = PasswordHash.Create("$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$RdescudvJCsgTVEvUzTFhg");
Assert(passHashRes.IsSuccess, "PasswordHash.Create succeeds");
Assert(passHashRes.Value.ToString() == "***HASHED***", "PasswordHash SensitiveData masking works");

// 6. Range & TimeRange
var rangeRes = Range<int>.Create(10, 100);
Assert(rangeRes.IsSuccess, "Range<int>.Create succeeds");
Assert(rangeRes.Value.Contains(50), "Range<int>.Contains works");

var timeRangeRes = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true);
Assert(timeRangeRes.IsSuccess && timeRangeRes.Value.CrossesMidnight, "TimeRange overnight works under NativeAOT");

// 7. Fiscal Satellites — Argentina (CUIT)
var cuitRes = Cuit.Create("20-12345678-6");
Assert(cuitRes.IsSuccess, "Fiscal.Argentina CUIT verification succeeds");
Assert(cuitRes.Value.VerificationDigit == 6, "CUIT verification digit matches");

// 8. Fiscal Satellites — Chile (RUT)
var rutRes = Rut.Create("76.192.083-9");
Assert(rutRes.IsSuccess, "Fiscal.Chile RUT verification succeeds");
Assert(rutRes.Value.Dv == '9', "RUT verification digit matches");

// 9. Fiscal Satellites — Colombia (NIT)
var nitRes = Nit.Create("830099999-9");
Assert(nitRes.IsSuccess, "Fiscal.Colombia NIT verification succeeds");
Assert(nitRes.Value.VerificationDigit == 9, "NIT verification digit matches");

// 10. Fiscal Satellites — Dominican Republic (Cedula, RNC & NCF)
var cedulaRes = Cedula.Create("001-1234567-3");
Assert(cedulaRes.IsSuccess, "Fiscal.DominicanRepublic Cedula succeeds");
var ncfRes = Ncf.Create("B0100000001");
Assert(ncfRes.IsSuccess, "Fiscal.DominicanRepublic NCF succeeds");

// 11. Fiscal Satellites — Mexico (RFC)
var rfcRes = Rfc.Create("ABC680524P76");
Assert(rfcRes.IsSuccess, "Fiscal.Mexico RFC succeeds");
Assert(rfcRes.Value.IsCompany, "RFC company flag evaluated");

// 12. Fiscal Satellites — Peru (RUC)
var rucRes = Ruc.Create("20456789014");
Assert(rucRes.IsSuccess, "Fiscal.Peru RUC succeeds");
Assert(rucRes.Value.IsLegalEntity, "RUC entity flag evaluated");

Console.WriteLine($"\nSUCCESS: All {passed} NativeAOT smoke tests passed with zero warnings/errors.");
return 0;
