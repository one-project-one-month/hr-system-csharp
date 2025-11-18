using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.CheckInOut
{
    public class CheckInOutRequestModel
    {
    }
    public class CheckInRequestModel
    {
        public string CheckInLatitude { get; set; }
        public string CheckInLongitude { get; set; }
    }
    public class CheckOutRequestModel
    {
        public string CheckOutLatitude { get; set; }
        public string CheckOutLongitude { get; set; }
    }
}
