using System.Data;
using LinqToDB;
using LinqToDB.Data;
using TrueCode.CurrencyFillerService.Core;

namespace TrueCode.CurrencyFillerService.Infrastructure;

public class DatabaseCurrencyStorage : ICurrencyStorage
{
    private readonly CurrenciesContext _currenciesContext;

    public DatabaseCurrencyStorage(CurrenciesContext currenciesContext)
    {
        _currenciesContext = currenciesContext;
    }

    public async Task SaveAsync((DateTime Date, IEnumerable<Currency> Currencies) source, CancellationToken token)
    {
        await using var transaction =
            await _currenciesContext.BeginTransactionAsync(IsolationLevel.ReadCommitted, token);

        try
        {
            var toUpload = source.Currencies.Select(x => new CurrencyDb()
                { Name = x.Name, Rate = x.Rate });

            var currencyDbs = toUpload as CurrencyDb[] ??
                              toUpload as IReadOnlyCollection<CurrencyDb> ?? toUpload.ToArray();

            var first = currencyDbs.FirstOrDefault();

            if (first == null)
            {
                return;
            }

            await _currenciesContext.Currency.TruncateAsync(token: token);
            
            await _currenciesContext.FillerInfo.InsertOrUpdateAsync(() => new FillerServiceInfo
                    { Id = 1, LastUpdated = source.Date },
                info => new FillerServiceInfo { Id = 1, LastUpdated = source.Date },
                token: token);

            await _currenciesContext.Currency.BulkCopyAsync(currencyDbs, cancellationToken: token);

            await transaction.CommitAsync(token);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(token);
        }
    }

    public async Task<DateTime> LastUpdated(CancellationToken token)
    {
        return (await _currenciesContext.FillerInfo.FirstOrDefaultAsync(token: token))?.LastUpdated ??
               DateTime.MinValue;
    }
}