using System;
using System.Collections.Generic;
using System.Text;

namespace Fintrack.Domain.Entities.Abstract;

public abstract class Entity
{
    public Guid Id { get; set; }
}
