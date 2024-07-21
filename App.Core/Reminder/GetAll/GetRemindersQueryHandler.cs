using App.Domain.Models;
using App.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Reminders.GetAll
{
    public class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, Result<List<ReminderDto>>>
    {
        private readonly RangoMediaContext _context;

        public GetRemindersQueryHandler(RangoMediaContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ReminderDto>>> Handle(GetRemindersQuery request, CancellationToken cancellationToken)
        {
            var reminders = await _context.Reminders
                .Select(r => new ReminderDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    ReminderDateTime = r.Date,
                    IsEmailSent = r.IsEmailSent
                })
                .ToListAsync(cancellationToken);

            return Result<List<ReminderDto>>.Ok(reminders);
        }
    }
}
