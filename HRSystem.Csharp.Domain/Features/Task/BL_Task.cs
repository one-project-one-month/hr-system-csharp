using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Task;

public class BL_Task
{
    private readonly DA_Task _daTask;

    public BL_Task(DA_Task daTask)
    {
        _daTask = daTask;
    }
}
