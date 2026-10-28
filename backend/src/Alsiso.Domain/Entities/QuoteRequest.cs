using System;
using System.Collections.Generic;
using System.Text;
using Alsiso.Domain.Enums;

namespace Alsiso.Domain.Entities
{
   
    public sealed class QuoteRequest
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string CustomerName { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }
        public int Quantity { get; private set; }
        public string? Message { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public QuoteRequestStatus Status { get; private set; }

        public QuoteRequest(
            Guid id,
            Guid productId,
            string productName,
            string customerName,
            string email,
            string? phone,
            int quantity,
            string? message,
            DateTimeOffset createdAt)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(
                    "El identificador de la solicitud es obligatorio.",
                    nameof(id));
            }

            if (productId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Debes indicar el producto.",
                    nameof(productId));
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException(
                    "El nombre del producto es obligatorio.",
                    nameof(productName));
            }

            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new ArgumentException(
                    "El nombre del cliente es obligatorio.",
                    nameof(customerName));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(
                    "El correo electrónico es obligatorio.",
                    nameof(email));
            }

            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "La cantidad debe ser mayor que cero.",
                    nameof(quantity));
            }

            Id = id;
            ProductId = productId;
            ProductName = productName.Trim();
            CustomerName = customerName.Trim();
            Email = email.Trim();
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
            Quantity = quantity;
            Message = string.IsNullOrWhiteSpace(message) ? null : message.Trim();
            CreatedAt = createdAt.ToUniversalTime();
            Status = QuoteRequestStatus.New;
        }


        public void ChangeStatus(QuoteRequestStatus status)
        {
            if (status is not QuoteRequestStatus.New
                and not QuoteRequestStatus.Contacted
                and not QuoteRequestStatus.Closed)
            {
                throw new ArgumentOutOfRangeException(nameof(status));
            }

            Status = status;
        }






    }


}

