using System.Collections.Generic;

namespace Contracts
{
    public interface IGroupProviderModel
    {
        List<string> Imports { get; set; }

        public IEnumerable<IProviderModel> Models { get; set; }

        string Namespace { get; set; }

        string Schema { get; set; }
    }
}