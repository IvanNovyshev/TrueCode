using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TrueCode.CurrencyFillerService.Infrastructure;

public class CbrService : ICbrService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly XmlSerializer _xmlSerializer;

    public CbrService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _xmlSerializer = new XmlSerializer(typeof(ValCurs));
    }

    public async Task<DateTime> AvailableDateAsync(CancellationToken token = default)
    {
        var allCurrencies = await AllCurrenciesAsync(token);

        return allCurrencies.Date;
    }

    public async Task<ValCurs> AllCurrenciesAsync(CancellationToken token = default)
    {
        using var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("http://www.cbr.ru/");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        client.DefaultRequestHeaders.Add("Accept", "application/xml,text/xml;q=0.9,*/*;q=0.8");
        client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        client.DefaultRequestHeaders.Add("Connection", "keep-alive");

        var response = await client.GetAsync("/scripts/XML_daily.asp", token);


        response.EnsureSuccessStatusCode();

        await using var xml = await response.Content.ReadAsStreamAsync(token);

        var result = _xmlSerializer.Deserialize(xml) as ValCurs ?? throw new SerializationException();

        return result;
    }
}