using ChocolateAPI.Data;
using ChocolateAPI.Entites;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChocolateAPI.GraphQL
{
    public class Query
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TaskItem> GetTasks([Service] IDbContextFactory<ChocolateDbContext> dbf)
        => dbf.CreateDbContext().Tasks;

        public TaskItem GetTaskById(
            Guid id,
            [Service] IDbContextFactory<ChocolateDbContext> dbf)
            => dbf.CreateDbContext().Tasks
                   .Include(t => t.CreatedBy)
                   .First(t => t.Id == id);

        [Authorize(Roles = new[] { nameof(UserRole.Admin) })]
        [UsePaging]
        [UseSorting]
        public IQueryable<User> GetUsers([Service] IDbContextFactory<ChocolateDbContext> dbf)
            => dbf.CreateDbContext().Users;
    }

}
