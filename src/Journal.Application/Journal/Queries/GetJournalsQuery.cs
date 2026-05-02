using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journal.Application.Utils;
using Journal.Domain.Entities;
using MediatR;

using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.Journal.Queries
{
    public class GetJournalsQuery : IRequest<PaginatedList<JournalEntity>>
    {
        public string Search { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
