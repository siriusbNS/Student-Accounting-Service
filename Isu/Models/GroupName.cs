using System.Text.RegularExpressions;
using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    private static readonly Regex GroupNameRegex = new (@"^[A-Z][1-6][1-6]\d{2}[1-2]?$", RegexOptions.Compiled);
    public GroupName(string name)
    {
        if (name is null)
        {
            throw new GroupNameNullException("Group name is nullable.");
        }

        if (!GroupNameRegex.IsMatch(name))
        {
            throw new GroupNameException("No such Group exist.");
        }

        NameOfGroup = name;
    }

    public string NameOfGroup { get; private set; }
}