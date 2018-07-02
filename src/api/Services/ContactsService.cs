using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Titan.UFC.Users.WebAPI.Logger;
using Titan.UFC.Users.WebAPI.Models;
using Titan.UFC.Users.WebAPI.Repositories;

namespace Titan.UFC.Users.WebAPI.Services
{
    public class ContactsService : IContactsService
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly ILogger _logger;

        public ContactsService(IPhoneRepository phoneRepository,IEmailRepository emailRepository, ILogger<ContactsService> logger)
        {
            _phoneRepository = phoneRepository ?? throw new ArgumentNullException(nameof(ContactsService));
            _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(ContactsService));
            _logger = logger;
        }


        public async Task<Phone> ReadOnePhoneAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {userId}", userId);

            var phone = await _phoneRepository.ReadOneAsync(userId);
            return phone;
        }

        public async Task RemovePhoneAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.DeleteItem, "Remove phone by {userId}", userId);

            await _phoneRepository.RemoveAsync(userId);
        }

      public async  Task<Email> ReadOneEmailAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Get email by {userId}", userId);

            var email = await _emailRepository.ReadOneAsync(userId);
            return email;
        }

        public async Task RemoveEmailAsync(int userId)
        {
            _logger.LogInformation(LoggerEvents.DeleteItem, "Remove email by  {userId}", userId);

            await _emailRepository.RemoveAsync(userId);
        }

        public async Task<int> CheckValidUserByPhoneAsync(Phone phone)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {phoneNumber}", phone.Number);
            return await this._phoneRepository.CheckValidUserByPhoneAsync(phone);
        }

        public async Task<int> CheckValidUserByEmailAsync(Email email)
        {
            _logger.LogInformation(LoggerEvents.GetItem, "Getting item {emailAddress}", email.Address);
            return await this._emailRepository.CheckValidUserByEmailAsync(email);
        }
    }
}
