using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<IEnumerable<Paymentmethod>> GetAllPaymentMethods()
    {
        return await _paymentMethodRepository.GetAll();
    }

    public async Task<Paymentmethod> AddPaymentMethod(Paymentmethod paymentMethod)
    {
        await _paymentMethodRepository.Add(paymentMethod);
        return paymentMethod;
    }

    public async Task<Paymentmethod> UpdatePaymentMethod(Paymentmethod paymentMethod)
    {
        await _paymentMethodRepository.Update(paymentMethod);
        return paymentMethod;
    }

    public async Task<Paymentmethod> DeletePaymentMethod(int id)
    {
        var paymentMethod = await _paymentMethodRepository.GetById(id);
        if (paymentMethod == null) throw new Exception("Payment method not found.");
        await _paymentMethodRepository.Delete(id);
        return paymentMethod;
    }

    public async Task<Paymentmethod> GetPaymentMethodById(int id)
    {
        return await _paymentMethodRepository.GetById(id);
    }
}