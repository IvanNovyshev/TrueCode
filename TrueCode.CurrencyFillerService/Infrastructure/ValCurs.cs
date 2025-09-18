using System.Globalization;
using System.Xml.Serialization;

namespace TrueCode.CurrencyFillerService.Infrastructure;

[XmlRoot(ElementName = "ValCurs")]
public class ValCurs
{
    [XmlElement(ElementName = "Valute")] public List<Valute> Valutes { get; init; } = [];

    //[XmlAttribute(AttributeName = "Date")] public DateTime Date { get; init; }

    [XmlAttribute(AttributeName = "name")] public string Name { get; init; } = string.Empty;

    [XmlAttribute("Date")] public string DateRaw { get; set; }

    [XmlIgnore]
    public DateTime Date =>
        DateTime.TryParseExact(DateRaw, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
            out var dateOnly)
            ? dateOnly
            : DateTime.MinValue;
}