using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Domain.Entities;

public partial class Cartdetail
{
    public int CartDetailId { get; set; }

    public int? CartId { get; set; }

    public int? GameId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public virtual Cart? Cart { get; set; }
}
