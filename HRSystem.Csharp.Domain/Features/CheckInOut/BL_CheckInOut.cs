using HRSystem.Csharp.Domain.Models.CheckInOut;
using HRSystem.Csharp.Domain.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.CheckInOut
{
    public class BL_CheckInOut
    {
        private readonly DA_CheckInOut _daCheckInOut;
        public BL_CheckInOut(DA_CheckInOut daCheckInOut)
        {
            _daCheckInOut = daCheckInOut;
        }
        public async Task<Result<CheckInResponseModel>> CheckIn(CheckInRequestModel requestModel)
        {
            if (string.IsNullOrWhiteSpace(requestModel.CheckInLatitude))
            {
                return Result<CheckInResponseModel>.ValidationError("Latitude is required!");
            }
            if (string.IsNullOrWhiteSpace(requestModel.CheckInLongitude))
            {
                return Result<CheckInResponseModel>.ValidationError("Longitude is required!");
            }
            var data = await _daCheckInOut.CheckIn(requestModel);
            return data;
        }

        public async Task<Result<CheckOutResponseModel>> CheckOut(CheckOutRequestModel requestModel)
        {
            if (string.IsNullOrWhiteSpace(requestModel.CheckOutLatitude))
            {
                return Result<CheckOutResponseModel>.ValidationError("Latitude is required!");
            }
            if (string.IsNullOrWhiteSpace(requestModel.CheckOutLongitude))
            {
                return Result<CheckOutResponseModel>.ValidationError("Longitude is required!");
            }
            var data = await _daCheckInOut.CheckOut(requestModel);
            return data;
        }
    }
}
