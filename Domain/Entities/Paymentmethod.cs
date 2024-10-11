using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Domain.Entities;

public partial class Paymentmethod
{
    public int PaymentMethodId { get; set; }

    public string? Name { get; set; }
}
