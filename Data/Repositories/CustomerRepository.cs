using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(Context dbContext) : base(dbContext)
        {
        }

        public async Task UpdateCountAsync(Guid id, long diff)
        {
            var tableName = _dbContext.Model.FindEntityType(typeof(Customer)).GetTableName();
            var sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Diff", diff)
            };
            var query = $"UPDATE T SET T.OrderCount = T.OrderCount + @Diff OUTPUT inserted.* FROM {tableName} T WHERE Id = @Id";
            await _dbContext.Set<Customer>().FromSqlRaw(query, sqlParams).ToListAsync();
        }
    }
}
