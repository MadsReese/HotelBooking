using System;
using System.Collections.Generic;

using NUnit.Framework;
using NSubstitute;

using HotelBookingStartupProject;
using HotelBookingStartupProject.Models;
using HotelBookingStartupProject.BusinessLogic;
using HotelBookingStartupProject.Data.Repositories;


namespace HotelBookingStartupProjectTests
{
    [TestFixture]
    public class BookingManager_CreateBookingTests
    {
        [Test]
        public void CreateBooking_AddValidBooking_dd1_od_ReturnsTrue()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = true;

            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(7);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_dd1_od1_ReturnsFalse()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = false;

            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(8);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_dd1_oedNeg1_ReturnsFalse()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = false;

            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(13);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_dd1_oed_ReturnsFalse()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = false;

            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(14);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_dd1_n_ReturnsFalse()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = false;

            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(200);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_oed_n_ReturnsTrue()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = true;

            DateTime startDate = DateTime.Today.AddDays(14);
            DateTime endDate = DateTime.Today.AddDays(200);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_oed1_n_ReturnsTrue()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = true;

            DateTime startDate = DateTime.Today.AddDays(15);
            DateTime endDate = DateTime.Today.AddDays(200);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_oedNeg1_n_ReturnsFalse()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = false;

            DateTime startDate = DateTime.Today.AddDays(13);
            DateTime endDate = DateTime.Today.AddDays(200);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
        }

        [Test]
        public void CreateBooking_AddValidBooking_od_x_ReturnsFalse()
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = false;

            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(14);

            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            //Act
            bool actual = b.CreateBooking(booking);

            //Assert
            Assert.AreEqual(expexted, actual);
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