namespace ToDoList.Models
{
    public class PriorityModel
    {
        public int ToDoItemId { get; set; }
        public int Level { get; set; }

        public ToDoItemModel ToDoItem { get; set; }
    }
}
