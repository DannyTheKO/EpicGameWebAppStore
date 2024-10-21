using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Discount
{
    public int DiscountId { get; set; }

    public int? GameId { get; set; }

    public decimal? Percent { get; set; }

    public string? Code { get; set; }

    public DateTime? StartOn { get; set; }

    public DateTime? EndOn { get; set; }

    public virtual Game? Game { get; set; }
}
