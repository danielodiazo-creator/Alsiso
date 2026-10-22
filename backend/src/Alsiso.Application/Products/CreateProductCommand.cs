using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed record class CreateProductCommand(
        string Name,
        string Description,
        string Category,
        decimal? Price);
        
        
    
        
    
}
