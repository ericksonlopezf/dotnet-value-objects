# RFC-0002: Segregation of Multi-Country Fiscal Satellites into Dedicated NuGet Packages

> **Status:** Approved  
> **Authors:** Erickson Lopez (<ericksonlopezf@gmail.com>)  
> **Created:** 2026-08-21  
> **Target Release:** v1.0.0  

---

## 1. Summary

This RFC mandates the segregation of country-specific tax rules, fiscal IDs, and electronic invoice standards into independent satellite packages (`EricksonLopez.ValueObjects.Fiscal.<Country>`), keeping the core `EricksonLopez.ValueObjects` package lightweight, universal, and free from regional legislative churn.

---

## 2. Motivation

Regulatory requirements across Latin America (e.g. DGII in Dominican Republic, SII in Chile, DIAN in Colombia, SAT in Mexico, SUNAT in Peru, ARCA in Argentina) are subject to independent legislative updates and specialized checksum algorithms. Bundling all country tax rules into a monolithic core package would bloat assemblies for single-country deployments and create unnecessary dependency couplings.

---

## 3. Specification

- **Core Package**: Contains only universally applicable domain concepts (`Money`, `Address`, `Email`, `PhoneNumber`, `Range<T>`).
- **Satellite Packages**:
  - `EricksonLopez.ValueObjects.Fiscal.DominicanRepublic`
  - `EricksonLopez.ValueObjects.Fiscal.Chile`
  - `EricksonLopez.ValueObjects.Fiscal.Colombia`
  - `EricksonLopez.ValueObjects.Fiscal.Mexico`
  - `EricksonLopez.ValueObjects.Fiscal.Peru`
  - `EricksonLopez.ValueObjects.Fiscal.Argentina`

All satellites reference the core library and are 100% trim-safe and NativeAOT-ready.

---

## 4. Decision

Approved unanimously. 6 fiscal satellites established with independent packaging.
