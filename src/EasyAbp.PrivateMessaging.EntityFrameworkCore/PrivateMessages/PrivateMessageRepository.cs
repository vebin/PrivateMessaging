using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.PrivateMessaging.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.PrivateMessaging.PrivateMessages
{
    public class PrivateMessageRepository : EfCoreRepository<PrivateMessagingDbContext, PrivateMessage, Guid>,
        IPrivateMessageRepository
    {
        public PrivateMessageRepository(IDbContextProvider<PrivateMessagingDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public async Task<IReadOnlyList<PrivateMessage>> GetListAsync(IEnumerable<Guid> ids,
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var query = includeDetails ? WithDetails() : this;

            return await query.Where(p => ids.Contains(p.Id)).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<long> CountSendingAsync(Guid userId, bool unreadOnly = false,
            CancellationToken cancellationToken = default)
        {
            return await this.Where(m => m.CreatorId == userId && (!unreadOnly || !m.ReadTime.HasValue))
                .CountAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<PrivateMessage>> GetListSendingAsync(Guid userId, int skipCount,
            int maxResultCount, bool unreadOnly = false, CancellationToken cancellationToken = default)
        {
            return await this.Where(m => m.CreatorId == userId && (!unreadOnly || !m.ReadTime.HasValue))
                .PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<long> CountReceivingAsync(Guid userId, bool unreadOnly = false,
            CancellationToken cancellationToken = default)
        {
            return await this.Where(m => m.ToUserId == userId && (!unreadOnly || !m.ReadTime.HasValue))
                .CountAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<PrivateMessage>> GetListReceivingAsync(Guid userId, int skipCount,
            int maxResultCount, bool unreadOnly = false, CancellationToken cancellationToken = default)
        {
            return await this.Where(m => m.ToUserId == userId && (!unreadOnly || !m.ReadTime.HasValue))
                .PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}