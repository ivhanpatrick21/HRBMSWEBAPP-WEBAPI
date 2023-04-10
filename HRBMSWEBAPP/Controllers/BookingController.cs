﻿using HRBMSWEBAPP.Data;
using HRBMSWEBAPP.Models;
using HRBMSWEBAPP.Repository;
using HRBMSWEBAPP.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRBMSWEBAPP.Controllers
{
    //[Authorize(Roles ="Admin, User")]
    public class BookingController : Controller
    {

       private readonly IBookingDBRepository _repo;
        IRoomDBRepository _Roomrepo;
        HRBMSDBCONTEXT _context;

        public BookingController(IBookingDBRepository repo, IRoomDBRepository Roomrepo, HRBMSDBCONTEXT context)
        {
            this._repo = repo;
            _Roomrepo = Roomrepo;
            _context = context;
        }

        //public IActionResult GetAllBookings()
        //{
        //    var booklist = _repo.GetAllBooking();
        //    return View(booklist);

        //}

        public async Task<IActionResult> GetAllBookings()
        {
            var rooms = _context.Room.ToList();
            var room = new Room { Status = true };
            //var availableDisplayString = room.DisplayStatus;
            //var bookedDisplayString = "Booked";

            List<Booking> booking = await this._repo.GetAllBooking();
            return View(booking);
        }

        //public async Task<IActionResult> GetAllBookings()
        //{

        //    List<Booking> booking = await this._repo.GetAllBooking();
        //    return View(booking);
        //}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking booking = await this._repo.GetBookingById((int)id);
            return View(booking);
        }


        //public IActionResult Details(int bookId)
        //{
        //    var book = _repo.GetBookingById(bookId);
        //    return View(book);
        //}

        public IActionResult Delete(int id)
        {
            var booklist = _repo.DeleteBooking(id);
            return RedirectToAction(controllerName: "Booking", actionName: "GetAllBookings");
        }

        [HttpGet]
        public IActionResult Create()
        {
           
            //var rooms = _repo.GetBookingById();
            //var roomModel = new Room
            //{
            //    RoomList = room.Select(r => new SelectListItem
            //    {
            //        Text = r.Name,
            //        Value = r.Id.ToString(),
            //        Selected = (role != null && r.Id == role.Id)
            //    })
            //};
            //ViewBag.RoleList = roleViewModel.RoleList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var book = _repo.AddBooking(booking);
                var room = _Roomrepo.GetRoomById(booking.RoomId);

                // Update the room status to false
                var rooms = new Room { Status = room.Status.Equals(false) };
                _Roomrepo.UpdateRoom(rooms.Id, rooms);
                return RedirectToAction("GetAllBookings");
            }
            ViewData["Message"] = "Data is not valid to create the booking";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var old = await this._repo.GetBookingById(id);
            return View(old);

        }


        [HttpPost]
        public async Task<IActionResult> Update(Booking booking)
        {
            
            await _repo.UpdateBooking(booking.Id,booking);
            return RedirectToAction("GetAllBookings");
        }
    }
}
