using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<IPagedResults<EvacueeListItem>> GetEvacueesAsync(EvacueeSearchQueryParameters searchQuery)
        {
            var query = db.Evacuees
                .Include(e => e.EvacueeRegistration)
                    .ThenInclude(r => r.IncidentTask)
                        .ThenInclude(i => i.Community)
                .Include(e => e.EvacueeRegistration)
                    .ThenInclude(r => r.IncidentTask)
                        .ThenInclude(i => i.Region)
               .Include(e => e.EvacueeRegistration)
                    .ThenInclude(r => r.HostCommunity)
                .Include(e => e.Referrals)
                .Where(e => e.EvacueeRegistration.Active == searchQuery.Active);

            if (searchQuery.HasQuery())
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.LastName, $"%{searchQuery.Query}%") ||
                    e.EvacueeRegistration.IncidentTask.TaskNumber == searchQuery.Query ||
                    e.EvacueeRegistration.EssFileNumber.ToString() == searchQuery.Query ||
                    EF.Functions.Like(e.EvacueeRegistration.HostCommunity.Name, $"%{searchQuery.Query}%") ||
                    EF.Functions.Like(e.EvacueeRegistration.IncidentTask.Community.Name, $"%{searchQuery.Query}%")
);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(searchQuery.LastName)) query = query.Where(e => EF.Functions.Like(e.LastName, $"%{searchQuery.LastName}%"));
                if (!string.IsNullOrWhiteSpace(searchQuery.FirstName)) query = query.Where(e => EF.Functions.Like(e.FirstName, $"%{searchQuery.FirstName}%"));
                if (!string.IsNullOrWhiteSpace(searchQuery.IncidentTaskNumber)) query = query.Where(e => e.EvacueeRegistration.IncidentTask.TaskNumber == searchQuery.IncidentTaskNumber);
                if (!string.IsNullOrWhiteSpace(searchQuery.EssFileNumber)) query = query.Where(e => e.EvacueeRegistration.EssFileNumber.ToString() == searchQuery.EssFileNumber);
                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedFrom)) query = query.Where(e => e.EvacueeRegistration.IncidentTask.Community.Id.ToString() == searchQuery.EvacuatedFrom);
                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedTo)) query = query.Where(e => e.EvacueeRegistration.HostCommunity.Id.ToString() == searchQuery.EvacuatedTo);
                if (searchQuery.HasReferrals.HasValue && searchQuery.HasReferrals.Value) query = query.Where(e => e.Referrals.Any(r => r.Referral.Active));
                if (searchQuery.HasReferrals.HasValue && !searchQuery.HasReferrals.Value) query = query.Where(e => !e.Referrals.Any(r => r.Referral.Active));
                if (searchQuery.RegistrationCompleted.HasValue) query = query.Where(e => e.EvacueeRegistration.IsFinalized == searchQuery.RegistrationCompleted.Value);
            }

            if (!searchQuery.HasSortBy()) searchQuery.SortBy = "-essFileNumber";

            var pagedQuery = new PagedQuery<Models.Db.Evacuee>(query, searchQuery.Offset, searchQuery.Limit);
            query = pagedQuery.Query;

            var results = await query.Sort(MapSortToFields(searchQuery.SortBy)).ToArrayAsync();

            return new PagedResult<EvacueeListItem>(results.Select(mapper.Map<EvacueeListItem>), pagedQuery.Pagination);
        }

        private string MapSortToFields(string sort)
        {
            return sort
                    .Replace("evacuatedFrom", "EvacueeRegistration.IncidentTask.Community.Name", StringComparison.InvariantCultureIgnoreCase)
                    .Replace("evacuatedTo", "EvacueeRegistration.HostCommunity.Name", StringComparison.InvariantCultureIgnoreCase)
                    .Replace("essFileNumber", "EvacueeRegistration.EssFileNumber", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
