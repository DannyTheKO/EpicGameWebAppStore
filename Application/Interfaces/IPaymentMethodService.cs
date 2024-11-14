using Domain.Entities;

namespace Application.Interfaces;

public interface IPaymentMethodService
{
    Task<IEnumerable<Paymentmethod>> GetAllPaymentMethodsAsync();
    Task<Paymentmethod> AddPaymentMethodAsync(Paymentmethod paymentMethod);
    Task<Paymentmethod> UpdatePaymentMethodAsync(Paymentmethod paymentMethod);
    Task<Paymentmethod> DeletePaymentMethodAsync(int id);
    Task<Paymentmethod> GetPaymentMethodByIdAsync(int id);
}