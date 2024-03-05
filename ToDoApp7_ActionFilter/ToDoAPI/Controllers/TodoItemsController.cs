using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Filters;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    //[LogActionFilter]         // Apply LogActionFilter at Controller Level
    public class TodoItemsController : ControllerBase
    {
        private ITodoService _todoService;

        public TodoItemsController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/TodoItems
        [AllowAnonymous]
        [HttpGet]
        // [LogActionFilter]    // Apply LogActionFilter at Action Method Level
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllTodoItems()
        {            
            var todoItems = await _todoService.GetAllTodoItems();
            if (todoItems == null)
            {                
                return NotFound();
            }
            return Ok(todoItems);
        }

        // GET: api/TodoItems/1
        // Send Id as HTTP Route data
        [AllowAnonymous]
        [HttpGet("{id}")]
        // [TypeFilter<SampleExceptionFilter>]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItemById([FromRoute] long id)
        {
            var todoItem = await _todoService.GetTodoItemById(id);
            if (todoItem == null)
            {
                int x = 3; // To test the custom exception filter
                int y = 0;
                int z = x / y;
                return NotFound();
            }
            return Ok(todoItem);
        }

        // GET: api/TodoItems/get?id=1
        // Send Id as Query string parameter
        [HttpGet("get")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItemById2([FromQuery] long id)
        {
            var todoItem = await _todoService.GetTodoItemById(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(todoItem);
        }

        // POST: api/TodoItems
        // Send Todo Item in HTTP Request body
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] TodoItemDTO todoItemDTO)
        {
            var todoDTO = await _todoService.AddTodoItem(todoItemDTO);
            if (todoDTO == null)
            {
                return Problem("Unable to add the todo item!");
            }
            return CreatedAtAction(nameof(GetTodoItemById), new { id = todoDTO.Id }, todoDTO);
        }

        // POST: api/TodoItems/post?name=test&iscomplete=true
        [Route("post")]
        [HttpPost]
        // Send Todo Item in HTTP Request Query String Parameters
        public async Task<ActionResult<TodoItem>> PostTodoItem2([FromQuery] TodoItemDTO todoItemDTO)
        {
            var todoDTO = await _todoService.AddTodoItem(todoItemDTO);
            if (todoDTO == null)
            {
                return Problem("Unable to add the todo item!");
            }
            return CreatedAtAction(nameof(GetTodoItemById), new { id = todoDTO.Id }, todoDTO);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            var todoDTO = await _todoService.UpdateTodoItem(id, todoItemDTO);
            if (todoDTO == null)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoDTO = await _todoService.DeleteTodoItem(id);
            if ( todoDTO == null)
            {
                return NotFound($"Todo Item with Id {id} not found!");
            }
            return NoContent();
        }        
    }
}
