# Master Architectural Specification: Fiscal Value Objects Ecosystem for .NET 10

> **Ecosystem**: `EricksonLopez.ValueObjects`  
> **Runtime Target**: .NET 10 (C# 13 / C# 14)  
> **Paradigms**: Domain-Driven Design (DDD), Zero-Allocation, Native AOT-First, Result Pattern  
> **Jurisdictions**: Dominican Republic (DO 🇩🇴), Mexico (MX 🇲🇽), Argentina (AR 🇦🇷), Chile (CL 🇨🇱), Colombia (CO 🇨🇴), Peru (PE 🇵🇪)

---

## 1. Executive Summary

This specification formalizes the architecture, regulatory foundation, and technical design of the **Fiscal Value Objects** subsystem within the `EricksonLopez.ValueObjects` ecosystem.

### Foundational Principles:
1. **Core Domain Purity**: The core domain library (`EricksonLopez.ValueObjects`) contains zero country-specific tax rules.
2. **Dedicated Satellite Packages**: Each fiscal jurisdiction is isolated into an independent satellite NuGet package (`EricksonLopez.ValueObjects.Fiscal.{Country}`).
3. **Fail-Closed Validation**: Tax identification numbers, electronic invoice series, and fiscal authorization codes execute mathematical validation algorithms (e.g. Modulo 10, Modulo 11 with weights) upon instantiation via static `Create(...)` methods returning `Result<T>`.

---

## 2. Comprehensive Jurisdictional Specifications

### 2.1 🇩🇴 Dominican Republic (`EricksonLopez.ValueObjects.Fiscal.DominicanRepublic`)

Governed by the Dominican Republic Tax Authority (**DGII**), Tax Code Law 11-92, and Electronic Invoicing Law 32-23.

| Value Object | Regulatory Concept | Validation & Checksum Rules |
|---|---|---|
| `Rnc` | National Taxpayer Registry (`Registro Nacional del Contribuyente`) | 9-digit legal entity or 11-digit individual ID validated via **Modulo 11** with weights `[7, 9, 8, 6, 5, 4, 3, 2]`. |
| `Cedula` | National Identity and Voter Card (`Cedula de Identidad y Electoral`) | 11-digit national identity document validated via **Modulo 10** (Luhn algorithm variant) with alternating weights `[1, 2, 1, 2, ...]`. |
| `Ncf` | Fiscal Invoice Sequence Number (`Numero de Comprobante Fiscal` - Series B) | 11-character traditional fiscal invoice sequence (`B` + 2-digit type + 8-digit sequential correlative). E.g., `B0100000005`. |
| `ElectronicNcf` | Electronic Fiscal Invoice Sequence (`Comprobante Fiscal Electronico` - e-CF Series E) | 13-character electronic invoice sequence (`E` + 2-digit e-CF type [31..47] + 10-digit sequential correlative). E.g., `E310000000001`. |
| `FiscalPeriod` | Fiscal Tax Reporting Period (`Periodo Fiscal Tributario`) | Monthly reporting period in `YYYY-MM` format used in DGII 606, 607, 608, and IT-1 formats. |
| `SecurityCode` | Electronic Invoice Security Verification Code | 6-character cryptographic hash fragment extracted from electronic invoices for DGII QR verification. |

---

### 2.2 🇲🇽 Mexico (`EricksonLopez.ValueObjects.Fiscal.Mexico`)

Governed by the Mexican Tax Administration Service (**SAT**), Federal Fiscal Code (CFF Art. 29/29-A), and CFDI 4.0 standard.

| Value Object | Regulatory Concept | Validation & Invariant Rules |
|---|---|---|
| `Rfc` | Federal Taxpayer Registry (`Registro Federal de Contribuyentes`) | 12-character legal entity (`^[A-Z&N]{3}[0-9]{6}[A-Z0-9]{3}$`) or 13-character individual (`^[A-Z&N]{4}[0-9]{6}[A-Z0-9]{3}$`) with SAT homoclave checksum. |
| `Curp` | Single Population Registry Code (`Clave Unica de Registro de Poblacion`) | 18-character RENAPO national identification with state code and check digit. |
| `FiscalUuid` | Fiscal Folio UUID (`Folio Fiscal UUID` - CFDI 4.0) | Standard RFC 4122 UUID v4 assigned by an authorized PAC/SAT upon XML stamping. |
| `IdCcp` | Bill of Lading Identifier (`Identificador Carta Porte` - v3.1) | 36-character RFC 4122 UUID format with mandatory `CCC` prefix mandate for transport compliance. |
| `PedimentoNumber` | Customs Declaration Number (`Numero de Pedimento Aduanal` - Anexo 22) | 15-digit custom declaration format (`YY` year + `AA` customs + `PPPP` patent + `Y` last digit year + `NNNNNN` sequential). |
| `TaxRegimeCode` | SAT Tax Regime Code (`Regimen Fiscal SAT`) | 3-digit SAT catalog code (e.g., `601` General Legal Entities, `605` Salaries and Wages). |
| `PaymentFormCode` | SAT Payment Method Code (`Forma de Pago SAT`) | 2-digit SAT catalog code (e.g., `01` Cash, `03` Electronic Transfer, `04` Credit Card). |
| `CfdiUsageCode` | CFDI Usage Code (`Uso del CFDI`) | 3 or 4-character SAT code (e.g., `G01` Acquisition of merchandise, `G03` General expenses, `CP01` Payments). |

---

### 2.3 🇦🇷 Argentina (`EricksonLopez.ValueObjects.Fiscal.Argentina`)

Governed by the Federal Administration of Public Income (**ARCA / AFIP**), RG 1415/03, and BCRA banking regulations.

| Value Object | Regulatory Concept | Validation & Invariant Rules |
|---|---|---|
| `Cuit` | Unique Tax Identification Code (`Clave Unica de Identificacion Tributaria`) | 11-digit tax ID (`XY-NNNNNNNN-Z`) validated via **Modulo 11** with weights `[5, 4, 3, 2, 7, 6, 5, 4, 3, 2]`. Prefixes: `20`, `23`, `24`, `27`, `30`, `33`, `34`. |
| `Cuil` | Unique Labor Identification Code (`Clave Unica de Identificacion Laboral`) | 11-digit personal labor identifier for employees and social security (Modulo 11). |
| `Cbu` | Standard Banking Code (`Clave Bancaria Uniforme`) | 22-digit banking account code with dual Modulo 10 check digits (Block 1 bank/branch, Block 2 account). |
| `Cvu` | Standard Virtual Banking Code (`Clave Virtual Uniforme`) | 22-digit fintech/virtual wallet account identifier validated with dual Modulo 10 check digits. |
| `Cae` | Electronic Authorization Code (`Codigo de Autorizacion Electronico`) | 14-digit electronic invoice authorization code issued by AFIP web services. |
| `Caea` | Anticipated Electronic Authorization Code (`CAE Anticipado`) | 14-digit anticipated authorization code for high-volume contingencies. |
| `PointOfSale` | Fiscal Point of Sale (`Punto de Venta`) | 4 or 5-digit fiscal point of sale sequence (e.g. `00001`). |
| `VoucherNumber` | Voucher Sequential Number (`Numero de Comprobante`) | 8-digit sequential document number per point of sale. |
| `VoucherType` | Voucher Document Type (`Tipo de Comprobante`) | Official AFIP code (e.g., `001` Invoice A, `006` Invoice B, `011` Invoice C). |
| `VatRate` | VAT Tax Rate (`Tasa de Alicuota IVA`) | Standard AFIP VAT rates (0%, 2.5%, 5%, 10.5%, 21%, 27%). |

---

### 2.4 🇨🇱 Chile (`EricksonLopez.ValueObjects.Fiscal.Chile`)

Governed by the Internal Revenue Service (**SII**), Electronic Invoicing DTE standards, and Law 21.133.

| Value Object | Regulatory Concept | Validation & Invariant Rules |
|---|---|---|
| `Rut` | Single Tax Role (`Rol Unico Tributario`) | National tax & civil ID (`XXXXXXXX-K`) validated via **Modulo 11** with cyclical weights `[2, 3, 4, 5, 6, 7]` and verification digit `0-9` or `K`. |
| `FiscalFolio` | DTE Fiscal Folio (`Folio DTE`) | Positive non-zero integer authorized by the SII CAF (`Codigo de Autorizacion de Folios`). |
| `DteTypeCode` | Electronic Tax Document Type Code (`Tipo de Documento Tributario Electronico`) | Official SII codes: `33` Electronic Invoice, `34` Non-Taxable/Exempt Invoice, `39` Electronic Receipt, `41` Exempt Receipt, `52` Dispatch Guide, `61` Credit Note. |
| `DocumentReference` | Prior Document Cross-Reference | Cross-document reference linking credit/debit notes to original DTEs. |
| `TaxRateVat` | Chilean VAT Tax Rate | 19.0% standard Value Added Tax rate. |
| `WithholdingRate` | Professional Fees Withholding Rate (`Tasa de Retencion Honorarios`) | Progressive withholding rate under Law 21.133 (e.g. 13.75% for 2024, 14.5% for 2025). |

---

### 2.5 🇨🇴 Colombia (`EricksonLopez.ValueObjects.Fiscal.Colombia`)

Governed by the Special Administrative Unit Directorate of National Taxes and Customs (**DIAN**) and RADIAN invoicing registry.

| Value Object | Regulatory Concept | Validation & Invariant Rules |
|---|---|---|
| `Nit` | Tax Identification Number (`Numero de Identificacion Tributaria`) | 9 or 10-digit number with check digit verified via DIAN **Modulo 11** using prime weights `[3, 7, 13, 17, 19, 23, 29, 37, 41, 43, 47, 53, 59]`. |
| `Cufe` | Electronic Invoice Unique Code (`Codigo Unico de Factura Electronica`) | 96-character SHA-384 cryptographic hash computed across invoice fields for DIAN stamping. |
| `Cude` | Electronic Document Unique Code (`Codigo Unico de Documento Electronico`) | 96-character SHA-384 hash for electronic debit/credit notes and support documents. |
| `Cune` | Electronic Payroll Unique Code (`Codigo Unico de Nomina Electronica`) | 96-character SHA-384 hash for electronic payroll documents. |
| `DaneMunicipalityCode` | DANE Geopolitical Municipality Code | 5-digit geopolitical municipality code from the National Administrative Department of Statistics. |
| `CiiuCode` | ISIC Economic Activity Code (`Codigo de Actividad Economica CIIU`) | 4-digit International Standard Industrial Classification code. |
| `AuthorizationRange` | DIAN Numbering Authorization Range (`Rango de Numeracion DIAN`) | Resolution prefix and valid interval `[From .. To]` issued by DIAN numbering resolutions. |

---

### 2.6 🇵🇪 Peru (`EricksonLopez.ValueObjects.Fiscal.Peru`)

Governed by the National Superintendency of Customs and Tax Administration (**SUNAT**), UBL 2.1 e-invoicing, and SIRE platform.

| Value Object | Regulatory Concept | Validation & Invariant Rules |
|---|---|---|
| `Ruc` | Single Taxpayers Registry (`Registro Unico de Contribuyentes`) | 11-digit tax ID validated via **Modulo 11** with weights `[5, 4, 3, 2, 7, 6, 5, 4, 3, 2]`. Mandatory prefix validation: `10` (Natural person), `15`/`17` (Special person), `20` (Legal entity). |
| `CpeIdentifier` | Electronic Payment Voucher Identifier (`Identificador CPE` - Type-Series-Correlative) | Composed electronic invoice identifier matching SUNAT format: `[Type]-[Series]-[Correlative]`. E.g., `01-F001-00000123`. |
| `CpeTypeCode` | Electronic Payment Voucher Type Code (`Tipo de Comprobante de Pago Electronico`) | SUNAT Catalog 01 codes: `01` Invoice (`Factura`), `03` Sales Receipt (`Boleta de Venta`), `07` Credit Note (`Nota de Credito`), `08` Debit Note (`Nota de Debito`), `09` Dispatch Guide (`Guia de Remision Remitente`). |
| `DetractionAccount` | SPOT Detractions Bank Account (`Cuenta de Detracciones SPOT`) | 11-digit Banco de la Nacion detraction account sequence (`00-XXX-XXXXXX`). |
| `UbigeoCode` | Geographic Location Code (`Codigo de Ubicacion Geografica INEI`) | 6-digit territorial code representing Department (2 digits) + Province (2 digits) + District (2 digits). |
| `TaxPeriod` | SIRE Tax Reporting Period (`Periodo Tributario SIRE`) | 6-digit monthly tax declaration period in `YYYYMM` format. |
| `SunatProductCode` | SUNAT Product Classification Code (`Codigo de Producto SUNAT`) | 8-digit classification code from the United Nations Standard Products and Services Code (UNSPSC) adapted by SUNAT. |
