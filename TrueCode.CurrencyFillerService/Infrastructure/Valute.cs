using System.Globalization;
using System.Xml.Serialization;

namespace TrueCode.CurrencyFillerService.Infrastructure;

[XmlRoot(ElementName = "Valute")]
public class Valute
{
    private static NumberFormatInfo _formatInfo = new() { CurrencyDecimalSeparator = "," };

    [XmlAttribute(AttributeName = "ID")] public required string Id { get; init; }

    [XmlElement(ElementName = "NumCode")] public required int NumCode { get; init; }

    [XmlElement(ElementName = "CharCode")] public required string CharCode { get; init; }

    [XmlElement(ElementName = "Nominal")] public required int Nominal { get; init; }

    [XmlElement(ElementName = "Name")] public required string Name { get; init; }

    [XmlElement(ElementName = "Value")] public required string ValueRaw { get; init; }

    [XmlElement(ElementName = "VunitRate")]
    public required string VunitRate { get; init; }

    [XmlIgnore] public decimal Value => decimal.Parse(ValueRaw, NumberStyles.Currency, _formatInfo);
}