using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CustomerManagementSystem.Helpers;
using CustomerManagementSystem.Models.ViewModels;
using CustomerManagementSystem.Models.Entities;
using CustomerManagementSystem.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

namespace CustomerManagementSystem.Areas.Api.Customer
{
    [Authorize]
    [ApiController]
    [Route("Api/Customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        
        public CustomerController(
            ICustomerRepository customerRepository, 
            IMapper mapper, 
            IWebHostEnvironment environment, 
            ICommentRepository commentRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _environment = environment;
            _commentRepository = commentRepository;
        }

        /// <summary>
        /// GET ALL CUSTOMERS
        /// </summary>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllCustomers([FromQuery] string filter, [FromQuery] int pageIndex = 1, 
            [FromQuery] int size = 10, [FromQuery] string sortBy = "Id")
        {
            IEnumerable<CustomerViewModel> qry = !string.IsNullOrEmpty(filter)
                ? _customerRepository.AllIncluding()
                    .Where(n => n.Name.ToLower()
                    .Contains(filter.ToLower()))
                    .Select(c => _mapper.Map<CustomerViewModel>(c))
                    .ToList()
                : _customerRepository.AllIncluding()
                    .Select(c => _mapper.Map<CustomerViewModel>(c))
                    .ToList();
            
            var result = 
                PagingList.Create(qry, size, pageIndex, sortBy, "Id");

            return Ok(result);
        }

        /// <summary>
        /// GET A SINGLE CUSTOMER
        /// </summary>
        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult GetSingleCustomer(int id)
        {
            var customer = _customerRepository.GetSingle(c => c.Id == id);
            if (customer == null)
                return NotFound(new {message = "Customer not found"});
            
            return Ok(_mapper.Map<CustomerViewModel>(customer));
        }

        /// <summary>
        /// CREATE A CUSTOMER
        /// </summary>
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerPostModel model)
        {
            if (!ModelState.IsValid)
            {
                
            }
            
            var customer = _mapper.Map<Models.Entities.Customer>(model);
            customer.ProfilePicture = await ImageUploader(model.Image);
            customer.CreatedDate = DateTime.Now;
            customer.CreatedBy = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _customerRepository.Add(customer);
            await _customerRepository.CommitAsync();
            return Ok();
        }
        
        /// <summary>
        /// UPDATE A CUSTOMER
        /// </summary>
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateCustomer([FromQuery] int id, [FromBody] CustomerPostModel model)
        {
            if (!ModelState.IsValid)
            {
                
            }
            
            var customer = _customerRepository.GetSingle(c => c.Id == id);
            
            if (customer == null)
                return NotFound(new {message = "Customer not found"});
            
            customer.LastModifiedDate = DateTime.Now;
            customer.LastModifiedBy = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            customer.Name = model.Name;
            customer.Address = model.Address;
            customer.EmailAddress = model.EmailAddress;
            customer.ProfilePicture = await ImageUploader(model.Image);
            
            _customerRepository.Update(customer);

            return Ok();
        }

        /// <summary>
        /// DELETE A CUSTOMER
        /// </summary>
        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteCustomer([FromQuery] int id)
        {
            var customer = _customerRepository.GetSingle(c => c.Id == id);
            if (customer == null)
                return NotFound(new {message = "Customer not found"});
            
            _customerRepository.Delete(customer);
            return Ok();
        }

        /// <summary>
        /// SEND COMMENT TO A CUSTOMER
        /// </summary>
        [HttpPost]
        [Route("{id}/Comment")]
        public async Task<IActionResult> SendCustomerComment(int id, [FromBody]CommentPostModel body)
        {   
            var customer = _customerRepository.GetSingle(c => c.Id == id);
            if (customer == null)
                return NotFound(new {message = "Customer not found"});

            Comment comment = new Comment();
            comment.Message = body.Comment;
            comment.CustomerId = customer.Id;
            
            await _commentRepository.Add(comment);
            await _commentRepository.CommitAsync();
            
            CustomerCommentPostModel customerCommentPostModel = new CustomerCommentPostModel();
            customerCommentPostModel.Comment = body.Comment;
            customerCommentPostModel.Header = "Customer Management System";
            customerCommentPostModel.Name = customer.Name;
            customerCommentPostModel.Subject = "Customer Management System Comment";
            customerCommentPostModel.EmailAddress = customer.EmailAddress;

            await Mailer.SendCommentToCustomer(customerCommentPostModel);
            
            return Ok();
        }

        private async Task<string> ImageUploader(string image)
        {
            if (!string.IsNullOrEmpty(image))
            {
                var bytes = Convert.FromBase64String(image);
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", "profileImage");
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                var fileName = DateTime.Now.ToFileTime() + ".png";
                var fileNamePath = Path.Combine(filePath, fileName);
                if (bytes.Length > 0)
                {
                    await using var stream = new FileStream(fileNamePath, FileMode.Create);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
                return fileNamePath;
            }
            return "no-file.png";
        }
    }
}