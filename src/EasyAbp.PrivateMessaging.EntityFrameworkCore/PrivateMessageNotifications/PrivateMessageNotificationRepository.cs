using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.PrivateMessaging.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PrivateMessaging.PrivateMessageNotifications
{
    public class PrivateMessageNotificationRepository :
        EfCoreRepository<PrivateMessagingDbContext, PrivateMessageNotification, Guid>,
        IPrivateMessageNotificationRepository
    {
        public PrivateMessageNotificationRepository(IDbContextProvider<PrivateMessagingDbContext> dbContextProvider) :
            base(dbContextProvider)
        {
        }

        public async Task<long> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await this.Where(n => n.UserId == userId).CountAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<PrivateMessageNotification>> GetListByUserIdAsync(Guid userId, int skipCount,
            int maxResultCount, CancellationToken cancellationToken = default)
        {
            return await this.Where(n => n.UserId == userId).PageBy(skipCount, maxResultCount)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task DeleteByPrivateMessageIdAsync(Guid privateMessageId, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(n => n.PrivateMessageId == privateMessageId, true, cancellationToken: cancellationToken);
        }
    }
}