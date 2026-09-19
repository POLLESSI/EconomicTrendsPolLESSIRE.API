using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using EconomicTrendsPolLESSIRE.Hubs.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public class UserMessageService : IUserMessageService
    {
        private readonly IUserMessageRepository _repo;
        private readonly IProfanityService _profanityService;
        private readonly IMessageCorrelationService _messageCorrelationService;
        private readonly IMessageTriageService _messageTriageService;
        private readonly IUserMessageAdminQueueRepository _adminQueueRepository;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly ILogger<UserMessageService> _logger;

        public UserMessageService(IUserMessageRepository repo, IProfanityService profanityService, IMessageCorrelationService messageCorrelationService, IMessageTriageService messageTriageService, IUserMessageAdminQueueRepository adminQueueRepository, IHubContext<MessageHub> hubContext, ILogger<UserMessageService> logger)
        {
            _repo = repo;
            _profanityService = profanityService;
            _messageCorrelationService = messageCorrelationService;
            _messageTriageService = messageTriageService;
            _adminQueueRepository = adminQueueRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        public Task<int> ArchivePastUserMessagesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserMessageAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserMessage>> GetLatestAsync(int take = 100, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage?> GetUserMessageByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> InsertAsync(UserMessage msg, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> InsertAsync(UserMessage msg, bool requestAdminReview, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}


















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.