# Bounded Context & Layer Boundaries

---

## 1. Value Object Ecosystem Boundaries

```mermaid
graph TD
    Core[EricksonLopez.ValueObjects (Core VO: Money, Address, Geo)]
    FiscDom[Fiscal.DominicanRepublic] --> Core
    FiscCl[Fiscal.Chile] --> Core
    FiscCo[Fiscal.Colombia] --> Core
    FiscMx[Fiscal.Mexico] --> Core
    FiscPe[Fiscal.Peru] --> Core
    FiscAr[Fiscal.Argentina] --> Core

    EF[EntityFrameworkCore Adapter] --> Core
    Dap[Dapper Adapter] --> Core
    STJ[Serialization.Json Adapter] --> Core
```
