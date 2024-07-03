using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoItemModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string IsCompleted { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        [Required]
        [Range(1, 3)]
        public int Priority { get; set; }
        public int UserId { get; set; }

        public UserModel User { get; set; }
        public PriorityModel PriorityModel { get; set; }
    }
}
