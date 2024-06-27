using System;
using System.Collections.Generic;
using SoftUni.Models;

namespace SoftUni.Models;

public class Town
{
    public Town()
    {
        this.Addresses = new HashSet<Address>();
    }

    public int TownId { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; }
}