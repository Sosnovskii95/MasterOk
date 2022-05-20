﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using MasterOk.Models.FilterSortViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    [Authorize(Roles = "user")]
    public class UsersController : Controller
    {
        private readonly DataBaseContext _context;

        public UsersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(ESortModelUsers sort)
        {
            IQueryable<User> dataBaseContext = _context.Users;

            dataBaseContext = sort switch
            {
                ESortModelUsers.IdAsc => dataBaseContext.OrderBy(s => s.Id),
                ESortModelUsers.IdDesc => dataBaseContext.OrderByDescending(s => s.Id),
                ESortModelUsers.ActiveAsc => dataBaseContext.OrderBy(s => s.ActiveUser),
                ESortModelUsers.ActiveDesc => dataBaseContext.OrderByDescending(s => s.ActiveUser),
                ESortModelUsers.AgeAsc => dataBaseContext.OrderBy(s => s.Age),
                ESortModelUsers.AgeDesc => dataBaseContext.OrderByDescending(s => s.Age),
                ESortModelUsers.EmailAsc => dataBaseContext.OrderBy(s => s.EmailUser),
                ESortModelUsers.EmailDesc => dataBaseContext.OrderByDescending(s => s.EmailUser),
                ESortModelUsers.LoginAsc => dataBaseContext.OrderBy(s => s.LoginUser),
                ESortModelUsers.LoginDesc => dataBaseContext.OrderByDescending(s => s.LoginUser),
                ESortModelUsers.NameAsc => dataBaseContext.OrderBy(s => s.FirstLastNameStaff),
                ESortModelUsers.NameDesc => dataBaseContext.OrderByDescending(s => s.FirstLastNameStaff),
                ESortModelUsers.NumberPhoneAsc => dataBaseContext.OrderBy(s => s.NumberPhoneStaff),
                ESortModelUsers.NumberPhoneDesc => dataBaseContext.OrderByDescending(s => s.NumberPhoneStaff),
                _ => dataBaseContext
            };

            return View(new SortViewModelUsers
            {
                Users = await dataBaseContext.ToListAsync(),
                SortModelUsers = new SortModelUsers(sort)
            });
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmailUser,LoginUser,PasswordUser,FirstLastNameStaff,Age,NumberPhoneStaff, ActiveUser")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmailUser,LoginUser,PasswordUser,FirstLastNameStaff,Age,NumberPhoneStaff,ActiveUser")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
