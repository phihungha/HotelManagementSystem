using HotelManagementSoftware.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSoftware.Business
{
    public class MaintenanceBusiness
    {
        /// <summary>
        /// Get all currently opening maintenance requests.
        /// Maintenace items are not included since this method is only used
        /// in list screens.
        /// </summary>
        /// <returns>List of maintenance requests</returns>
        public async Task<List<MaintenanceRequest>> GetOpeningMaintenanceRequests()
        {
            return await GetMaintenanceRequests(status: MaintenanceRequestStatus.Opened);
        }

        /// <summary>
        /// Get a maintenance request by ID. Each maintenance request
        /// contains maintenance items.
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns>Maintenance request</returns>
        public async Task<MaintenanceRequest?> GetMaintenanceRequestById(int id)
        {
            using (var db = new Database())
            {
                return await db.MaintenanceRequests
                        .Include(i => i.OpenEmployee)
                        .Include(i => i.CloseEmployee)
                        .Include(i => i.Room)
                        .Include(i => i.MaintenanceItems)
                        .FirstOrDefaultAsync(i => i.MaintenanceRequestId == id);
            }
        }

        /// <summary>
        /// Get maintenance requests that satisfy specified criteria.
        /// Maintenace items are not included since this method is only used
        /// in list screens.
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="roomNumber">Room number</param>
        /// <param name="note">Note search term</param>
        /// <param name="openEmployeeName">Open employee name</param>
        /// <param name="closeEmployeeName">Close employee name</param>
        /// <param name="fromStartTime">From start time</param>
        /// <param name="toStartTime">To start time</param>
        /// <param name="fromEndTime">From end time</param>
        /// <param name="toEndTime">To end time</param>
        /// <param name="fromCloseTime">From close time</param>
        /// <param name="toCloseTime">To close time</param>
        /// <returns>List of maintenance requests</returns>
        public async Task<List<MaintenanceRequest>> GetMaintenanceRequests(
            MaintenanceRequestStatus? status = null,
            int? roomNumber = null,
            string? note = null,
            string? openEmployeeName = null,
            string? closeEmployeeName = null,
            DateTime? fromStartTime = null,
            DateTime? toStartTime = null,
            DateTime? fromEndTime = null,
            DateTime? toEndTime = null,
            DateTime? fromCloseTime = null,
            DateTime? toCloseTime = null)
        {
            using (var db = new Database())
            {
                var request = db.MaintenanceRequests
                    .Include(i => i.OpenEmployee)
                    .Include(i => i.CloseEmployee)
                    .Include(i => i.Room);

                var filteredRequest = request.Where(i => true);

                if (roomNumber != null)
                    filteredRequest = filteredRequest
                        .Where(i => i.Room != null && i.Room.RoomNumber == roomNumber);

                if (fromStartTime != null)
                    filteredRequest = filteredRequest.Where(i => i.StartTime >= fromStartTime);

                if (toStartTime != null)
                    filteredRequest = filteredRequest.Where(i => i.StartTime <= toStartTime);

                if (fromEndTime != null)
                    filteredRequest = filteredRequest.Where(i => i.EndTime >= fromEndTime);

                if (toEndTime != null)
                    filteredRequest = filteredRequest.Where(i => i.EndTime <= toEndTime);

                if (fromCloseTime != null)
                    filteredRequest = filteredRequest.Where(i => i.CloseTime >= fromCloseTime);

                if (toCloseTime != null)
                    filteredRequest = filteredRequest.Where(i => i.CloseTime <= toCloseTime);

                if (openEmployeeName != null)
                    filteredRequest = filteredRequest
                        .Where(i => i.OpenEmployee != null && 
                                    i.OpenEmployee.Name == openEmployeeName);

                if (closeEmployeeName != null)
                    filteredRequest = filteredRequest
                        .Where(i => i.CloseEmployee != null && 
                                    i.CloseEmployee.Name == closeEmployeeName);

                if (status != null)
                    filteredRequest = filteredRequest.Where(i => i.Status == status);

                if (note != null)
                    filteredRequest = filteredRequest
                        .Where(i => i.Note != null && i.Note.Contains(note));

                return await filteredRequest.ToListAsync();
            }
        }

        /// <summary>
        /// Create a maintenance request
        /// </summary>
        /// <param name="request">Maintenance request</param>
        public async Task CreateMaintenanceRequest(MaintenanceRequest request)
        {
            ValidateMaintenanceRequest(request);
            using (var db = new Database())
            {
                Room _room = await db.Rooms.FirstAsync(i => i.RoomId == request.Room.RoomId);
                Employee _openEmployee = await db.Employees.FirstAsync(i => i.EmployeeId == request.OpenEmployeeId);

                if (request.Room == null)
                    throw new ArgumentException("Room cannot be empty");

                if (request.OpenEmployee == null)
                    throw new ArgumentException("Open employee cannot be empty");

                request.Room = _room;
                request.OpenEmployee = _openEmployee;

                db.Add(request);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Edit a maintenance request
        /// </summary>
        /// <param name="request">Updated maintenance request</param>
        public async Task EditMaintenanceRequest(MaintenanceRequest request)
        {
            ValidateMaintenanceRequest(request);
            using (var db = new Database())
            {
                db.Update(request);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Close a maintenance request
        /// </summary>
        /// <param name="request">Maintenance request to close</param>
        /// <param name="closeTime">Close time</param>
        /// <param name="closeEmployee">Close employee</param>
        public async Task CloseMaintenanceRequest(MaintenanceRequest request, DateTime closeTime, Employee closeEmployee)
        {
            ValidateMaintenanceRequest(request);
            using (var db = new Database())
            {
                MaintenanceRequest _request 
                    = await db.MaintenanceRequests.FirstAsync(
                                i => i.MaintenanceRequestId == request.MaintenanceRequestId);
                Employee _closeEmployee = await db.Employees.FirstAsync(i => i.EmployeeId == closeEmployee.EmployeeId);
                Room _room = await db.Rooms.FirstAsync(i => i.RoomId == request.Room.RoomId);

                if (request.Status == MaintenanceRequestStatus.Closed)
                    throw new ArgumentException("Request already closed");

                if (closeEmployee.EmployeeType != EmployeeType.Manager 
                    && closeEmployee.EmployeeType != EmployeeType.MaintenanceManager)
                    throw new ArgumentException("Close employee needs to be a manager or maintenance manager");

                _request.Status = MaintenanceRequestStatus.Closed;
                _request.CloseTime = closeTime;
                _request.Room = _room;
                _request.CloseEmployee = _closeEmployee;
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete maintenance items
        /// </summary>
        /// <param name="items">Maintenance items</param>
        public async Task DeleteMaintenanceItems(List<MaintenanceItem> items)
        {
            using (var db = new Database())
            {
                db.RemoveRange(items);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Validate maintenance request's info before adding or updating.
        /// </summary>
        /// <param name="request">Maintenance request to validate</param>
        /// <exception cref="ArgumentException">Validation failure</exception>
        public void ValidateMaintenanceRequest(MaintenanceRequest request)
        {
            if (request.OpenEmployee == null || request.OpenEmployeeId == null)
                throw new ArgumentException("Request needs to have an open employee");
            if (request.Status == MaintenanceRequestStatus.Opened
                && (request.CloseEmployee != null || request.CloseEmployeeId != null))
                throw new ArgumentException("Currently opening request cannot have close time");
            if (request.Status == MaintenanceRequestStatus.Closed
                && (request.CloseEmployee == null || request.CloseEmployeeId == null))
                throw new ArgumentException("Closed request needs to have a close employee");
            if (request.StartTime >= request.CloseTime)
                throw new ArgumentException("Close time cannot be ahead of start time");
            if (request.Status == MaintenanceRequestStatus.Closed
                && request.CloseTime == null)
                throw new ArgumentException("Close time cannot be empty if the request is closed");
            if (request.Room == null)
                throw new ArgumentException("Room cannot be empty");
            if (request.StartTime >= request.EndTime)
                throw new ArgumentException("End time cannot be ahead of start time");
            request.MaintenanceItems.ForEach(i => ValidateMaintenanceItem(i));
        }

        /// <summary>
        /// Validate maintenance item's info.
        /// </summary>
        /// <param name="item">Maintenance item to validate</param>
        /// <exception cref="ArgumentException">Validation failure</exception>
        public void ValidateMaintenanceItem(MaintenanceItem item)
        {
            if (item.Name == "")
                throw new ArgumentException("Name cannot be empty");
            if (item.Quantity <= 0)
                throw new ArgumentException("Quantity cannot be less than 1");
        }
    }
}
