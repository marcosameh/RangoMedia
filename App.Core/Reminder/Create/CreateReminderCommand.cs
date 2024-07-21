using App.Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Reminders.Create
{
    public class CreateReminderCommand : IRequest<Result<int>>
    {
        public string Title { get; set; }
        public DateTime ReminderDateTime { get; set; }
    }
}
