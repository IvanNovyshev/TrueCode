using LinqToDB;
using LinqToDB.Data;

namespace TrueCode.UserService.Infrastructure;

public class UserContext : DataConnection
{
    public UserContext(DataOptions<UserContext> options) : base(options.Options)
    {
        
    }

    public ITable<UserDb> Users => this.GetTable<UserDb>();
}