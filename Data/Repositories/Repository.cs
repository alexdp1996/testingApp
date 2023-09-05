using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Base
    {

        protected readonly DbContext _dbContext;

        public Repository(Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var newEntry = (await _dbContext.Set<TEntity>().AddAsync(entity)).Entity;
            await _dbContext.SaveChangesAsync();
            return newEntry;
        }

        public async Task<IEnumerable<TEntity>> ReadAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> ReadAsync(Guid id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                throw new NotFoundException();
            }

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var type = entity.GetType();
            var fields = entity.GetType()
                .GetProperties()
                .Select(prop => (prop.Name, Value: prop.PropertyType.IsEnum ? (int) prop.GetValue(entity) : prop.GetValue(entity)))
                .ToList();

            var tableName = _dbContext.Model.FindEntityType(type).GetTableName();
            var sqlParams = fields.Select(f => f.Value.GetType() == typeof(DateTime) ? GetDatetime2($"@{f.Name}", (DateTime) f.Value) : new SqlParameter($"@{f.Name}", f.Value)).ToArray();

            var t = await _dbContext.Set<TEntity>().FromSqlRaw($"Select * from orders").ToListAsync();

            var query = $"UPDATE T SET {string.Join(",", fields.Where(f => f.Name != "Id" && f.Name != "LastUpdated").Select(f => $"T.{f.Name} = @{f.Name}"))}, T.LastUpdated = GETUTCDATE() OUTPUT inserted.* FROM {tableName} T WHERE T.Id = @Id AND T.LastUpdated = @LastUpdated;";
            var queryResult = await _dbContext.Set<TEntity>().FromSqlRaw($"{query}", sqlParams).ToListAsync();
            var result = queryResult.FirstOrDefault();

            if (result == null) 
            {
                throw new ConcurencyUpdateException();
            }

            return result;
        }

        private static SqlParameter GetDatetime2(string name, DateTime date)
        {
            var param = new SqlParameter(parameterName: name, dbType: SqlDbType.DateTime2);
            param.Value = date;
            return param;
        }
    }
}
