using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;
        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();

            if (request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "Greg";
                output.LastName = "Thomas";
            }
            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(
            NewCustomerRequest request,
            IServerStreamWriter<CustomerModel> responseStream,
            ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName="Adi",
                    LastName="Sekar",
                    EmailAddress="test@test.com",
                    Age = 32,
                    IsAlive = true
                },
                 new CustomerModel
                {
                    FirstName="Sue",
                    LastName="Storm",
                    EmailAddress="sue@stormy.net",
                    Age = 52,
                    IsAlive = false
                },
                  new CustomerModel
                {
                    FirstName="Bilbo",
                    LastName="Baggins",
                    EmailAddress="bilbo@hobbit.com",
                    Age = 102,
                    IsAlive = false
                }
            };

            // Send customer 1 at a time, async in a stream. Same like reading 1 line at a time
            foreach (var customer in customers)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(customer);
            }
        }
    }
}
