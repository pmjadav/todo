using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public class TodoService : ITodoService
    {
        private readonly ToDoContext _context;
        private readonly IMapper _mapper;
        public TodoService(ToDoContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TodoItemDTO>> GetAllTodoItems()
        {
            return await _context.TodoItem.Select(x => _mapper.Map<TodoItemDTO>(x)).ToListAsync();
        }

        public async Task<TodoItemDTO?> GetTodoItemById(long id)
        {
            if (_context.TodoItem == null)
            {
                return null;
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return null;
            }
            return _mapper.Map<TodoItemDTO>(todoItem);
        }
        public async Task<TodoItemDTO> AddTodoItem(TodoItemDTO todoDTO)
        {
            var todoItem = _mapper.Map<TodoItem>(todoDTO);
            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoItemDTO>(todoItem);
        }
        public async Task<TodoItemDTO?> UpdateTodoItem(long id, TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return null;
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if ( todoItem == null)
            {
                return null;
            }
            _mapper.Map<TodoItemDTO, TodoItem>(todoDTO, todoItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return todoDTO;
        }
        public async Task<TodoItemDTO?> DeleteTodoItem(long id)
        {
            if (_context.TodoItem == null)
            {
                return null;
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return null;
            }
            _context.TodoItem.Remove(todoItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return _mapper.Map<TodoItemDTO>(todoItem);
        }
        
    }
}
