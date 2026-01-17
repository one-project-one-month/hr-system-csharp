using HRSystem.Csharp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Task
{
    public class TaskListRequestModel: PaginationRequestModel
    {
        public string?  Name { get; set; }
     }
}
