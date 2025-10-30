using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class CompanyRulesInfoModel
    {

        public string? OfficeStartTime { get; set; }
        public string? OfficeEndTime { get; set; }
        public string? OfficeBreakHour { get; set; }
        public string? CheckinAcceptable { get; set; }
        public string? CheckinOneHourLate { get; set; }
        public string? CheckoutAcceptable { get; set; }
        public string? CheckoutHourLate { get; set; }
        public string? MorningHalfCheckinAcceptable { get; set; }
        public string? MorningHalfCheckinHourLate { get; set; }
        public string? EveningHalfCheckoutAcceptable { get; set; }
        public string? EveningHalfCheckoutHourLate { get; set; }
        public string? HourLateFlagDeduction { get; set; }
        public string? HalfDayFlagDeduction { get; set; }
        public string? FullDayFlagDeduction { get; set; }

        public CompanyRulesInfoModel()
        {
        }
    }
}
