using System;
using System.Collections.Generic;
using HotelBookingStartupProject.Models;
using HotelBookingStartupProject.Data.Repositories;
using System.Linq;

namespace HotelBookingStartupProject.BusinessLogic
{
    public class BookingManager : IBookingManager
    {
        private IRepository<Booking> bookingRepository;
        private IRepository<Room> roomRepository;

        public BookingManager(IRepository<Booking> bookingRepo, IRepository<Room> roomRepo)
        {
            bookingRepository = bookingRepo;
            roomRepository = roomRepo;
        }

        public bool CreateBooking(Booking booking)
        {
            int roomId = -1;
            var startDate = booking.StartDate;
            var endDate = booking.EndDate;

            var today = new DateTime();
            today = DateTime.Today;
            if(startDate == today )
            {
                return false;
            }


            var activeBookings = bookingRepository.GetAll().Where(b => b.IsActive);
            foreach (var room in roomRepository.GetAll())
            {
                var activeBookingsForCurrentRoom = activeBookings.Where(b => b.RoomId == room.Id);
                if (activeBookingsForCurrentRoom.All(b => startDate < b.StartDate &&
                    endDate <= b.StartDate || startDate >= b.EndDate && endDate > b.EndDate))
                {
                    roomId = room.Id;
                    break;
                }
            }

            if (roomId >= 0)
            {
                booking.RoomId = roomId;
                booking.IsActive = true;
                bookingRepository.Add(booking);
                return true;
            }
            return false;

        }

        public int FindAvailableRoom(DateTime startDate, DateTime endDate)
        {
			if (startDate.Ticks == 0 || endDate.Ticks == 0)
                return -1;
            if(startDate.CompareTo(endDate) > 0)
                return -1;
            if (startDate.Ticks == int.MaxValue || endDate.Ticks == int.MaxValue)
                return -1;

            return 1;

        }

        public List<DateTime> GetFullyOccupiedDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> occupiedDates = new List<DateTime>();

            var allBookings = bookingRepository.GetAll();
            var allRooms = roomRepository.GetAll();

            var numRooms = allRooms.Count();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var bookingsAtDate = new List<Booking>();
                foreach(Booking b in allBookings)
                {
					if (IsDateBookedInBooking(b, date))
					{
                        bookingsAtDate.Add(b);
					}
                }
                if(bookingsAtDate.Count() >= numRooms)
                {
                    occupiedDates.Add(date);
                }

            }
            occupiedDates.Sort((x, y) => x.Date.CompareTo(y.Date));

            return occupiedDates;
        }

        private Boolean IsDateBookedInBooking(Booking booking, DateTime date)
        {
            return date.Ticks > booking.StartDate.Ticks && date.Ticks < booking.EndDate.Ticks;
        }
    }
}