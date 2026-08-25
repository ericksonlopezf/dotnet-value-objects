# Level 02 — Geographical & Contact Value Objects

In Level 02, we model spatial and communication concepts (`Address`, `GeoCoordinate`, `PhoneNumber`).

---

## 1. Geographical Coordinates

```csharp
using EricksonLopez.ValueObjects;

var location = new GeoCoordinate(latitude: 40.7128, longitude: -74.0060);
double distanceKm = location.DistanceTo(new GeoCoordinate(34.0522, -118.2437));
```
