using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public interface IProductImageStorage
    {
        Task<string> SaveAsync(
            Stream imageStream,
            string contentType,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            string imageReference,
            CancellationToken cancellationToken);

    }
}
