using System;
using System.Linq;
using System.Collections.Generic;

namespace CodeGenHelpers
{
    internal class DocumentationComment
    {
        internal string? Summary { get; set; }

        internal Dictionary<string, string> ParameterDoc { get; } = new Dictionary<string, string>();

        internal bool InheritDoc { get; set; }

        internal string? InheritFrom { get; set; }

        internal void Write(ref CodeWriter writer)
        {
            if (InheritDoc)
            {
                writer.AppendLine($"/// <inheritdoc {(InheritFrom is null ? string.Empty : $"cref=\"{InheritFrom}\"")}/>");
                return;
            }

            if (Summary is {})
            {
                writer.AppendLine("/// <summary>");

                string[] lines = Summary.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                    writer.AppendLine($"/// {line}");

                writer.AppendLine("/// </summary>");
            }

            foreach (var param in ParameterDoc)
                writer.AppendLine($"/// <param name=\"{param.Key}\">{param.Value}</param>");
        }

        internal void RemoveUnusedParameters(Dictionary<string, string> parameters)
        {
            var unusedParameters = ParameterDoc.Where(p => !parameters.ContainsKey(p.Key)).ToArray();
            foreach ((var name, var _) in unusedParameters)
                ParameterDoc.Remove(name);
        }
    }
}