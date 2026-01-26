using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;

namespace Fintrack.Infrastructure.Repositories;

internal sealed class MccRepository : Repository<Mcc>, IMccRepository
{
    public MccRepository(FintrackContext context) : base(context)
    {
    }
}