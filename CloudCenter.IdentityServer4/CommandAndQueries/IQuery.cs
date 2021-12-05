using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCenter.IdentityServer4.CommandAndQueries
{
    /// <summary>
    /// Interface that represents a query to the system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuery<T> : IRequest<T>
    {
    }
}
