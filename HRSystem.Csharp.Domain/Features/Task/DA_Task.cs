using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Task;

public class DA_Task
{
    private readonly AppDbContext _db;

    public DA_Task(AppDbContext db)
    {
        _db = db;
    }
}
