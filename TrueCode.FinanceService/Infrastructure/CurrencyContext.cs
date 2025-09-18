using LinqToDB;
using LinqToDB.Data;

namespace TrueCode.FinanceService;

public class CurrencyContext : DataConnection
{
    public CurrencyContext(DataOptions<CurrencyContext> options) : base(options.Options)
    {
    }

    public ITable<FavoritesDb> Favorites => this.GetTable<FavoritesDb>();
    public ITable<CurrencyDb> Currencies => this.GetTable<CurrencyDb>();
}