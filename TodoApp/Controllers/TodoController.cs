using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;
using Persistence.Repositories;
using TodoApp.Data;

namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
            return View(await _todoRepository.GetAllAsync());
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoModel = await _todoRepository.Get(id);
            if (todoModel == null)
            {
                return NotFound();
            }

            return View(todoModel);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DateCreated")] TodoModel todoModel)
        {
            if (ModelState.IsValid)
            {
                todoModel.Id = Guid.NewGuid();
                todoModel.DateCreated = DateTime.Now;

                await _todoRepository.SaveOrUpdate(todoModel);
                
                return RedirectToAction(nameof(Index));
            }
            return View(todoModel);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoModel = await _todoRepository.Get(id);
            if (todoModel == null)
            {
                return NotFound();
            }
            return View(todoModel);
        }

        // POST: Todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,DateCreated")] TodoModel todoModel)
        {
            if (id != todoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _todoRepository.SaveOrUpdate(todoModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await TodoModelExists(todoModel.Id)))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoModel);
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoModel = await _todoRepository.Get(id);
            if (todoModel == null)
            {
                return NotFound();
            }

            return View(todoModel);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _todoRepository.Delete(id);
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TodoModelExists(Guid id)
        {
            return (await _todoRepository.Get(id)) is not null;
        }
    }
}
