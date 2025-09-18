using LinqToDB;
using LinqToDB.Data;

namespace TrueCode.CurrencyFillerService.Infrastructure;

public class CurrenciesContext : DataConnection
{
    public CurrenciesContext(DataOptions<CurrenciesContext> options) : base(options.Options)
    {
    }

    public ITable<CurrencyDb> Currency => this.GetTable<CurrencyDb>();
    public ITable<FillerServiceInfo> FillerInfo => this.GetTable<FillerServiceInfo>();
}