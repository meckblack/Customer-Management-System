using AutoMapper;
using CustomerManagementSystem.Models.Entities;
using CustomerManagementSystem.Models.ViewModels;

namespace CustomerManagementSystem.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Customer, CustomerViewModel>();
            
            CreateMap<CustomerPostModel, Customer>(); 
        }
    }
}