using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Domain.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int? GameId { get; set; }

    public int? AccountId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Cartdetail> Cartdetails { get; set; } = new List<Cartdetail>();
}
