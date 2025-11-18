using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Domain.Models.CheckInOut;
using HRSystem.Csharp.Domain.Models.Employee;
using HRSystem.Csharp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.CheckInOut
{
    public class DA_CheckInOut : AuthorizationService
    {
        private readonly AppDbContext _appDbContext;
        private readonly DA_Sequence _daSequence;

        public DA_CheckInOut(AppDbContext appDbContext, DA_Sequence daSequence, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _appDbContext = appDbContext;
            _daSequence = daSequence;
        }


        public async Task<Result<CheckInResponseModel>> CheckIn(CheckInRequestModel requestModel)
        {
            try
            {
                if (UserCode is null)
                {
                    return Result<CheckInResponseModel>.NotFoundError("Login First.");
                }
                var checkInCompleted = await _appDbContext.TblAttendances
                    .FirstOrDefaultAsync(x=>x.AttendanceDate== DateTime.UtcNow.Date 
                   && x.EmployeeCode == UserCode
                &&  x.DeleteFlag == x.DeleteFlag);
                if (checkInCompleted != null) {
                    return Result<CheckInResponseModel>.ValidationError("You have already checked in");
                }
                var companyLocation = await _appDbContext.TblLocations.Where(x => x.DeleteFlag == false).ToListAsync();
                foreach (var loc in companyLocation)
                {
                    var userLatitude = Convert.ToDouble(requestModel.CheckInLatitude);
                    var userLongitude = Convert.ToDouble(requestModel.CheckInLongitude);
                    var companyLat = Convert.ToDouble(loc.Latitude);
                    var companyLng = Convert.ToDouble(loc.Longitude);
                    var companyRadius = Convert.ToDouble(loc.Radius);
                    double distance = GetDistanceInMeters(
                        userLatitude,
                        userLongitude,
                        companyLat,
                        companyLng
                    );

                    if (distance <= companyRadius)
                    {
                        var attendanceCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.ATT.ToString());
                        var newAttendance = new TblAttendance()
                        {
                            AttendanceId = Guid.NewGuid().ToString(),
                            AttendanceCode = attendanceCode,
                            AttendanceDate = DateTime.UtcNow.Date,
                            EmployeeCode = UserCode,
                            CheckInTime = DateTime.UtcNow,
                            CheckInLocation = requestModel.CheckInLatitude + ", " + requestModel.CheckInLongitude,
                            CreatedBy = UserCode,
                            CreatedAt = DateTime.UtcNow,
                            DeleteFlag = false
                        };
                        await _appDbContext.TblAttendances.AddAsync(newAttendance);
                        await _appDbContext.SaveChangesAsync();

                        return Result<CheckInResponseModel>.Success("Check-in successful.");
                    }
                }

                return Result<CheckInResponseModel>.ValidationError("Outside of allowed zone."); // not inside any allowed zone
            }
            catch (Exception ex)
            {
                return Result<CheckInResponseModel>.Error($"An error occurred during check-in: {ex.Message}");
            }
        }


        public async Task<Result<CheckOutResponseModel>> CheckOut(CheckOutRequestModel requestModel)
        {
            try
            {
                if (UserCode is null)
                {
                    return Result<CheckOutResponseModel>.NotFoundError("Login First.");
                }

                var companyLocation = await _appDbContext.TblLocations.Where(x => x.DeleteFlag == false).ToListAsync();
                var attendanceRecord = await _appDbContext.TblAttendances
                    .Where(a => a.EmployeeCode == UserCode && a.CheckInTime == DateTime.UtcNow.Date && a.DeleteFlag == false)
                    .FirstOrDefaultAsync();
                if (attendanceRecord == null)
                {
                    return Result<CheckOutResponseModel>.ValidationError("No check-in record found for today.");
                }
                var checkOutValidation = await _appDbContext.TblAttendances
                    .FirstOrDefaultAsync(a=> a.EmployeeCode == UserCode
                    && a.CheckOutTime == DateTime.UtcNow.Date && a.DeleteFlag == false);
                if (checkOutValidation != null) {
                    return Result<CheckOutResponseModel>.ValidationError("You have already checked out");
                }
                foreach (var loc in companyLocation)
                {
                    var userLatitude = Convert.ToDouble(requestModel.CheckOutLatitude);
                    var userLongitude = Convert.ToDouble(requestModel.CheckOutLongitude);
                    var companyLat = Convert.ToDouble(loc.Latitude);
                    var companyLng = Convert.ToDouble(loc.Longitude);
                    var companyRadius = Convert.ToDouble(loc.Radius);
                    double distance = GetDistanceInMeters(
                        userLatitude,
                        userLongitude,
                        companyLat,
                        companyLng
                    );

                    if (distance <= companyRadius)
                    {
                       attendanceRecord.CheckOutTime = DateTime.UtcNow;
                       attendanceRecord.CheckOutLocation = requestModel.CheckOutLatitude + ", " + requestModel.CheckOutLongitude;
                          attendanceRecord.ModifiedBy = UserCode;
                          attendanceRecord.ModifiedAt = DateTime.UtcNow;

                        var checkout = await _appDbContext.SaveChangesAsync() > 0;

                        return checkout
                            ?Result<CheckOutResponseModel>.Success("Checkout successfully")
                        :Result<CheckOutResponseModel>.Error("Checkout failed");
                    }
                }

                return Result<CheckOutResponseModel>.ValidationError("Outside of allowed zone."); // not inside any allowed zone
            }
            catch (Exception ex)
            {
                return Result<CheckOutResponseModel>.Error($"An error occurred during check-in: {ex.Message}");
            }
        }


        //helper function to calculate distance between two coordinates
        public static double GetDistanceInMeters(double userLat, double userLon, double locationlat, double locationlon)
        {
            const double R = 6371000; // Earth radius in meters
            double dLat = ToRadians(locationlat - userLat);
            double dLon = ToRadians(locationlon - userLon);

            userLat = ToRadians(userLat);
            locationlat = ToRadians(locationlat);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(userLat) * Math.Cos(locationlat) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private static double ToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }

    }
}
