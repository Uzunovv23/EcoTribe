﻿using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using EcoTribe.Services.Implementations;
using EcoTribe.Services.Interfaces;
using EcoTribe.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoTribe.Web.Controllers
{
    public class EventSponsorController : Controller
    {
        private readonly IEventSponsorService eventSponsorService;

        public EventSponsorController(IEventSponsorService eventSponsorService)
        {
            this.eventSponsorService = eventSponsorService;
        }
        public IActionResult Index()
        {
            List<EventSponsorViewModel> eventSponsors = eventSponsorService.GetAll().ToList();
            return View(eventSponsors);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventSponsorInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {

                eventSponsorService.Create(inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(inputModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the event sponsor. " + ex.Message);
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var eventSponsor = eventSponsorService.GetById(id);
            if (eventSponsor == null)
            {
                return NotFound();
            }

            var inputModel = ModelConverter.ConvertToModel<EventSponsorViewModel, EventSponsorInputModel>(eventSponsor);

            return View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventSponsorInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                eventSponsorService.Update(id, inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the event sponsor.");
                return View(inputModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var eventSponsor = eventSponsorService.GetById(id);
            if (eventSponsor == null)
            {
                return NotFound();
            }

            return View(eventSponsor);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                eventSponsorService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error deleting the event sponsor.");
            }
        }
    }
}
