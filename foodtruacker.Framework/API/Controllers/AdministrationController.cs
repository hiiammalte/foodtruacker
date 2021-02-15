using foodtruacker.API.DTOs;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.Queries;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using foodtruacker.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace foodtruacker.API.Controllers
{
    public class AdministrationController : ApiController
    {
        private readonly IMediator _mediator;

        public AdministrationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateAdminAccountDTO dto)
        {
            var command = new AdminAccountCreateCommand
            {
                Email = dto.Email,
                PlainPassword = dto.Password,
                Firstname = dto.Firstname,
                Lastname = dto.Surname,
                SecretProductKey = dto.SecretProductKey
            };

            CommandResult result = await _mediator.Send(command);
            return result.IsSuccess switch
            {
                true => Ok(),
                false => HandleFailedCommand(result)
            };
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAdminInfo(Guid Id)
        {
            var query = new GetAdminInfoQuery(Id);

            AdminInfo result = await _mediator.Send(query);
            return result switch
            {
                not null => Ok(result),
                null => NotFound()
            };
        }

        [HttpPatch]
        [Route("{id}/email")]
        public async Task<IActionResult> ChangeEmail(UpdateEmailAddressDTO dto, Guid id)
        {
            var command = new AdminAccountChangeEmailCommand
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

        [HttpPatch]
        [Route("{id}/password")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordDTO dto, Guid id)
        {
            var command = new AdminAccountChangePasswordCommand
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
