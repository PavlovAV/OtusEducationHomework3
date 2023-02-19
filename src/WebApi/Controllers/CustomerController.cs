using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> _customerRepository;
        public CustomerController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("{id:long}")]   
        public async Task<IActionResult> GetCustomerAsync([FromRoute] long id)
        {
            var customer = await _customerRepository.GetAsync(id);
            if (customer == null)
            {
                return NotFound("Покупатель с таким id не существует");
            }

            return Ok(customer);
        }

        [HttpPost("")]   
        public async Task<ActionResult<long>> CreateCustomerAsync([FromBody] Customer customer)
        {
            //var rnd = new Random();
            //customer.Id = rnd.Next(1, 100);
            var existCustomer = await _customerRepository.GetAsync(customer.Id);
            if (existCustomer != null)
            {
                return Conflict("Покупатель с id " + customer.Id + " уже существует");
            }
            await _customerRepository.AddAsync(customer);
            return Ok(customer.Id);
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomerAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> PutCustomerAsync(long id, [FromBody] Customer entity)
        {
            var customer = await _customerRepository.GetAsync(id);
            if (customer == null)
            {
                return NotFound("Покупатель с таким id не существует");
            }

            await _customerRepository.UpdateAsync(entity);
            return Ok();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCustomerAsync(long id)
        {
            var customer = await _customerRepository.GetAsync(id);
            if (customer == null)
            {
                return NotFound("Покупатель с таким id не существует");
            }

            await _customerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}