using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed record class UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        string Category,
        Decimal? Price);
    



    
}
