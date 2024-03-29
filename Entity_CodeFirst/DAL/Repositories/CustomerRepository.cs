﻿using Entity_CodeFirst.DAL.Interfaces;
using Entity_CodeFirst.Entities;
using Entity_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity_CodeFirst.DAL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CustomerModel>> GetCustomersAsync()
        {
            var query = from c in _dbContext.Customers
                        select new CustomerModel
                        {
                            Id = c.Id,
                            OrderHistoryId = c.OrderHistoryId,
                            Name = c.Name,
                            Address = c.Address
                        };

            var customers = await query.ToListAsync();

            return customers;
        }

        public async Task<CustomerModel> GetCustomerAsync(int id)
        {
            //var customer = await _dbContext.Customers.FindAsync(id);

            var query = from c in _dbContext.Customers
                        where c.Id == id
                        select new CustomerModel
                        {
                            Id = c.Id,
                            OrderHistoryId = c.OrderHistoryId,
                            Name = c.Name,
                            Address = c.Address
                        };
            
            var customer = await query.FirstOrDefaultAsync();

            return customer;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(int customerId, Customer customer)
        {
            var newCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);

            if (newCustomer == null)
            {
                return;
            }

            newCustomer.OrderHistoryId = customer.OrderHistoryId;
            newCustomer.Name = customer.Name;
            newCustomer.Address = customer.Address;

            _dbContext.Customers.Update(newCustomer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
            {
                return;
            }

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();
        }

    }
}
