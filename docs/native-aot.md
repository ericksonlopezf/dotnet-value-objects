# NativeAOT & Trimming Compatibility

> **Ahead-of-Time Compilation, Serverless Optimization & Zero Reflection**

---

## 1. NativeAOT Compliance Principles

- **Zero Dynamic Code Generation**: No `Emit`, no dynamic proxies, and no un-trimmable reflection.
- **Trimming Annotations**: `<IsAotCompatible>true</IsAotCompatible>` and `<IsTrimmable>true</IsTrimmable>` set centrally in `Directory.Build.props`.
- **Automated Smoke Gate**: The `aot-smoke-test.yml` GitHub workflow compiles a native Linux binary and asserts zero warnings and clean execution on every build.
