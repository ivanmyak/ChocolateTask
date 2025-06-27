namespace ChocolateAPI.GraphQL.Inputs
{
    public record UpdateTaskInput(
        Guid Id,
        string Title,
        string Description,
        ChocolateAPI.Data.TaskStatus Status
      );
}
