using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using HotelBookingStartupProject.BusinessLogic;
using HotelBookingStartupProject.Data.Repositories;
using HotelBookingStartupProject.Models;
using NSubstitute;
using NUnit.Framework;

namespace HotelBookingStartupProjectSpecFlowTest
{
    [Binding]
    public class SpecFlowFeatureCreateBookingsSteps
    {
        private DateTime startDate;
        private DateTime endDate;

        private bool result;
        
        [Given(@"I have the entered a (.*)(.*) startDate")]
        public void GivenIHaveTheEnteredAStartDate(string p0, DateTime p1)
        {
            startDate = p1;
        }
        
        [Given(@"I have also entered a (.*)(.*) endDate")]
        public void GivenIHaveAlsoEnteredAEndDate(string p0, DateTime p1)
        {
            endDate = p1;
        }
        
        [Given(@"There are available rooms false")]
        public void GivenThereAreAvailableRoomsFalse()
        {
            //Handled by the factory
        }
        
        [When(@"I press Create")]
        public void WhenIPressCreate()
        {
            //Act
            var b = CreateFakeBookingManager();

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            result = b.CreateBooking(booking);
        }
        
        [Then(@"A new booking should be created")]
        public void ThenANewBookingShouldBeCreated()
        {
            Assert.AreEqual(result, true);
        }

        private IBookingManager CreateFakeBookingManager()
        {
            IRepository<Booking> fakeBookingrepo = Substitute.For<IRepository<Booking>>();
            IRepository<Room> fakeRoomrepo = Substitute.For<IRepository<Room>>();

            fakeRoomrepo.GetAll().Returns(GenerateFakeRooms());
            fakeBookingrepo.GetAll().Returns(GenerateFakeBookings());

            IBookingManager b = new BookingManager(fakeBookingrepo, fakeRoomrepo);

            return b;
        }

        private IEnumerable<Booking> GenerateFakeBookings()
        {
            DateTime date = DateTime.Today;
            List<Booking> bookings = new List<Booking>
            {
                new Booking { StartDate=date.AddDays(7), EndDate=date.AddDays(14), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { StartDate=date.AddDays(7), EndDate=date.AddDays(14), IsActive=true, CustomerId=2, RoomId=2 },
                new Booking { StartDate=date.AddDays(7), EndDate=date.AddDays(14), IsActive=true, CustomerId=1, RoomId=3 },
            };

            return bookings;
        }

        private IEnumerable<Room> GenerateFakeRooms()
        {
            List<Room> rooms = new List<Room>
            {
                new Room { Description="A", Id=1 },
                new Room { Description="B", Id=2 },
                new Room { Description="C", Id=3 }
            };
            return rooms;
        }
    }
}
