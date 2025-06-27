using ChocolateAPI.Data;
using ChocolateAPI.Entites;
using ChocolateAPI.Extensions;
using HotChocolate.Authorization;
using System;

namespace ChocolateAPI.GraphQL
{
    public class Mutation
    {
        // 1) Создать новую задачу
        [Authorize] // только аутентифицированные
        public async Task<TaskItem> CreateTaskAsync(
          Inputs.CreateTaskInput input,
          [Service] ChocolateDbContext db,
          [Service] IHttpContextAccessor http)
        {
            var userId = http.HttpContext!.User.GetUserId();
            var task = new TaskItem
            {
                Title = input.Title,
                Description = input.Description,
                CreatedById = userId,
                 Status = Data.TaskStatus.New,
                CreatedAt = DateTime.UtcNow
            };
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            return task;
        }

        // 2) Обновить поля задачи
        [Authorize]
        public async Task<TaskItem?> UpdateTaskAsync(
          Inputs.UpdateTaskInput input,
          [Service] ChocolateDbContext db)
        {
            var task = await db.Tasks.FindAsync(input.Id);
            if (task is null) return null;

            task.Title = input.Title;
            task.Description = input.Description;
            task.Status = input.Status;
            await db.SaveChangesAsync();
            return task;
        }

        // 3) Удалить задачу (только Admin)
        [Authorize(Roles = new[] { "Admin" })]
        public async Task<bool> DeleteTaskAsync(
          Guid id,
          [Service] ChocolateDbContext db)
        {
            var task = await db.Tasks.FindAsync(id);
            if (task is null) return false;
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
