#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Customs;

public class CustomCar
{
    [JsonPropertyName("auxLightColor")]
    public int AuxLightColor { get; set; }
    [JsonPropertyName("auxLightKey")]
    public int AuxLightKey { get; set; }
    [JsonPropertyName("bannerTemplateKey")]
    public int BannerTemplateKey { get; set; }
    [JsonPropertyName("carGuid")]
    public int CarGuid { get; set; }
    [JsonPropertyName("carModelType")]
    public int CarModelType { get; set; }
    [JsonPropertyName("competitorName")]
    public string CompetitorName { get; set; }
    [JsonPropertyName("competitorNationality")]
    public int CompetitorNationality { get; set; }
    [JsonPropertyName("competitorTeamName")]
    public int CupCategory { get; set; }
    [JsonPropertyName("customSkinName")]
    public string CustomSkinName { get; set; }
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
    [JsonPropertyName("FilePath")]
    public string FilePath { get; set; }
    [JsonPropertyName("licenseType")]
    public int LicenseType { get; set; }
    [JsonPropertyName("nationality")]
    public int Nationality { get; set; }
    [JsonPropertyName("raceNumber")]
    public int RaceNumber { get; set; }
    [JsonPropertyName("raceNumberPadding")]
    public int RaceNumberPadding { get; set; }
    [JsonPropertyName("rimColor1Id")]
    public int RimColor1Id { get; set; }
    [JsonPropertyName("rimColor2Id")]
    public int RimColor2Id { get; set; }
    [JsonPropertyName("rimMaterialType1")]
    public int RimMaterialType1 { get; set; }
    [JsonPropertyName("rimMaterialType2")]
    public int RimMaterialType2 { get; set; }
    [JsonPropertyName("skinColor1Id")]
    public int SkinColor1Id { get; set; }
    [JsonPropertyName("skinColor2Id")]
    public int SkinColor2Id { get; set; }
    [JsonPropertyName("skinColor3Id")]
    public int SkinColor3Id { get; set; }
    [JsonPropertyName("skinMaterialType1")]
    public int SkinMaterialType1 { get; set; }
    [JsonPropertyName("skinMaterialType2")]
    public int SkinMaterialType2 { get; set; }
    [JsonPropertyName("skinMaterialType3")]
    public int SkinMaterialType3 { get; set; }
    [JsonPropertyName("skinTemplateKey")]
    public int SkinTemplateKey { get; set; }
    [JsonPropertyName("sponsorId")]
    public int SponsorId { get; set; }
    [JsonPropertyName("teamGuid")]
    public int TeamGuid { get; set; }
    [JsonPropertyName("teamName")]
    public string TeamName { get; set; }
    [JsonPropertyName("teamTemplateKey")]
    public int TeamTemplateKey { get; set; }
    [JsonPropertyName("useEnduranceKit")]
    public int UseEnduranceKit { get; set; }
}