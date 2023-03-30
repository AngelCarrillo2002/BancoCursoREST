using Application.Feautres.Clientes.Coomands.CreateClienteCommand;
using Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banco.Controllers.Users
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        //POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateClienteCommand command, CancellationToken cancellationToken = default)
        {
            var task = Task.Run<Response<int>> (async () =>
            {
                var result = await mediator.Send(command);
                return result;
            }, cancellationToken);
            return Ok(task);
        }
    }
}
