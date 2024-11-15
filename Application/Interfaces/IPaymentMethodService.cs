using Domain.Entities;

namespace Application.Interfaces;

public interface IPaymentMethodService
{
    Task<IEnumerable<Paymentmethod>> GetAllPaymentMethods();
    Task<Paymentmethod> AddPaymentMethod(Paymentmethod paymentMethod);
    Task<Paymentmethod> UpdatePaymentMethod(Paymentmethod paymentMethod);
    Task<Paymentmethod> DeletePaymentMethod(int id);
    Task<Paymentmethod> GetPaymentMethodById(int id);
}