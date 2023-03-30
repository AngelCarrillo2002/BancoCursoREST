using Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feautres.Clientes.Coomands.CreateClienteCommand
{
    public class CreateClienteCommand : IRequest<Response<int>>
    {
        private int _edad;
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
    }
    public class CreateClienteCommandHnadler : IRequestHandler<CreateClienteCommand, Response<int>>
    {
        public async Task<Response<int>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
