namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// What a field's value measures, which decides how it's shown under the game's Units setting.
// Confirmed against the Acura ARX-06 default setup with Units set to Imperial (2026-09-26):
//   Pressure   - raw Pa;  Metric bar (raw / 100000); Imperial psi (raw / 6894.757)
//   SpringRate - raw N/m; Metric N/mm (raw / 1000);  Imperial lb/in (raw / 175.1268)
//   Length     - raw m;   Metric mm (raw * 1000);    Imperial in (raw / 0.0254)
//   Volume     - raw L;   Metric L;                  Imperial UK gallons (raw / 4.54609), not US
// Everything else (angles, %, N, kW, clicks, ratios, force feedback) shows the same in both.
public enum SetupFieldQuantity
{
    None,
    Pressure,
    SpringRate,
    Length,
    Volume
}
