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
    public class BookingManagerTests
    {
        

        [Test]
        public void FindAvailableRoom_NoDate_ReturnNeg1()
        {
            //Arrange
            var b = CreateFakeBookingManager();

            var startDate = new DateTime();
            var endDate = new DateTime();

            var expected = -1;

            //Act
            int actual = b.FindAvailableRoom(startDate, endDate);

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void FindAvailableRoom_ValidDate_Return1()
        {
            //Arrange
            var b = CreateFakeBookingManager();

            var startDate = new DateTime(2017, 09, 14);
            var endDate = new DateTime(2017, 09, 17);

            var expected = 1;

            //Act
            int actual = b.FindAvailableRoom(startDate, endDate);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Ignore(reason:"not implemented")]
        [Test]
        public void FindAvailableRoom_NoAvailable_ReturnNeg1()
        {
            //Arrange
            var b = CreateFakeBookingManager();
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(7);
            int expected = -1;

            //Act
            int result = b.FindAvailableRoom(startDate, endDate);


            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetFullyOccupiedDates_NoFullyOccupiedDates_ReturnEmptyList()
        {
            //Arrange
            var b = CreateFakeBookingManager();
            DateTime startDate = DateTime.Today.AddMonths(1);
            DateTime endDate = DateTime.Today.AddMonths(1).AddDays(7);

            //Act
            var listOfDates = b.GetFullyOccupiedDates(startDate, endDate);

            //Assert
            Assert.IsEmpty(listOfDates);
        }

        [Test]
        public void GetFullyOccupiedDates_IsFullyOccupied_ReturnPopulatedList()
        {
            //Arrange
            var b = CreateFakeBookingManager();
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(14);

            //Act
            var listOfDates = b.GetFullyOccupiedDates(startDate, endDate);

            //Assert
            Assert.IsNotEmpty(listOfDates);
        }

        [Test]
        public void CreateBooking_ListOfDates_ReturnsListOfBools([ValueSource("_testDates")] TestData testData)
        {
            //arrange
            var b = CreateFakeBookingManager();
            bool expexted = testData.ExpectedResult;

            DateTime startDate = testData.StartDate;
            DateTime endDate = testData.EndDate;

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

        public static DateTime _today()
        {
            var today = new DateTime();
            today = DateTime.Today;
            return today;
        }

        private static TestData[] _testDates = new[]{
            new TestData(){StartDate = _today().AddDays(1), EndDate = _today().AddDays(7), ExpectedResult = true},
            new TestData(){StartDate = _today().AddDays(1), EndDate = _today().AddDays(8), ExpectedResult = false},
            new TestData(){StartDate = _today().AddDays(1), EndDate = _today().AddDays(13), ExpectedResult = false},
            new TestData(){StartDate = _today().AddDays(1), EndDate = _today().AddDays(14), ExpectedResult = false},
            new TestData(){StartDate = _today().AddDays(1), EndDate = _today().AddDays(200), ExpectedResult = false},
            new TestData(){StartDate = _today().AddDays(14), EndDate = _today().AddDays(200), ExpectedResult = true},
            new TestData(){StartDate = _today().AddDays(15), EndDate = _today().AddDays(200), ExpectedResult = true},
            new TestData(){StartDate = _today().AddDays(13), EndDate = _today().AddDays(200), ExpectedResult = false},
            new TestData(){StartDate = _today().AddDays(7), EndDate = _today().AddDays(8), ExpectedResult = false},
            new TestData(){StartDate = _today().AddDays(7), EndDate = _today().AddDays(14), ExpectedResult = false},
            new TestData(){StartDate = _today().AddDays(7), EndDate = _today().AddDays(200), ExpectedResult = false},
            new TestData(){StartDate = _today(), EndDate = _today().AddDays(7), ExpectedResult = false},
            new TestData(){StartDate = _today(), EndDate = _today().AddDays(8), ExpectedResult = false},
            new TestData(){StartDate = _today(), EndDate = _today().AddDays(14), ExpectedResult = false},
            new TestData(){StartDate = _today(), EndDate = _today().AddDays(15), ExpectedResult = false}
        };
        }

        public class TestData
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool ExpectedResult { get; set; }


        }
    }