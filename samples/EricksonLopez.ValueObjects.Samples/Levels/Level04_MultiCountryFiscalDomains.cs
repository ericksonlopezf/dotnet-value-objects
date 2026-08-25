// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.Fiscal.Peru;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 04: Multi-Country Fiscal Domains.
/// Demonstrates strongly typed, immutable Value Objects with official regulatory validation algorithms
/// across 6 Latin American tax authorities (DGII, SAT, DIAN, AFIP, SII, SUNAT).
/// </summary>
public static class Level04_MultiCountryFiscalDomains
{
    /// <summary>
    /// Executes the multi-country fiscal domain demonstrations.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 4] MULTI-COUNTRY FISCAL DOMAINS (REGULATORY COMPLIANCE)");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // 1. DOMINICAN REPUBLIC (DGII)
        Console.WriteLine("[1. Dominican Republic - DGII]");
        var rnc = Rnc.Create("1-31-88073-8").Value;
        var cedula = Cedula.Create("001-1234567-3").Value;
        var ncf = Ncf.Create("B0100000001").Value;
        var encf = ElectronicNcf.Create("E310000000001").Value;
        var fiscalPeriodDo = FiscalPeriod.Create(2026, 8).Value;
        var secCode = SecurityCode.Create("aB39Zq").Value;

        Console.WriteLine($"  - Corporate RNC       : {rnc}");
        Console.WriteLine($"  - National ID (Cedula): {cedula} (Masked)");
        Console.WriteLine($"  - Traditional NCF     : {ncf} (Type: {ncf.Type})");
        Console.WriteLine($"  - Electronic e-NCF    : {encf} (e-CF Type: {encf.Type})");
        Console.WriteLine($"  - Fiscal Period       : {fiscalPeriodDo}");
        Console.WriteLine($"  - Security Code       : {secCode}");

        // 2. MEXICO (SAT - CFDI 4.0 & Carta Porte)
        Console.WriteLine("\n[2. Mexico - SAT CFDI 4.0]");
        var rfc = Rfc.Create("ABC680524P76").Value;
        var curp = Curp.Create("GODE561231HDFRRN08").Value;
        var cfdiUsage = CfdiUsageCode.Create("G01").Value;
        var taxRegime = TaxRegimeCode.Create("601").Value;
        var paymentForm = PaymentFormCode.Create("01").Value;
        var uuid = FiscalUuid.Create(Guid.NewGuid()).Value;
        var pedimento = PedimentoNumber.Create("234739990001234").Value;

        Console.WriteLine($"  - Taxpayer RFC        : {rfc}");
        Console.WriteLine($"  - Individual CURP     : {curp} (Masked)");
        Console.WriteLine($"  - CFDI Usage          : {cfdiUsage}");
        Console.WriteLine($"  - Tax Regime          : {taxRegime}");
        Console.WriteLine($"  - Payment Form        : {paymentForm}");
        Console.WriteLine($"  - Fiscal Folio UUID   : {uuid}");
        Console.WriteLine($"  - Pedimento Number    : {pedimento}");

        // 3. COLOMBIA (DIAN - Electronic Invoicing)
        Console.WriteLine("\n[3. Colombia - DIAN]");
        var nit = Nit.Create("800197268-4").Value;
        var cufe = Cufe.Create(new string('0', 96)).Value;
        var ciiu = CiiuCode.Create("6201").Value;
        var dane = DaneMunicipalityCode.Create("11001").Value;
        var taxType = TaxTypeCode.Create("01").Value;

        Console.WriteLine($"  - NIT with Modulo 11 DV: {nit} (Base: {nit.BaseNumber}, DV: {nit.VerificationDigit})");
        Console.WriteLine($"  - Invoice CUFE         : {cufe.Value[..16]}... (96 hex chars)");
        Console.WriteLine($"  - CIIU Activity Code   : {ciiu}");
        Console.WriteLine($"  - DANE Municipality    : {dane} (Bogota D.C.)");
        Console.WriteLine($"  - Tax Type Code        : {taxType}");

        // 4. ARGENTINA (AFIP - Electronic Invoicing & CBU/CVU)
        Console.WriteLine("\n[4. Argentina - AFIP]");
        var cuit = Cuit.Create("20-12345678-6").Value;
        var cuil = Cuil.Create("27-23456789-1").Value;
        var cbu = Cbu.Create("0720000700000001234565").Value;
        var cvu = Cvu.Create("0000001700000001234565").Value;
        var pos = PointOfSale.Create(1).Value;
        var voucherNum = VoucherNumber.Create(42).Value;
        var voucherLetter = VoucherLetter.Create("A").Value;
        var voucherType = VoucherType.Create(1).Value;
        var vatRate = VatRate.Create(21.0m).Value;

        Console.WriteLine($"  - CUIT / CUIL          : {cuit} / {cuil}");
        Console.WriteLine($"  - Banking CBU (22d)    : {cbu}");
        Console.WriteLine($"  - Wallet CVU (22d)     : {cvu}");
        Console.WriteLine($"  - AFIP Voucher         : Invoice {voucherLetter} Type {voucherType.Code} No. {pos.Value:D4}-{voucherNum.Value:D8}");
        Console.WriteLine($"  - Argentina VAT Rate   : {vatRate.Percentage}%");

        // 5. CHILE (SII - DTE & RUT)
        Console.WriteLine("\n[5. Chile - SII DTE]");
        var rut = Rut.Create("76.192.083-9").Value;
        var dteType = DteTypeCode.Create(33).Value;
        var folio = FiscalFolio.Create(123456).Value;
        var vatChile = TaxRateVat.Create(19.0m).Value;
        var withRate = WithholdingRate.Create(13.75m).Value;

        Console.WriteLine($"  - RUT with Modulo 11   : {rut.ToFormattedString()} (Body: {rut.Body}, DV: {rut.Dv})");
        Console.WriteLine($"  - DTE Type Code        : {dteType.Code} (Electronic Invoice)");
        Console.WriteLine($"  - DTE Fiscal Folio     : {folio.Value}");
        Console.WriteLine($"  - Chile VAT Rate       : {vatChile.Percentage}%");
        Console.WriteLine($"  - Withholding Rate     : {withRate.Percentage}%");

        // 6. PERU (SUNAT - CPE & RUC)
        Console.WriteLine("\n[6. Peru - SUNAT CPE]");
        var ruc = Ruc.Create("10456789019").Value;
        var cpeType = CpeTypeCode.Create("01").Value;
        var cpeId = CpeIdentifier.Create("01-F001-00000001").Value;
        var affectation = AffectationTypeCode.Create("10").Value;
        var detraction = DetractionAccount.Create("00051123456").Value;
        var periodPe = TaxPeriod.Create(2026, 1).Value;
        var ubigeo = UbigeoCode.Create("040101").Value;

        Console.WriteLine($"  - SUNAT RUC (11 d)     : {ruc} (Type: {(ruc.IsNaturalPerson ? "Natural Person" : "Legal Entity")})");
        Console.WriteLine($"  - CPE Receipt          : {cpeId} (Type: {cpeType})");
        Console.WriteLine($"  - Affectation Type     : {affectation} (Taxable - Onerous)");
        Console.WriteLine($"  - Detraction Account   : {detraction}");
        Console.WriteLine($"  - Tax Period           : {periodPe}");
        Console.WriteLine($"  - Ubigeo Code          : {ubigeo} (Arequipa)");
    }
}
