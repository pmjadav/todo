using AutoMapper;

namespace ToDoAPI.Models
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemDTO>()
                .ReverseMap();
        }
    }
}
