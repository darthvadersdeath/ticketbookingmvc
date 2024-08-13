﻿using Microsoft.AspNetCore.Mvc;
using TicketMasterMVC.Models;

namespace TicketMasterMVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApiService _apiService;

        public TicketsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = await _apiService.GetTicketsAsync();
            return View(tickets);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateTicketAsync(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _apiService.GetTicketAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateTicketAsync(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _apiService.GetTicketAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket); // This will return the delete confirmation view with the ticket details
        }

        // POST: Tickets/Delete/5
        [HttpPost]
        [ActionName("Delete")] // This matches the delete action method in your view
        [ValidateAntiForgeryToken] // This helps prevent CSRF attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteTicketAsync(id); // Call the method directly

            // Since it doesn't return a value, you can redirect after the call
            return RedirectToAction(nameof(Index)); // Redirect to the index action after deletion
        }

    }
}
