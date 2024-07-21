using App.Domain.Models;
using App.Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Reminders.Create
{
    public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, Result<int>>
    {
        private readonly RangoMediaContext _context;

        public CreateReminderCommandHandler(RangoMediaContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = new Reminder
            {
                Title = request.Title,
                Date = request.ReminderDateTime              
            };

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Ok(reminder.Id, "Reminder created successfully.");
        }
    }
}
