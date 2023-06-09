﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    public class ValidationBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request,CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(_validators.Any())
            {
                var context = new FluentValidation.ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(failures => failures != null).ToList();

                if (failures.Count !=0)
                    throw new Exceptions.ValidationException(failures);
            }
            return await next();
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal struct NewStruct<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public IEnumerable<IValidator<TRequest>> validators;
        public object Item2;

        public NewStruct(IEnumerable<IValidator<TRequest>> validators, object item2)
        {
            this.validators = validators;
            Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            return obj is NewStruct<TRequest, TResponse> other &&
                   EqualityComparer<IEnumerable<IValidator<TRequest>>>.Default.Equals(validators, other.validators) &&
                   EqualityComparer<object>.Default.Equals(Item2, other.Item2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(validators, Item2);
        }

        public void Deconstruct(out IEnumerable<IValidator<TRequest>> validators, out object item2)
        {
            validators = this.validators;
            item2 = Item2;
        }

        public static implicit operator (IEnumerable<IValidator<TRequest>> validators, object)(NewStruct<TRequest, TResponse> value)
        {
            return (value.validators, value.Item2);
        }

        public static implicit operator NewStruct<TRequest, TResponse>((IEnumerable<IValidator<TRequest>> validators, object) value)
        {
            return new NewStruct<TRequest, TResponse>(value.validators, value.Item2);
        }
    }
}
