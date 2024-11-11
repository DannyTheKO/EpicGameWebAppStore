using Domain.Entities;

namespace Domain.Repository;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<Paymentmethod>> GetAll();
    Task<Paymentmethod> GetById(int id);
    Task Add(Paymentmethod paymentMethod);
    Task Update(Paymentmethod paymentMethod);
    Task Delete(int id);
}