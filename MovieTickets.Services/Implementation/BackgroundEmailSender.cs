using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository.Interface;
using MovieTickets.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Services.Implementation
{
   
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emaiService;
        private readonly IRepository<EmailMessage> _mailRepository;

        public BackgroundEmailSender(IEmailService emaiService, IRepository<EmailMessage> mailRepository)
        {
            _emaiService = emaiService;
            _mailRepository = mailRepository;
        }

        public async Task DoWork()
        {
            await _emaiService.SendEmailAsync(_mailRepository.GetAll().Where(z => !z.Status).ToList());
        }
    }
}
