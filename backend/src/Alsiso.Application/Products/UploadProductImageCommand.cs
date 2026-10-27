using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed record UploadProductImageCommand(
        Guid ProductId,
        Stream ImageStream,
        string ContentType);
    
    
}
