# HeliosTrack: Solar Asset Performance & Operational Efficiency Engine

HeliosTrack is a full-stack internal engineering prototype designed to solve a critical operational challenge in utility and commercial solar management. 

Unlike total system failures, degraded inverters, dirty panels, or localized tracking failures often go unnoticed because arrays continue to produce power—just at a lower capacity. This tool implements a self-contained, high-performance data pipeline that normalizes real-time power metrics against solar geometry to automate underperformance flagging, protecting organizational profitability and optimizing technician field deployment.

---

## Architecture & Data Design

HeliosTrack is built using a decoupled, thin-controller architecture leveraging preferred enterprise frameworks.

### Core Tech Stack
* **Frontend:** React (Vite, TypeScript)
* **Backend:** .NET 9 Web API (C#)
* **Database:** MySQL Server
* **ORM:** Entity Framework Core (Fluent API)

### Database Relationship Schema
The data model uses a strict hierarchical structure optimized for physical asset tracking:
`Site` (1) ➡️ (Many) `Grid` (1) ➡️ (Many) `SolarPanel` (1) ➡️ (Many) `PerformanceLog`.

To maximize query speed on read-heavy internal dashboards, the system operates on a **Write-Heavy Optimization pattern**. Instead of executing expensive spherical trigonometry calculations on the fly during a dashboard `GET` request, the exact operational efficiency and an execution flag (`IsUnderperforming`) are processed **exactly once** on the `POST` side and persisted immutably to MySQL.

---

## The Core Intelligence: NOAA Solar Normalization

Comparing raw energy output ($kWh$) directly to a panel's peak capacity ($kW$) introduces severe data noise; a perfectly healthy panel will look "broken" at 8:00 AM or 5:00 PM simply because the sun is low. 

HeliosTrack resolves this by routing incoming logs through a localized `SolarMathHelper` engine that implements the **NOAA Global Monitoring Laboratory Solar Position Algorithms** directly in C#.

### The Mathematical Pipeline:
1. **Temporal-to-Spatial Mapping:** Converts the incoming UTC log timestamp into a `fractionalYear` (Gamma) to map the Earth's orbital position.
2. **Declination (`decl`):** Computes the exact seasonal tilt angle of the earth relative to the sun (fluctuating smoothly between $-23.5^\circ$ and $+23.5^\circ$ via sine/cosine waves).
3. **True Solar Time Offset:** Multiplies the site's decimal `Longitude` variable by $4.0$ minutes per degree to calculate the exact physical moment the sun passes over that specific panel meridian, bypassing timezone clock discrepancies.
4. **Hour Angle (`ha`):** Determines the Earth's physical rotation degrees away from true solar noon.
5. **Solar Elevation Angle ($\alpha$):** Solves the spherical trigonometry identity to find the sun's angle relative to the horizon:
   $$\cos(\text{Zenith}) = (\sin(\text{lat}) \cdot \sin(\text{decl})) + (\cos(\text{lat}) \cdot \cos(\text{decl}) \cdot \cos(\text{ha}))$$

### 3-Step Normalization Filter:
Once the sun's elevation angle is known, the service layer evaluates efficiency using three operational checkpoints:
1. **Expected Power:** $\text{Peak Capacity (kW)} \times \sin(\alpha)$
2. **Expected Energy:** $\text{Expected Power (kW)} \times \text{Log Interval Time (Hours)}$
3. **Relative Efficiency:** $\frac{\text{Actual Energy Generated (Value)}}{\text{Expected Energy Baseline}}$

If the `Relative Efficiency` drops below **75%** during active peak hours, the database flag `IsUnderperforming` is flipped to `true`.

---

## ⚡ Engineering & Performance Decisions

* **LINQ Projections Over Eager Loading:** To prevent the database overhead of full entity tracking and multi-table Left Joins (the Cartesian Product problem), the calculation pipeline utilizes strict LINQ `.Select()` projections. This ensures MySQL only transmits the exact decimal coordinates required for the NOAA math, drastically lowering RAM utilization.
* **Production Edge-Case Bypassing:** Real-world physical environments inject data noise. The ingestion service implements strict guard clauses:
    * **Nighttime/Dawn Bypassing:** Bypasses logic if calculated $\alpha < 15^\circ$, eliminating division-by-zero crashes and preventing false failure alarms caused by minimum inverter startup voltages.
    * **UTC Standardization:** All relational database timestamps are written strictly in UTC, ensuring the system is immune to localized Daylight Saving Time shifts.
