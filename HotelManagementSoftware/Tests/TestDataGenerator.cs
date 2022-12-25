using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSoftware.Tests
{
    public class TestDataGenerator
    {
        private EmployeeBusiness employeeBusiness;
        private CountryBusiness countryBusiness;
        private CustomerBusiness customerBusiness;
        private RoomBusiness roomBusiness;
        private RoomTypeBusiness roomTypeBusiness;
        private ReservationBusiness reservationBusiness;
        private ReservationCancelFeePercentBusiness reservationCancelFeePercentBusiness;
        private HousekeepingBusiness housekeepingBusiness;
        private MaintenanceBusiness maintenanceBusiness;
        private FloorBusiness floorBusiness;

        public TestDataGenerator()
        {
            employeeBusiness = App.Current.Services.GetRequiredService<EmployeeBusiness>();
            countryBusiness = App.Current.Services.GetRequiredService<CountryBusiness>();
            customerBusiness = App.Current.Services.GetRequiredService<CustomerBusiness>();
            roomBusiness = App.Current.Services.GetRequiredService<RoomBusiness>();
            roomTypeBusiness = App.Current.Services.GetRequiredService<RoomTypeBusiness>();
            reservationBusiness = App.Current.Services.GetRequiredService<ReservationBusiness>();
            reservationCancelFeePercentBusiness = App.Current.Services.GetRequiredService<ReservationCancelFeePercentBusiness>();
            housekeepingBusiness = App.Current.Services.GetRequiredService<HousekeepingBusiness>();
            maintenanceBusiness = App.Current.Services.GetRequiredService<MaintenanceBusiness>();
            floorBusiness = App.Current.Services.GetRequiredService<FloorBusiness>();
        }

        public async Task CreateMaxFloor()
        {
            await floorBusiness.SetMaxFloorNumber(5);
        }

        public async Task GenerateEmployees()
        {
            await employeeBusiness.CreateEmployee(new Employee("Nguyễn Lễ Tân",
                                                            "letan",
                                                            EmployeeType.Receptionist,
                                                            Gender.Male,
                                                            new DateTime(1985, 10, 10),
                                                            "123456789",
                                                            "0344250402",
                                                            "120 Lê Hồng Phong",
                                                            "letan@gmail.com"), "1234letan");
            await employeeBusiness.CreateEmployee(new Employee("Hà Quản Lý",
                                                         "quanly",
                                                         EmployeeType.Manager,
                                                         Gender.Female,
                                                         new DateTime(1968, 5, 10),
                                                         "987654321",
                                                         "0344250403",
                                                         "182/2 Lê Thành Phương",
                                                         "quanly@gmail.com"), "1234quanly");
            await employeeBusiness.CreateEmployee(new Employee("Lê Dọn Phòng",
                                                         "donphong",
                                                         EmployeeType.HousekeepingManager,
                                                         Gender.Male,
                                                         new DateTime(1976, 2, 1),
                                                         "123456789123",
                                                         "0344250404",
                                                         "5 Nguyễn Du",
                                                         "donphong@gmail.com"), "1234donphong");
            await employeeBusiness.CreateEmployee(new Employee("Vũ Sửa Phòng",
                                                         "suaphong",
                                                         EmployeeType.MaintenanceManager,
                                                         Gender.Female,
                                                         new DateTime(1990, 4, 5),
                                                         "321987654321",
                                                         "0344250405",
                                                         "1 Nguyễn Khuyến",
                                                         "suaphong@gmail.com"), "1234suaphong");
        }

        public async Task GenerateCustomers()
        {
            Country vietnam = (await countryBusiness.GetAllCountries())
                .First(i => i.Name == "Vietnam");
            var customerBusiness = new CustomerBusiness();
            await customerBusiness.CreateCustomer(new Customer("Nguyễn Văn A",
                                                         new DateTime(1975, 4, 2),
                                                         IdNumberType.Cmnd,
                                                         "123456789",
                                                         Gender.Male,
                                                         "0344250406",
                                                         "110 Nguyễn Huệ",
                                                         "Quận 1",
                                                         "TP. HCM",
                                                         vietnam,
                                                         PaymentMethod.Cash));

            await customerBusiness.CreateCustomer(new Customer("Nguyễn Văn B",
                                                         new DateTime(2001, 1, 1),
                                                         IdNumberType.Cmnd,
                                                         "123456789101",
                                                         Gender.Male,
                                                         "0344250407",
                                                         "120 Quang Trung",
                                                         "Nha Trang",
                                                         "Khánh Hòa",
                                                         vietnam,
                                                         PaymentMethod.Cash));

            Country usa = (await countryBusiness.GetAllCountries())
                .First(i => i.Name == "United States");
            await customerBusiness.CreateCustomer(new Customer("Mary John",
                                                         new DateTime(2000, 10, 23),
                                                         IdNumberType.Passport,
                                                         "12345",
                                                         Gender.Female,
                                                         "1-844-872-4681",
                                                         "110 Mary Street",
                                                         "Lincoln",
                                                         "Nebraska",
                                                         usa,
                                                         PaymentMethod.Visa,
                                                         "123456",
                                                         DateTime.Now.AddYears(5)));
        }

        public async Task GenerateRoomsAndRoomTypes()
        {
            await roomTypeBusiness.AddRoomType(new RoomType("Normal",
                                                      2,
                                                      500000,
                                                      "Standard single two-person bed"));
            await roomTypeBusiness.AddRoomType(new RoomType("Deluxe",
                                                      4,
                                                      1000000,
                                                      "Two two-person bed"));
            await roomTypeBusiness.AddRoomType(new RoomType("Presidential",
                                                      2,
                                                      3000000,
                                                      "Luxury room with two-person bed"));

            List<RoomType> roomTypes = await roomTypeBusiness.GetRoomTypes();

            RoomType normal = roomTypes.First(i => i.Name == "Normal");
            await roomBusiness.AddRoom(new Room(101, normal, 1));
            await roomBusiness.AddRoom(new Room(102, normal, 1));
            await roomBusiness.AddRoom(new Room(103, normal, 1));
            await roomBusiness.AddRoom(new Room(201, normal, 2));
            await roomBusiness.AddRoom(new Room(202, normal, 2));

            RoomType deluxe = roomTypes.First(i => i.Name == "Deluxe");
            await roomBusiness.AddRoom(new Room(104, deluxe, 1));
            await roomBusiness.AddRoom(new Room(105, deluxe, 1));
            await roomBusiness.AddRoom(new Room(106, deluxe, 1));
            await roomBusiness.AddRoom(new Room(203, deluxe, 2));
            await roomBusiness.AddRoom(new Room(204, deluxe, 2));

            RoomType presidential = roomTypes.First(i => i.Name == "Presidential");
            await roomBusiness.AddRoom(new Room(205, presidential, 2));
            await roomBusiness.AddRoom(new Room(206, presidential, 2));
        }

        public async Task GenerateCancelFeePercents()
        {
            await reservationCancelFeePercentBusiness.Add(new ReservationCancelFeePercent(1, 100));
            await reservationCancelFeePercentBusiness.Add(new ReservationCancelFeePercent(2, 70));
            await reservationCancelFeePercentBusiness.Add(new ReservationCancelFeePercent(3, 50));
            await reservationCancelFeePercentBusiness.Add(new ReservationCancelFeePercent(4, 30));
        }

        public async Task GenerateReservations()
        {
            Employee currentEmployee = (await employeeBusiness.GetEmployeesByName("Nguyễn Lễ Tân"))[0];

            var arrivalTime = DateTime.Now.AddDays(2);
            var departureTime = DateTime.Now.AddDays(5);
            Customer customer = (await customerBusiness.GetCustomersByName("Nguyễn Văn A"))[0];
            Room room = (await roomBusiness.GetUsableRooms(arrivalTime, departureTime, "Normal"))[0];
            await reservationBusiness.CreateReservation(new Reservation(arrivalTime,
                                                                  departureTime,
                                                                  1,
                                                                  room,
                                                                  customer,
                                                                  currentEmployee), false);

            arrivalTime = DateTime.Now.AddHours(12);
            departureTime = DateTime.Now.AddDays(2);
            customer = (await customerBusiness.GetCustomersByName("Nguyễn Văn B"))[0];
            room = (await roomBusiness.GetUsableRooms(arrivalTime, departureTime, "Presidential"))[0];
            await reservationBusiness.CreateReservation(new Reservation(arrivalTime,
                                                                  departureTime,
                                                                  2,
                                                                  room,
                                                                  customer,
                                                                  currentEmployee), false);

            arrivalTime = DateTime.Now;
            departureTime = arrivalTime.Date.AddDays(5);
            customer = (await customerBusiness.GetCustomersByName("Mary John"))[0];
            room = (await roomBusiness.GetUsableRooms(arrivalTime, departureTime, "Deluxe"))[0];
            await reservationBusiness.CreateReservation(new Reservation(arrivalTime,
                                                                  departureTime,
                                                                  4,
                                                                  room,
                                                                  customer,
                                                                  currentEmployee), true);
        }

        public async Task CancelReservation()
        {
            Reservation reservation = (await reservationBusiness.GetReservations(customerName: "Nguyễn Văn A"))[0];
            await reservationBusiness.CancelReservation(reservation);
        }

        public async Task GenerateHousekeepingRequests()
        {
            Employee currentEmployee = (await employeeBusiness.GetEmployeesByName("Nguyễn Lễ Tân"))[0];
            List<Room> rooms = await roomBusiness.GetRooms();

            var housekeepingRequest1 = new HousekeepingRequest(currentEmployee.EmployeeId,
                                                              currentEmployee,
                                                              rooms[0],
                                                              DateTime.Now,
                                                              DateTime.Now.AddDays(1),
                                                              "note 1");
            await housekeepingBusiness.CreateHousekeepingRequest(housekeepingRequest1);

            var housekeepingRequest2 = new HousekeepingRequest(currentEmployee.EmployeeId,
                                                              currentEmployee,
                                                              rooms[1],
                                                              DateTime.Now.AddDays(10),
                                                              DateTime.Now.AddDays(20),
                                                              "note 2");
            await housekeepingBusiness.CreateHousekeepingRequest(housekeepingRequest2);
        }

        public async Task GenerateMaintenanceRequests()
        {
            Employee currentEmployee = (await employeeBusiness.GetEmployeesByName("Nguyễn Lễ Tân"))[0];
            List<Room> rooms = await roomBusiness.GetRooms();

            var maintenanceRequest1 = new MaintenanceRequest(currentEmployee.EmployeeId,
                                                            currentEmployee,
                                                            rooms[0],
                                                            DateTime.Now,
                                                            DateTime.Now.AddDays(1),
                                                            "note 1")
            {
                MaintenanceItems =
                {
                    new MaintenanceItem("Lightbulb", 2),
                    new MaintenanceItem("TV", 1)
                }
            };
            await maintenanceBusiness.CreateMaintenanceRequest(maintenanceRequest1);

            var maintenanceRequest2 = new MaintenanceRequest(currentEmployee.EmployeeId,
                                                            currentEmployee,
                                                            rooms[1],
                                                            DateTime.Now.AddDays(10),
                                                            DateTime.Now.AddDays(20),
                                                            "note 2")
            {
                MaintenanceItems =
                {
                    new MaintenanceItem("AC", 1),
                    new MaintenanceItem("Bed", 2)
                }
            };
            await maintenanceBusiness.CreateMaintenanceRequest(maintenanceRequest2);
        }

        public async Task CloseHousekeepingRequest()
        {
            Employee currentEmployee = (await employeeBusiness.GetEmployeesByName("Lê Dọn Phòng"))[0];
            HousekeepingRequest request = (await housekeepingBusiness.GetHousekeepingRequests(note: "note 2"))[0];
            await housekeepingBusiness.CloseHousekeepingRequest(request, DateTime.Now.AddDays(15), currentEmployee);
        }

        public async Task CloseMaintenanceRequest()
        {
            Employee currentEmployee = (await employeeBusiness.GetEmployeesByName("Vũ Sửa Phòng"))[0];
            MaintenanceRequest request = (await maintenanceBusiness.GetMaintenanceRequests(note: "note 2"))[0];
            await maintenanceBusiness.CloseMaintenanceRequest(request, DateTime.Now.AddDays(15), currentEmployee);
        }
    }
}
