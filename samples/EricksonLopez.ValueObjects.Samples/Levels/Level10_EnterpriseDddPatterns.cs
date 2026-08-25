// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.DomainPrimitives;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 10: Enterprise Domain-Driven Design (DDD) &amp; Multi-Tenant SaaS Architecture.
/// Demonstrates Aggregate Roots composed purely of rich Value Objects, full audit trails,
/// multi-tenancy bounds, <see cref="DateRange"/> and <see cref="BusinessDate"/> in enterprise contexts,
/// and the <see cref="ValueObjectDomainPrimitiveExtensions"/> bridge between
/// EricksonLopez.ValueObjects and EricksonLopez.DomainPrimitives ecosystems.
/// </summary>
public static class Level10_EnterpriseDddPatterns
{
    /// <summary>
    /// Executes enterprise DDD and multi-tenant SaaS architecture demonstrations.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 10] ENTERPRISE ARCHITECTURE: DDD, MULTI-TENANCY, AUDIT, AND DATERANGE");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // ─── 1. Multi-Tenant Context & Licensing ──────────────────────────────────────
        Console.WriteLine("[1. Multi-Tenant Context & Licensing]");
        var tenant  = TenantCode.Create("acme-global-corp").Value;
        var license = LicenseKey.Create("ABCD-EF01-2345-6789").Value;
        var tz      = TimeZoneCode.Create("America/Santo_Domingo").Value;
        var lang    = LanguageCode.Create("es").Value;
        var locale  = LocaleCode.Create("es-DO").Value;
        var webUrl  = WebsiteUrl.Create("https://acme.enterprise.com").Value;

        Console.WriteLine($"  - Tenant        : {tenant}");
        Console.WriteLine($"  - License       : {license}");
        Console.WriteLine($"  - Configuration : TimeZone={tz}, Language={lang}, Locale={locale}");
        Console.WriteLine($"  - Portal URL    : {webUrl}");

        // ─── 2. Complete Audit Traceability ───────────────────────────────────────────
        Console.WriteLine("\n[2. Complete Audit Traceability]");
        var createdBy  = CreatedBy.Create("usr-system-provisioner").Value;
        var modifiedBy = ModifiedBy.Create("usr-finance-admin").Value;
        var subject    = Subject.Create("Monthly Billing Batch Approval").Value;
        var comment    = Comment.Create("Approved after cross-validation with the tax authority.").Value;
        var fileName   = FileName.Create("Fiscal_Report_August_2026.pdf").Value;
        var mime       = ContentType.Create("application/pdf").Value;

        Console.WriteLine($"  - Created By    : {createdBy}");
        Console.WriteLine($"  - Modified By   : {modifiedBy}");
        Console.WriteLine($"  - Subject       : {subject}");
        Console.WriteLine($"  - Comment       : {comment}");
        Console.WriteLine($"  - Attachment    : {fileName} ({mime})");

        // ─── 3. DateRange in Enterprise Context ───────────────────────────────────────
        Console.WriteLine("\n[3. DateRange — Delivery Windows and Fiscal Periods]");

        // Delivery window for an enterprise order
        var orderPlaced  = new DateOnly(2026, 8, 24);
        var deliveryFrom = new DateOnly(2026, 8, 30);
        var deliveryTo   = new DateOnly(2026, 9, 7);

        var deliveryWindow = DateRange.Create(deliveryFrom, deliveryTo).Value;
        Console.WriteLine($"  - Delivery Window  : {deliveryWindow}");
        Console.WriteLine($"  - Available Days   : {deliveryWindow.DurationInDays}");
        Console.WriteLine($"  - Contains 01-Sep  : {deliveryWindow.Contains(new DateOnly(2026, 9, 1))}");

        // Fiscal quarter as a DateRange
        var q3Start = new DateOnly(2026, 7, 1);
        var q3End   = new DateOnly(2026, 9, 30);
        var q3      = DateRange.Create(q3Start, q3End).Value;

        Console.WriteLine($"  - Q3 2026          : {q3} ({q3.DurationInDays} days)");
        Console.WriteLine($"  - Q3 overlaps del. : {q3.Overlaps(deliveryWindow)}");

        // ─── 4. BusinessDate in Enterprise Context ────────────────────────────────────
        Console.WriteLine("\n[4. BusinessDate — Business Dates in Contracts and Invoices]");

        var invoiceDate   = BusinessDate.Create(new DateOnly(2026, 8, 24)).Value;
        var dueDate       = BusinessDate.Create(new DateOnly(2026, 9, 24)).Value;
        var contractStart = BusinessDate.Create(new DateOnly(2026, 1, 1)).Value;
        var contractEnd   = BusinessDate.Create(new DateOnly(2026, 12, 31)).Value;

        Console.WriteLine($"  - Issue Date       : {invoiceDate}");
        Console.WriteLine($"  - Due Date         : {dueDate}");
        Console.WriteLine($"  - Issue < Due      : {invoiceDate < dueDate}");
        Console.WriteLine($"  - Contract 2026    : [{contractStart} .. {contractEnd}]");
        Console.WriteLine($"  - From DTO (UTC)   : {BusinessDate.FromDateTimeOffset(DateTimeOffset.UtcNow).Value}");

