using App.Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Reminders.GetAll
{
    public class GetRemindersQuery : IRequest<Result<List<ReminderDto>>>
    {
    }
}
