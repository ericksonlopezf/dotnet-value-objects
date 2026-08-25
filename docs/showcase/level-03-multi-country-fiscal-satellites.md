# Level 03: Multi-Country Fiscal Satellites

> **Module:** Statutory Tax Identifiers & Electronic Invoice Validation across 6 LATAM Jurisdictions  
> **Key Packages:** `Fiscal.DominicanRepublic`, `Fiscal.Chile`, `Fiscal.Colombia`, `Fiscal.Mexico`, `Fiscal.Peru`, `Fiscal.Argentina`

---

## 1. Dominican Republic (DGII)

Validates 9-digit RNC (Modulo 11), 11-digit Cedula (Modulo 10 / Luhn), and Electronic Invoices (`ElectronicNcf` e-CF):

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

Result<Rnc> rnc = Rnc.Create("101000001");
Result<Cedula> cedula = Cedula.Create("00100000001");
Result<ElectronicNcf> eNcf = ElectronicNcf.Create("E3100000001");
```

---

## 2. Chile (SII)

Validates RUT (Rol Único Tributario) using official Modulo 11 check digit calculation ('0'-'9' or 'K'):

```csharp
using EricksonLopez.ValueObjects.Fiscal.Chile;

Result<Rut> rut = Rut.Create("12345678-5");
Result<FiscalFolio> folio = FiscalFolio.Create(123456);
```

---

## 3. Colombia (DIAN)

Validates NIT (Número de Identificación Tributaria) with prime-weighted Modulo 11 verification, and CUFE/CUDE SHA-384 hashes:

```csharp
using EricksonLopez.ValueObjects.Fiscal.Colombia;

Result<Nit> nit = Nit.Create("900123456-1");
```

---

## 4. Mexico (SAT CFDI 4.0)

Validates RFC for natural/moral entities with statutory homoclave and CFDI Fiscal UUIDs:

```csharp
using EricksonLopez.ValueObjects.Fiscal.Mexico;

Result<Rfc> rfc = Rfc.Create("XAXX010101000");
Result<FiscalUuid> uuid = FiscalUuid.Create("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d");
```

---

## 5. Peru (SUNAT) & Argentina (ARCA/AFIP)

```csharp
// Peru: RUC (SUNAT 11-digit Modulo 11)
using EricksonLopez.ValueObjects.Fiscal.Peru;
Result<Ruc> ruc = Ruc.Create("20100070970");

// Argentina: CUIT / CUIL (11-digit Modulo 11)
using EricksonLopez.ValueObjects.Fiscal.Argentina;
Result<Cuit> cuit = Cuit.Create("20-12345678-9");
```
