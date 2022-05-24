using System.Collections.Generic;
using Domain.Models.General;

namespace Contracts
{
	public interface IProviderModel
    {
        Name Name { get; set; }
        string Description { get; set; }
        List<string> Imports { get; set; }
        string Namespace { get; set; }
        string Schema { get; set; }
    }
}