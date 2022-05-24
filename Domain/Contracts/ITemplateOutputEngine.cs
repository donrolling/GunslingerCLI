using Domain.Models.General;

namespace Contracts
{
	public interface ITemplateOutputEngine
	{
		OperationResult Write(string path, string output, bool isStub, bool processTemplateStubs);

		OperationResult Write(string destinationPath, string entityName, string schema, string output, bool isStub, bool processTemplateStubs);

		OperationResult CleanupOutputDirectory(string contextTemplateDirectory);

		OperationResult Rename(string destinationPath, string source, string destination, string classRenameValue, string classReplaceValue);
	}
}