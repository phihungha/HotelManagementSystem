using HotelManagementSoftware.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSoftware.Business
{
    public class HousekeepingBusiness
    {
        /// <summary>
        /// Get all currently opening housekeeping requests
        /// </summary>
        /// <returns>List of housekeeping requests</returns>
        public async Task<List<HousekeepingRequest>> GetOpeningHousekeepingRequests()
        {
            return await GetHousekeepingRequests(status: HousekeepingRequestStatus.Opened);
        }

        /// <summary>
        /// Get a housekeeping request by ID.
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns>Housekeeping request</returns>
        public async Task<HousekeepingRequest?> GetHousekeepingRequestById(int id)
        {
            using (var db = new Database())
            {
                return await db.HousekeepingRequests
                        .Include(i => i.OpenEmployee)
                        .Include(i => i.CloseEmployee)
                        .Include(i => i.Room)
                        .FirstOrDefaultAsync(i => i.HousekeepingRequestId == id);
            }
        }

        /// <summary>
        /// Get housekeeping requests that satisfy specified criteria.
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="roomNumber">Room number</param>
        /// <param name="note">Note search term</param>
        /// <param name="openEmployeeName">Open employee name</param>
        /// <param name="closeEmployeeName">Close employee name</param>
        /// <param name="fromStartTime">Min start time</param>
        /// <param name="toStartTime">Max start time</param>
        /// <param name="fromEndTime">Min end time</param>
        /// <param name="toEndTime">Max end time</param>
        /// <param name="fromCloseTime">Min close time</param>
        /// <param name="toCloseTime">Max close time</param>
        /// <returns>List of housekeeping requests</returns>
        public async Task<List<HousekeepingRequest>> GetHousekeepingRequests(
            HousekeepingRequestStatus? status = null,
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
                var request = db.HousekeepingRequests
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
        /// Create a housekeeping request
        /// </summary>
        /// <param name="request">Housekeeping request</param>
        public async Task CreateHousekeepingRequest(HousekeepingRequest request)
        {
            ValidateHousekeepingRequest(request);
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
        /// Edit a housekeeping request
        /// </summary>
        /// <param name="request">Updated housekeeping request</param>
        public async Task EditHousekeepingRequest(HousekeepingRequest request)
        {
            ValidateHousekeepingRequest(request);
            using (var db = new Database())
            {
                db.Update(request);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Close a housekeeping request
        /// </summary>
        /// <param name="request">Housekeeping request to close</param>
        /// <param name="closeTime">Close time</param>
        /// <param name="closeEmployee">Close employee</param>
        public async Task CloseHousekeepingRequest(HousekeepingRequest request, DateTime closeTime, Employee closeEmployee)
        {
            ValidateHousekeepingRequest(request);
            using (var db = new Database())
            {
                HousekeepingRequest _request
                    = await db.HousekeepingRequests.FirstAsync(
                                i => i.HousekeepingRequestId  == request.HousekeepingRequestId);
                Employee _closeEmployee = await db.Employees.FirstAsync(i => i.EmployeeId == closeEmployee.EmployeeId);
                Room _room = await db.Rooms.FirstAsync(i => i.RoomId == request.Room.RoomId);

                if (request.Status == HousekeepingRequestStatus.Closed)
                    throw new ArgumentException("Request already closed");

                if (closeEmployee.EmployeeType != EmployeeType.Manager
                    && closeEmployee.EmployeeType != EmployeeType.HousekeepingManager)
                    throw new ArgumentException("Close employee needs to be a manager or housekeeping manager");

                _request.Status = HousekeepingRequestStatus.Closed;
                _request.CloseTime = closeTime;
                _request.Room = _room;
                _request.CloseEmployee = _closeEmployee;
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Validate housekeeping request's info before adding or updating.
        /// </summary>
        /// <param name="request">Housekeeping request to validate</param>
        /// <exception cref="ArgumentException">Validation failure</exception>
        public void ValidateHousekeepingRequest(HousekeepingRequest request)
        {
            if (request.OpenEmployee == null || request.OpenEmployeeId == null)
                throw new ArgumentException("Request needs to have an open employee");
            if (request.Status == HousekeepingRequestStatus.Opened
                && (request.CloseEmployee != null || request.CloseEmployeeId != null))
                throw new ArgumentException("Currently opening request cannot have close time");
            if (request.Status == HousekeepingRequestStatus.Closed 
                && (request.CloseEmployee == null || request.CloseEmployeeId == null))
                throw new ArgumentException("Closed request needs to have a close employee");
            if (request.StartTime >= request.CloseTime)
                throw new ArgumentException("Close time cannot be ahead of start time");
            if (request.Status == HousekeepingRequestStatus.Closed
                && request.CloseTime == null)
                throw new ArgumentException("Close time cannot be empty if the request is closed");
            if (request.Room == null)
                throw new ArgumentException("Room cannot be empty");
            if (request.StartTime >= request.EndTime)
                throw new ArgumentException("End time cannot be ahead of start time");
        }
    }
}
