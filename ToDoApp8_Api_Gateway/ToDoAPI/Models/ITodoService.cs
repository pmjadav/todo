namespace ToDoAPI.Models
{
    public interface ITodoService
    {
        public Task<IEnumerable<TodoItemDTO>> GetAllTodoItems(); 
        public Task<TodoItemDTO?> GetTodoItemById(long id);
        
        public Task<TodoItemDTO> AddTodoItem(TodoItemDTO todoDTO);
        public Task<TodoItemDTO?> UpdateTodoItem(long id, TodoItemDTO todoDTO);
        public Task<TodoItemDTO?> DeleteTodoItem(long id);
    }
}
