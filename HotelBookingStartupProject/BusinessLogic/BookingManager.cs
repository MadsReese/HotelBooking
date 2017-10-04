﻿using System;
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
            return true;
        }

        public int FindAvailableRoom(DateTime startDate, DateTime endDate)
        {
			if (startDate.Ticks == 0 || endDate.Ticks == 0)
                return -1;
            if(startDate.CompareTo(endDate) > 0)
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

            return occupiedDates;
        }

        private Boolean IsDateBookedInBooking(Booking booking, DateTime date)
        {
            return date.Ticks > booking.StartDate.Ticks && date.Ticks < booking.EndDate.Ticks;
        }
    }
}