using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core;

// Sourced from the AC Evo Dedicated Server's cars.json (name + display_name fields only, per
// Mike 2026-07-27). That file has no numeric car ID, so FindByModelId treats the UDP
// CarModelType byte as an index into this array's order - an unverified assumption pending
// confirmation against real broadcasting traffic. FindByName is the reliable lookup.
public class AceCarInfoProvider : IAceCarInfoProvider
{
    private static AceCarInfoProvider? singletonInstance;

    private readonly List<AceCarInfo> cars =
    [
        new() { Name = "preset_695b_mech_1", DisplayName = "Abarth 695 Biposto - Standard" },
        new() { Name = "preset_75t_mech_1", DisplayName = "Alfa Romeo 75 Turbo Evoluzione - Standard" },
        new() { Name = "preset_sprintgta_mech_1", DisplayName = "Alfa Romeo Giulia Sprint GTA - Standard" },
        new() { Name = "preset_gtam_mech_1", DisplayName = "Alfa Romeo Giulia GTAm - Standard" },
        new() { Name = "preset_mln_mech_1", DisplayName = "Alfa Romeo Junior - Elettrica 280 CV Veloce" },
        new() { Name = "preset_a110s_mech_1", DisplayName = "Alpine A110 S - Aero Package" },
        new() { Name = "preset_a290b_mech_1", DisplayName = "Alpine A290 Beta - Standard" },
        new() { Name = "preset_r8gt3_mech_1", DisplayName = "Audi R8 LMS GT3 Evo II - GT3" },
        new() { Name = "preset_r8gt4_mech_1", DisplayName = "Audi R8 LMS GT4 Evo - GT4" },
        new() { Name = "preset_rs3_mech_1", DisplayName = "Audi RS 3 Sportback - Sportback" },
        new() { Name = "preset_rs6_mech_1", DisplayName = "Audi RS 6 Avant - Standard" },
        new() { Name = "preset_sq_mech_1", DisplayName = "Audi Sport quattro - Standard" },
        new() { Name = "preset_m2_mech_1", DisplayName = "BMW M2 Coupe - Standard" },
        new() { Name = "preset_m2_mech_2", DisplayName = "BMW M2 Coupe - Performance" },
        new() { Name = "preset_m2csr_mech_1", DisplayName = "BMW M2 CS Racing - 350" },
        new() { Name = "preset_m2csr_mech_2", DisplayName = "BMW M2 CS Racing - 450" },
        new() { Name = "preset_m3evoiii_mech_1", DisplayName = "BMW M3 E30 Sport Evo (Evolution III) - Standard" },
        new() { Name = "preset_e46csl_mech_1", DisplayName = "BMW M3 E46 CSL - Standard" },
        new() { Name = "preset_m4csl_mech_1", DisplayName = "BMW M4 CSL - Standard" },
        new() { Name = "preset_m4gt3_mech_1", DisplayName = "BMW M4 GT3 Evo - GT3" },
        new() { Name = "preset_m8c_mech_1", DisplayName = "BMW M8 Competition - Standard" },
        new() { Name = "ks_caterham_485_csr_mech_1", DisplayName = "Caterham 485 CSR - Road" },
        new() { Name = "ks_caterham_485_csr_mech_2", DisplayName = "Caterham 485 CSR - Track" },
        new() { Name = "ks_caterham_485_csr_mech_3", DisplayName = "Caterham 485 CSR - Final Edition" },
        new() { Name = "ks_caterham_acmd_mech_1", DisplayName = "Caterham Academy - Academy" },
        new() { Name = "preset_zl1_mech_1", DisplayName = "Chevrolet Camaro ZL1 - Standard" },
        new() { Name = "preset_zl1_mech_2", DisplayName = "Chevrolet Camaro ZL1 - 1LE" },
        new() { Name = "preset_dalexp_mech_1", DisplayName = "Dallara EXP - Standard" },
        new() { Name = "preset_dalsc_mech_1", DisplayName = "Dallara Stradale - Coupe" },
        new() { Name = "preset_dalsc_mech_2", DisplayName = "Dallara Stradale - Spider" },
        new() { Name = "preset_dalsc_mech_3", DisplayName = "Dallara Stradale - Track" },
        new() { Name = "preset_240z_mech_1", DisplayName = "Datsun 240Z (S30) - Standard" },
        new() { Name = "preset_240z_mech_2", DisplayName = "Datsun 240Z (S30) - Tuned" },
        new() { Name = "preset_288gto_mech_1", DisplayName = "Ferrari 288 GTO - Standard" },
        new() { Name = "preset_296gt3_mech_1", DisplayName = "Ferrari 296 GT3 - GT3" },
        new() { Name = "preset_296gtb_mech_1", DisplayName = "Ferrari 296 GTB - Standard" },
        new() { Name = "preset_296gtb_mech_2", DisplayName = "Ferrari 296 GTB - Assetto Fiorano" },
        new() { Name = "preset_f488ce_mech_1", DisplayName = "Ferrari 488 Challenge Evo - Ferrari Challenge" },
        new() { Name = "preset_sp3_mech_1", DisplayName = "Ferrari Daytona SP3 - Standard" },
        new() { Name = "preset_f2004_mech_1", DisplayName = "Ferrari F2004 - Standard" },
        new() { Name = "preset_f40lm_mech_1", DisplayName = "Ferrari F40 LM - Standard" },
        new() { Name = "preset_sf25_mech_1", DisplayName = "Ferrari SF-25 - F1 2025" },
        new() { Name = "preset_escortrs_mech_1", DisplayName = "Ford Escort RS Cosworth - Standard" },
        new() { Name = "preset_msggt3_mech_1", DisplayName = "Ford Mustang GT3 - GT3" },
        new() { Name = "preset_nsxr_mech_1", DisplayName = "Honda NSX-R 92R - Standard" },
        new() { Name = "preset_s2000ap1_mech_1", DisplayName = "Honda S2000 AP1 - Standard" },
        new() { Name = "preset_i30n_hb_mech_1", DisplayName = "Hyundai i30 N Hatchback - Performance" },
        new() { Name = "preset_i30n_hb_mech_2", DisplayName = "Hyundai i30 N Hatchback - Performance N Limited" },
        new() { Name = "preset_xbgt2_mech_1", DisplayName = "KTM X-Bow GT2 - GT2" },
        new() { Name = "preset_xbgt4_mech_1", DisplayName = "KTM X-Bow GT4 - GT4" },
        new() { Name = "preset_cntqv_mech_1", DisplayName = "Lamborghini Countach LP5000 QV - LP5000 QV" },
        new() { Name = "preset_stevo2_mech_1", DisplayName = "Lamborghini Huracán ST EVO2 - Super Trofeo" },
        new() { Name = "preset_sto_mech_1", DisplayName = "Lamborghini Huracán STO - Road" },
        new() { Name = "preset_sto_mech_2", DisplayName = "Lamborghini Huracán STO - Trackday" },
        new() { Name = "preset_hfevoii_mech_1", DisplayName = "Lancia Delta HF integrale Evoluzione II - Standard" },
        new() { Name = "preset_hfevoii_mech_2", DisplayName = "Lancia Delta HF integrale Evoluzione II - Evo 3 \"Violet\"" },
        new() { Name = "preset_emira_mech_1", DisplayName = "Lotus Emira - Touring" },
        new() { Name = "preset_emira_mech_2", DisplayName = "Lotus Emira - Sports" },
        new() { Name = "preset_exigev6_mech_1", DisplayName = "Lotus Exige V6 Cup - Standard" },
        new() { Name = "preset_exigev6_mech_2", DisplayName = "Lotus Exige V6 Cup - Lotus Motorsport" },
        new() { Name = "preset_mcgt2_mech_1", DisplayName = "Maserati GT2 - GT2" },
        new() { Name = "preset_mx5na_mech_1", DisplayName = "Mazda MX-5 NA - Standard" },
        new() { Name = "preset_mx5na_mech_2", DisplayName = "Mazda MX-5 NA - RS Limited" },
        new() { Name = "preset_mx5ndcup_mech_1", DisplayName = "Mazda MX-5 ND Cup - Global Cup ND1" },
        new() { Name = "preset_mx5ndcup_mech_2", DisplayName = "Mazda MX-5 ND Cup - Global Cup ND2" },
        new() { Name = "preset_190e_mech_1", DisplayName = "Mercedes-Benz 190E 2.5-16 Evo II - Standard" },
        new() { Name = "preset_amggt2_mech_1", DisplayName = "Mercedes-AMG GT2 - GT2" },
        new() { Name = "preset_jcs_mech_1", DisplayName = "Mini John Cooper S - Standard" },
        new() { Name = "preset_jcs_mech_2", DisplayName = "Mini John Cooper S - Tune" },
        new() { Name = "preset_t16_mech_1", DisplayName = "Peugeot 205 T16 - Standard" },
        new() { Name = "preset_gt4csmr_mech_1", DisplayName = "Porsche 718 Cayman GT4 Clubsport - GT4" },
        new() { Name = "preset_718gt4rs_mech_1", DisplayName = "Porsche 718 Cayman GT4 RS - Standard" },
        new() { Name = "preset_718gt4rs_mech_2", DisplayName = "Porsche 718 Cayman GT4 RS - Weissach" },
        new() { Name = "preset_935_mech_1", DisplayName = "Porsche 935 - GT2" },
        new() { Name = "preset_964_mech_1", DisplayName = "Porsche 911 Turbo 3.6 (964) - Standard" },
        new() { Name = "preset_964_mech_2", DisplayName = "Porsche 911 Turbo 3.6 (964) - X88" },
        new() { Name = "preset_gt2rscs_mech_1", DisplayName = "Porsche 911 GT2 RS Clubsport Evo (991II) - GT2" },
        new() { Name = "preset_992c_mech_1", DisplayName = "Porsche 911 GT3 Cup (992) - ABS TC" },
        new() { Name = "preset_992c_mech_2", DisplayName = "Porsche 911 GT3 Cup (992) - ABS" },
        new() { Name = "preset_992c_mech_3", DisplayName = "Porsche 911 GT3 Cup (992) - No ABS No TC" },
        new() { Name = "preset_992ren_mech_1", DisplayName = "Porsche 911 GT3 R Rennsport (992) - GT3" },
        new() { Name = "preset_992ren_mech_2", DisplayName = "Porsche 911 GT3 R Rennsport (992) - Unrestricted" },
        new() { Name = "preset_gt3rs_mech_1", DisplayName = "Porsche 911 GT3 RS (992) - Clubsport" },
        new() { Name = "preset_gt3rs_mech_2", DisplayName = "Porsche 911 GT3 RS (992) - Weissach" },
        new() { Name = "preset_r5gt_mech_1", DisplayName = "Renault 5 GT Turbo - Standard" },
        new() { Name = "preset_ae86_mech_1", DisplayName = "Toyota Sprinter Trueno 1600GT-Apex (AE86) - Zenki" },
        new() { Name = "preset_ae86_mech_2", DisplayName = "Toyota Sprinter Trueno 1600GT-Apex (AE86) - Kouki" },
        new() { Name = "preset_ae86_mech_3", DisplayName = "Toyota Sprinter Trueno 1600GT-Apex (AE86) - Kouki Black Limited" },
        new() { Name = "preset_ae86_mech_4", DisplayName = "Toyota Sprinter Trueno 1600GT-Apex (AE86) - Anime Tribute" },
        new() { Name = "preset_gr86_mech_1", DisplayName = "Toyota GR86 - Standard" },
        new() { Name = "preset_gr86_mech_2", DisplayName = "Toyota GR86 - Trueno Edition" },
        new() { Name = "preset_gr86_mech_3", DisplayName = "Toyota GR86 - Hakone Edition" },
        new() { Name = "preset_supramkiv_mech_1", DisplayName = "Toyota Supra MKIV - Standard" },
        new() { Name = "preset_supramkiv_mech_2", DisplayName = "Toyota Supra MKIV - Drift" },
        new() { Name = "preset_gtimk1_mech_1", DisplayName = "Volkswagen Golf GTI Mk1 - Standard" },
        new() { Name = "preset_mk8gti_mech_1", DisplayName = "Volkswagen Golf 8 GTI - Clubsport" },
        new() { Name = "preset_mk8r_mech_1", DisplayName = "Volkswagen Golf 8 R - Standard" }
    ];

    public static AceCarInfoProvider Instance => singletonInstance ??= new AceCarInfoProvider();

    public AceCarInfo? FindByModelId(int modelId)
    {
        return modelId >= 0 && modelId < this.cars.Count ? this.cars[modelId] : null;
    }

    public AceCarInfo? FindByName(string name)
    {
        return this.cars.FirstOrDefault(c => c.Name == name);
    }

    public IReadOnlyCollection<AceCarInfo> GetCarInfos()
    {
        return this.cars.AsReadOnly();
    }
}
