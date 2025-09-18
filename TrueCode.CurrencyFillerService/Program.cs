using System.Text;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using TrueCode.CurrencyFillerService;
using TrueCode.CurrencyFillerService.Core;
using TrueCode.CurrencyFillerService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddLinqToDBContext<CurrenciesContext>((provider, options) =>
    options.UsePostgreSQL(builder.Configuration.GetConnectionString("FillerConnection") ??
                          throw new ArgumentException("Cannot obtain connection string"))
        .UseDefaultLogging(provider));

builder.Services.AddScoped<ICurrencyStorage, DatabaseCurrencyStorage>();

builder.Services.AddSingleton<ICbrService, CbrService>();
builder.Services.AddScoped<ICurrencySource, CbrCurrencySource>();


builder.Services.AddHostedService<MainService>();
builder.Services.AddHttpClient();

var host = builder.Build();
host.Run();