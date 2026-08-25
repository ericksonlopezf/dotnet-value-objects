# Architectural Diagrams & Visual Maps

> **Visual Reference Blueprint for `EricksonLopez.ValueObjects`**

---

## 1. System Topology

```mermaid
graph LR
    subgraph CoreDomain [Core Domain Kernel]
        VO[EricksonLopez.ValueObjects]
        DPBridge[EricksonLopez.ValueObjects.DomainPrimitives]
    end

    subgraph FiscalSatellites [Multi-Country Fiscal Satellites]
        DO[Fiscal.DominicanRepublic]
        CL[Fiscal.Chile]
        CO[Fiscal.Colombia]
        MX[Fiscal.Mexico]
        PE[Fiscal.Peru]
        AR[Fiscal.Argentina]
    end

    subgraph Adapters [Persistence & Serialization]
        EF[EntityFrameworkCore]
        DAP[Dapper]
        JSON[Serialization.Json]
    end

    DO --> VO
    CL --> VO
    CO --> VO
    MX --> VO
    PE --> VO
    AR --> VO
    EF --> VO
    DAP --> VO
    JSON --> VO
    DPBridge --> VO
```

---

## 2. Money Allocation Flow

```mermaid
flowchart TD
    Start[Total Money: $100.00 USD] --> Input[Ratios: 1, 1, 1]
    Input --> Sum[Sum of Ratios = 3]
    Sum --> BaseCalc[Base Share: floor 100 * 1 / 3 = $33.33]
    BaseCalc --> Remainder[Remainder: $100.00 - $99.99 = $0.01]
    Remainder --> Distribute[Add $0.01 to highest remainder share]
    Distribute --> Output[Result: $33.34, $33.33, $33.33]
```

---

## 3. Fiscal Verification Pipeline

```mermaid
flowchart LR
    Input[Raw Tax ID] --> Sanitizer[Strip Hyphens & Spaces]
    Sanitizer --> LengthCheck{Correct Length?}
    LengthCheck -- No --> Error[Return Validation Error]
    LengthCheck -- Yes --> Algorithm{Run Country Algorithm: Modulo 11 / 10}
    Algorithm -- Checksum Mismatch --> Error
    Algorithm -- Checksum Match --> Success[Instantiate Valid Tax ID VO]
```
