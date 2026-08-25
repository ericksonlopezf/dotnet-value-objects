# Developer Onboarding & Verification Evidence

> **Step-by-Step Environment Setup & Local Verification**

---

## 1. Prerequisites

- **.NET SDK 10.0.100** (pinned in `global.json`)
- **PowerShell 7+ (pwsh)** for repository compliance scripts
- **Git** with LF line endings (`core.autocrlf = input`)

---

## 2. Build & Verification Steps

```bash
# 1. Clone repository
git clone https://github.com/ericksonlopezf/dotnet-value-objects.git
cd dotnet-value-objects

# 2. Restore dependencies
dotnet restore EricksonLopez.ValueObjects.slnx

# 3. Build solution with zero warnings
dotnet build EricksonLopez.ValueObjects.slnx --no-restore --configuration Release

# 4. Execute full test suite (1,687 unit & integration tests)
dotnet test EricksonLopez.ValueObjects.slnx --no-build --configuration Release

# 5. Run compliance audit
pwsh ./scripts/verify-compliance.ps1
```
