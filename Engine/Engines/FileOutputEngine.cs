using Contracts;
using Microsoft.Extensions.Logging;
using Models;
using Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Engine.Engines
{
    public class FileOutputEngine : LoggingWorker, ITemplateOutputEngine
    {
        private readonly List<string> _alreadyCleanedDirectories = new List<string>();

        public FileOutputEngine(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        /// <summary>
        /// Currently not using processTemplateStubs for my calculation, that might change
        /// </summary>
        /// <param name="path"></param>
        /// <param name="output"></param>
        /// <param name="isStub"></param>
        /// <param name="processTemplateStubs"></param>
        /// <returns></returns>
        public OperationResult Write(string path, string output, bool isStub, bool processTemplateStubs)
        {
            try
            {
                // prepare destination directory - todo: does this work if several directories need to be created?
                var destinationDirectory = path.Substring(0, path.LastIndexOf('\\'));
                Directory.CreateDirectory(destinationDirectory);
                var exists = File.Exists(path);

                if (!isStub)
                {
                    // normalish path
                    File.WriteAllText(path, output);
                }
                else
                {
                    // only write stubs if they don't already exist unless the settings
                    if (!exists)
                    {
                        File.WriteAllText(path, output);
                    }
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
            return OperationResult.Ok();
        }

        public OperationResult Write(string destinationPath, string entityName, string schema, string output, bool isStub, bool processTemplateStubs)
        {
            var validFileName = makeValidFileName(entityName);
            var validSchemaName = makeValidFileName(schema);
            var path = destinationPath.Replace("{entityName}", validFileName).Replace("{schema}", validSchemaName);
            return Write(path, output, isStub, processTemplateStubs);
        }

        public OperationResult CleanupOutputDirectory(string contextTemplateDirectory)
        {
            if (_alreadyCleanedDirectories.Contains(contextTemplateDirectory))
            {
                return OperationResult.Ok();
            }
            _alreadyCleanedDirectories.Add(contextTemplateDirectory);

            var di = new DirectoryInfo(contextTemplateDirectory);
            try
            {
                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception e)
            {
                return OperationResult.Fail($"Error FileOutputEngine.CleanupOutputDirectory: {e.Message}");
                throw;
            }
            return OperationResult.Ok();
        }

        public OperationResult Rename(string destinationPath, string source, string destination, string classRenameValue, string classReplaceValue)
        {
            var sourceFullPath = Path.Combine(destinationPath, source);
            var destinationFullPath = Path.Combine(destinationPath, destination);
            var sourceFileInfo = new FileInfo(sourceFullPath);
            if (!sourceFileInfo.Exists)
            {
                return OperationResult.Fail($"Source file doesn't exist: {sourceFullPath}.");
            }
            var destinationFileInfo = new FileInfo(destinationFullPath);
            if (destinationFileInfo.Exists)
            {
                // delete the destination file if it exists
                try
                {
                    destinationFileInfo.Delete();
                }
                catch (Exception ex)
                {
                    return OperationResult.Fail($"Destination file couldn't be deleted: {destinationFullPath}.\r\nError: {ex.Message}");
                }
            }
            try
            {
                // now rename the source file
                File.Move(sourceFullPath, destinationFullPath);
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"File ({sourceFullPath}) couldn't be renamed to: {destinationFullPath}.\r\nError: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(classRenameValue) && !string.IsNullOrEmpty(classReplaceValue))
            {
                try
                {
                    // now replace the file class name using whatever is in the classRenamePattern
                    var fileContents = File.ReadAllText(destinationFullPath).Replace(classRenameValue, classReplaceValue);
                    File.WriteAllText(destinationFullPath, fileContents);
                }
                catch (Exception ex)
                {
                    return OperationResult.Fail($"File ({destinationFullPath}) text replacement failed.\r\nError: {ex.Message}");
                }
            }

            return OperationResult.Ok();
        }

        private static string makeValidFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return Regex.Replace(name, invalidRegStr, "_");
        }
    }
}