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
    public class CompanyController : Controller
    {
        private readonly PayrollDbContext _context;

        public CompanyController(PayrollDbContext context)
        {
            _context = context;
        }

        // GET: Company
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Companies.ToListAsync());
        //}

        //// GET: Company/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var company = await _context.Companies
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(company);
        //}

        //// GET: Company/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Company/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,OperatingName,CBABusinessNumber,RegistrationDate,MailingAddressSameAsHeadOfficeAddress,WebsiteURL,Email,SigningOfficer,Directors")] Company company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(company);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(company);
        //}

        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var company = await _context.Companies.SingleOrDefaultAsync(m => m.Id == id);
            var company = await _context.Companies.Include(c=>c.Addresses).Include(e=>e.People).FirstOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }
            //Directly load from shareholder set for only 1 company.
            ViewBag.ShareHolders = _context.ShareHolders.Include(x=>x.Person).ToList();
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OperatingName,CBABusinessNumber,RegistrationDate,MailingAddressSameAsHeadOfficeAddress,WebsiteURL,Email,SigningOfficer,Directors")] Company company, string postvalue)
        {
            if (postvalue == "Save")
            {
                if (id != company.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(company);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!CompanyExists(company.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            ModelState.AddModelError(ex.Message, "Company saving error.");
                        }
                    }
                }
                company.Addresses = _context.Addresses.Where(x => x.CompanyId == company.Id).ToList();
                company.People = _context.People.Where(x => x.CompanyId == company.Id).ToList();
                return View(company);
            }

            //Default action
            return View(company);
        }

        //// GET: Company/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var company = await _context.Companies
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(company);
        //}

        //// POST: Company/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var company = await _context.Companies.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Companies.Remove(company);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
