namespace OfflineDemo.Models;

public class TodoModel
{
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string ToDoItem { get; set; }
    public bool IsComplete { get; set; }
}