        // ─── 5. DomainPrimitives Bridge ───────────────────────────────────────────────
        Console.WriteLine("\n[5. DomainPrimitives Bridge — ValueObjectDomainPrimitiveExtensions]");
        Console.WriteLine("  - The bridge enables seamless conversion between EricksonLopez.ValueObjects");
        Console.WriteLine("    and EricksonLopez.DomainPrimitives when a domain utilizes both ecosystems.");
        Console.WriteLine("  - ToDomainPrimitive<TSelf,TValue,TPrimitive>(): converts VO → IDomainPrimitive");
        Console.WriteLine("  - ToStrongId<TSelf,TValue,TStrongId>(): converts VO → IStrongId");
        Console.WriteLine("  - DomainPrimitiveErrorExtensions.ToError(): converts PrimitiveError → Error");
        Console.WriteLine("  - DomainPrimitiveErrorExtensions.ToPrimitiveError(): converts Error → PrimitiveError");
        Console.WriteLine("  (Actual execution requires an IDomainPrimitive type in the consuming domain.)");

        // ─── 6. Composite Aggregate Root: EnterpriseOrder ────────────────────────────
        Console.WriteLine("\n[6. Composite Aggregate Root: EnterpriseOrder — Full VO Composition]");
        var order = new EnterpriseOrder(
            id:           Guid.NewGuid(),
            tenant:       tenant,
            orderNumber:  OrderNumber.Create("ORD-2026-9001").Value,
            customerCode: CustomerCode.Create("CUST-7700").Value,
            currency:     CurrencyCode.Create("USD").Value,
            createdBy:    createdBy,
            deliveryWindow: deliveryWindow,
            invoiceDate:  invoiceDate
        );

        order.AddItem(SKU.Create("LAPTOP-DELL-XPS").Value, Quantity.Create(2).Value,  Money.Create(1500.00m, "USD").Value);
        order.AddItem(SKU.Create("MONITOR-4K-27").Value,   Quantity.Create(4).Value,  Money.Create(400.00m,  "USD").Value);

        var subtotal  = order.CalculateSubtotal();
        var taxRate   = TaxRate.Create(18.0m).Value;
        var taxAmount = taxRate.CalculateTax(subtotal);
        var total     = subtotal + taxAmount;

        Console.WriteLine($"  - Order {order.OrderNumber} for Tenant '{order.Tenant}':");
        Console.WriteLine($"    Total Lines    : {order.Items.Count}");
        Console.WriteLine($"    Subtotal       : {subtotal}");
        Console.WriteLine($"    VAT (18%)      : {taxAmount}");
        Console.WriteLine($"    Total with VAT : {total}");
        Console.WriteLine($"    Delivery Window: {order.DeliveryWindow}");
        Console.WriteLine($"    Invoice Date   : {order.InvoiceDate}");

        // ─── 7. DiscountRate in enterprise commercial negotiation ────────────────────
        Console.WriteLine("\n[7. Enterprise Negotiation — DiscountRate + TaxRate on Money]");
        var listPrice    = Money.Create(2000.00m, "USD").Value;
        var negotiated   = DiscountRate.Create(15.0m).Value;
        var netPrice     = negotiated.ApplyTo(listPrice);
        var taxes        = taxRate.CalculateTax(netPrice);
        var finalPrice   = netPrice + taxes;

        Console.WriteLine($"  - List Price     : {listPrice}");
        Console.WriteLine($"  - Discount (15%) : -{negotiated.CalculateDiscount(listPrice.Amount)} USD");
        Console.WriteLine($"  - Net Price      : {netPrice}");
        Console.WriteLine($"  - VAT (18%)      : +{taxes}");
        Console.WriteLine($"  - Final Price    : {finalPrice}");
    }

    /// <summary>
    /// Sample Domain Aggregate Root demonstrating pure Value Object composition
    /// including <see cref="DateRange"/> and <see cref="BusinessDate"/> temporal values.
    /// </summary>
    private sealed class EnterpriseOrder
    {
        public Guid Id               { get; }
        public TenantCode Tenant     { get; }
        public OrderNumber OrderNumber { get; }
        public CustomerCode CustomerCode { get; }
        public CurrencyCode Currency { get; }
        public CreatedBy CreatedBy   { get; }
        public DateRange DeliveryWindow { get; }
        public BusinessDate InvoiceDate { get; }
        public List<OrderItem> Items { get; } = [];

        public EnterpriseOrder(
            Guid id,
            TenantCode tenant,
            OrderNumber orderNumber,
            CustomerCode customerCode,
            CurrencyCode currency,
            CreatedBy createdBy,
            DateRange deliveryWindow,
            BusinessDate invoiceDate)
        {
            Id             = id;
            Tenant         = tenant;
            OrderNumber    = orderNumber;
            CustomerCode   = customerCode;
            Currency       = currency;
            CreatedBy      = createdBy;
            DeliveryWindow = deliveryWindow;
            InvoiceDate    = invoiceDate;
        }

        public void AddItem(SKU sku, Quantity quantity, Money unitPrice)
        {
            Items.Add(new OrderItem(sku, quantity, unitPrice));
        }

        public Money CalculateSubtotal()
        {
            decimal total = 0m;
            foreach (var item in Items)
            {
                total += item.UnitPrice.Amount * item.Quantity.Value;
            }
            return Money.Create(total, Currency.Value).Value;
        }
    }

    private sealed record OrderItem(SKU Sku, Quantity Quantity, Money UnitPrice);
}
