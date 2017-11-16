using System;
using TechTalk.SpecFlow;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using HotelBookingStartupProject.Models;
using HotelBookingStartupProject.BusinessLogic;
using HotelBookingStartupProject.Data.Repositories;

namespace HotelBookingStartupProjectSpecFlowTest
{
    [Binding]
    public class SpecFlowFeatureCreateBookingSteps
    {
        private Booking booking;
        private IBookingManager bookingmanager;
        private DateTime startDate;
        private DateTime endDate;
        private bool result;

        [Given(@"I have entered (.*) on the create booking page")]
        public void GivenIHaveEnteredBookingInformationOnTheCreateBookingPage(DateTime sDate)
        {
            //Arrange
            startDate = sDate;
            
            
            
        }
        [Given(@"And i have entered (.*) booking information on the create booking page")]
        public void AndihaveenteredEndDatebookinginformationonthecreatebookingpage(DateTime eDate)
        {
            booking = new Booking { StartDate = startDate, EndDate= eDate};
            bookingmanager = CreateFakeBookingManager();

        }


       [Given(@"There are available rooms")]
        public void GivenThereAreAvailableRooms()
        {
            // Factory handles this
        }
        
        [When(@"I press Create")]
        public void WhenIPressCreate()
        {
            //Act
            result = bookingmanager.CreateBooking(booking);

        }
        
        [Then(@"Anew booking (.*) should be created")]
        public void ThenAnewBookingShouldBeCreated()
        {
            //Assert 
            Assert.AreEqual(true, result);
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
