﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollExpertApp.Data;

namespace PayrollExpertApp.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PayrollDbContext _context;

        public PeopleController(PayrollDbContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.People.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(_context.Companies, "Id", "Name", _context.Companies.FirstOrDefault().Id);
            ViewBag.PayrollTypes = new SelectList(Utilities.GetDropDownSource(_context, "PayrollPeriodFrequency", true), "Value", "Text");
            ViewBag.RemittanceTypes = new SelectList(Utilities.GetDropDownSource(_context, "RemittancePeriodFrequency", true), "Value", "Text");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,CompanyId,SIN,Birthday,PayrollType,RemittanceType,ContractCopied,Comment,StartDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new {id = person.Id});
            }
            ViewBag.CompanyId = new SelectList(_context.Companies, "Id", "Name", _context.Companies.FirstOrDefault().Id);
            ViewBag.PayrollTypes = new SelectList(Utilities.GetDropDownSource(_context, "PayrollPeriodFrequency", true), "Value", "Text", person.PayrollType);
            ViewBag.RemittanceTypes = new SelectList(Utilities.GetDropDownSource(_context, "RemittancePeriodFrequency", true), "Value", "Text", person.RemittanceType);
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.CompanyId = new SelectList(_context.Companies, "Id", "Name", person.CompanyId);
            ViewBag.PayrollTypes = new SelectList(Utilities.GetDropDownSource(_context, "PayrollPeriodFrequency",true), "Value", "Text", person.PayrollType);
            ViewBag.RemittanceTypes = new SelectList(Utilities.GetDropDownSource(_context, "RemittancePeriodFrequency", true), "Value", "Text", person.RemittanceType);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,CompanyId,SIN,Birthday,PayrollType,RemittanceType,ContractCopied,Comment,StartDate")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            ViewBag.CompanyId = new SelectList(_context.Companies, "Id", "Name", person.CompanyId);
            ViewBag.PayrollTypes = new SelectList(Utilities.GetDropDownSource(_context, "PayrollPeriodFrequency", true), "Value", "Text", person.PayrollType);
            ViewBag.RemittanceTypes = new SelectList(Utilities.GetDropDownSource(_context, "RemittancePeriodFrequency", true), "Value", "Text", person.RemittanceType);
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.SingleOrDefaultAsync(m => m.Id == id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
