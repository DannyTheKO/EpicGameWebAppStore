using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<IEnumerable<Paymentmethod>> GetAllPaymentMethodsAsync()
        {
            
            return await _paymentMethodRepository.GetAll();
        }

        public async Task<Paymentmethod> AddPaymentMethodAsync(Paymentmethod paymentMethod)
        {
            await _paymentMethodRepository.Add(paymentMethod);
            return paymentMethod;
        }

        public async Task<Paymentmethod> UpdatePaymentMethodAsync(Paymentmethod paymentMethod)
        {
            await _paymentMethodRepository.Update(paymentMethod);
            return paymentMethod;
        }

        public async Task<Paymentmethod> DeletePaymentMethodAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetById(id);
            if (paymentMethod == null) throw new Exception("Payment method not found.");
            await _paymentMethodRepository.Delete(id);
            return paymentMethod;
        }

        public async Task<Paymentmethod> GetPaymentMethodByIdAsync(int id)
        {
            return await _paymentMethodRepository.GetById(id);
        }
    }
} 