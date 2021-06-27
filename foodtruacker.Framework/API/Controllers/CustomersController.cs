using foodtruacker.API.DTOs;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.Queries;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using foodtruacker.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace foodtruacker.API.Controllers
{
    
    [Authorize(Roles = "Admin, Customer")]
    public class CustomersController : ApiController
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomersInfo()
        {
            var query = new GetAllCustomerInfosQuery();

            List<CustomerInfo> result = await _mediator.Send(query);
            return result switch
            {
                not null => Ok(result),
                null => NotFound()
            };
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCustomerInfo(Guid id)
        {
            var query = new GetCustomerInfoQuery(id);

            CustomerInfo result = await _mediator.Send(query);
            return result switch
            {
                not null => Ok(result),
                null => NotFound()
            };
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(CreateCustomerAccountDTO dto)
        {
            var command = new CustomerAccountCreateCommand
            {
                Email = dto.Email,
                PlainPassword = dto.Password,
                Firstname = dto.Firstname,
                Lastname = dto.Surname,
            };

            CommandResult result = await _mediator.Send(command);
            return result.IsSuccess switch
            {
                true => Ok(),
                false => HandleFailedCommand(result)
            };
        }

        [Authorize(Roles = "CUSTOMER")]
        [HttpPatch]
        [Route("{id}/email")]
        public async Task<IActionResult> ChangeEmail(UpdateEmailAddressDTO dto, Guid id)
        {
            var command = new CustomerAccountChangeEmailCommand
            {
                ExpectedVersion = dto.ExpectedVersion,
                UserId = id,
                NewEmail = dto.Email
            };

            CommandResult result = await _mediator.Send(command);
            return result.IsSuccess switch
            {
                true => Ok(),
                false => HandleFailedCommand(result)
            };
        }

        [Authorize(Roles = "CUSTOMER")]
        [HttpPatch]
        [Route("{id}/password")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordDTO dto, Guid id)
        {
            var command = new CustomerAccountChangePasswordCommand
            {
                ExpectedVersion = dto.ExpectedVersion,
                UserId = id,
                CurrentPassword = dto.CurrentPassword,
                NewPassword = dto.NewPassword
            };

            CommandResult result = await _mediator.Send(command);
            return result.IsSuccess switch
            {
                true => Ok(),
                false => HandleFailedCommand(result)
            };
        }

    }
}
