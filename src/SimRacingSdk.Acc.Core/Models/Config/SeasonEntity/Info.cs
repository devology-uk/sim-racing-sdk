#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Info
{
    [JsonPropertyName("aiAggro")]
    public int AiAggro { get; init; }
    [JsonPropertyName("aiConsistency")]
    public int AiConsistency { get; init; }
    [JsonPropertyName("aiRainSkill")]
    public int AiRainSkill { get; init; }
    [JsonPropertyName("aiSkill")]
    public int AiSkill { get; init; }
    [JsonPropertyName("auxLightColor")]
    public int AuxLightColor { get; init; }
    [JsonPropertyName("auxLightKey")]
    public int AuxLightKey { get; init; }
    [JsonPropertyName("bannerTemplateKey")]
    public int BannerTemplateKey { get; init; }
    [JsonPropertyName("carGuid")]
    public int CarGuid { get; init; }
    [JsonPropertyName("carModelType")]
    public int CarModelType { get; init; }
    [JsonPropertyName("competitorName")]
    public string CompetitorName { get; init; }
    [JsonPropertyName("competitorNationality")]
    public int CompetitorNationality { get; init; }
    [JsonPropertyName("cupCategory")]
    public int CupCategory { get; init; }
    [JsonPropertyName("customSkinName")]
    public string CustomSkinName { get; init; }
    [JsonPropertyName("displayName")]
    public string DisplayName { get; init; }
    [JsonPropertyName("driverCategory")]
    public int DriverCategory { get; init; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; init; }
    [JsonPropertyName("GlovesTemplateKey")]
    public int GlovesTemplateKey { get; init; }
    [JsonPropertyName("helmetBaseColor")]
    public int HelmetBaseColor { get; init; }
    [JsonPropertyName("helmetDetailColor")]
    public int HelmetDetailColor { get; init; }
    [JsonPropertyName("helmetGlassColor")]
    public int HelmetGlassColor { get; init; }
    [JsonPropertyName("helmetGlassMetallic")]
    public double HelmetGlassMetallic { get; init; }
    [JsonPropertyName("helmetMaterialType")]
    public int HelmetMaterialType { get; init; }
    [JsonPropertyName("helmetTemplateKey")]
    public int HelmetTemplateKey { get; init; }
    [JsonPropertyName("lastName")]
    public string LastName { get; init; }
    [JsonPropertyName("licenseType")]
    public int LicenseType { get; init; }
    [JsonPropertyName("nationality")]
    public int Nationality { get; init; }
    [JsonPropertyName("nickName")]
    public string NickName { get; init; }
    [JsonPropertyName("playerID")]
    public string PlayerID { get; init; }
    [JsonPropertyName("raceNumber")]
    public int RaceNumber { get; init; }
    [JsonPropertyName("raceNumberPadding")]
    public int RaceNumberPadding { get; init; }
    [JsonPropertyName("rimColor1Id")]
    public int RimColor1Id { get; init; }
    [JsonPropertyName("rimColor2Id")]
    public int RimColor2Id { get; init; }
    [JsonPropertyName("rimMaterialType1")]
    public int RimMaterialType1 { get; init; }
    [JsonPropertyName("rimMaterialType2")]
    public int RimMaterialType2 { get; init; }
    [JsonPropertyName("shortName")]
    public string ShortName { get; init; }
    [JsonPropertyName("skinColor1Id")]
    public int SkinColor1Id { get; init; }
    [JsonPropertyName("skinColor2Id")]
    public int SkinColor2Id { get; init; }
    [JsonPropertyName("skinColor3Id")]
    public int SkinColor3Id { get; init; }
    [JsonPropertyName("skinMaterialType1")]
    public int SkinMaterialType1 { get; init; }
    [JsonPropertyName("skinMaterialType2")]
    public int SkinMaterialType2 { get; init; }
    [JsonPropertyName("skinMaterialType3")]
    public int SkinMaterialType3 { get; init; }
    [JsonPropertyName("skinTemplateKey")]
    public int SkinTemplateKey { get; init; }
    [JsonPropertyName("sponsorId")]
    public int SponsorId { get; init; }
    [JsonPropertyName("suitDetailColor1")]
    public int SuitDetailColor1 { get; init; }
    [JsonPropertyName("suitDetailColor2")]
    public int SuitDetailColor2 { get; init; }
    [JsonPropertyName("suitDetailColor3")]
    public int SuitTemplateKey { get; init; }
    [JsonPropertyName("teamGuid")]
    public int TeamGuid { get; init; }
    [JsonPropertyName("teamName")]
    public string TeamName { get; init; }
    [JsonPropertyName("teamTemplateKey")]
    public int TeamTemplateKey { get; init; }
    [JsonPropertyName("useEnduranceKit")]
    public int UseEnduranceKit { get; init; }
}