using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class OgnpFlow
{
    public OgnpFlow(int flowNumber, string megaFacultyName)
    {
        ArgumentNullException.ThrowIfNull(megaFacultyName);
        if (flowNumber < 1)
        {
            throw new FlowNumberException("flow number cannot be less than 1.");
        }

        FlowNumber = flowNumber;
        MegaFacultyName = megaFacultyName;
        ListOfOgnpGroups = new List<OgnpGroup>();
    }

    public int FlowNumber { get; private set; }
    public string MegaFacultyName { get; private set; }
    private List<OgnpGroup> ListOfOgnpGroups { get; set; }
    public IReadOnlyList<OgnpGroup> GetGroups() => ListOfOgnpGroups;

    public void AddOgnpGroup(OgnpGroup ognpGroup)
    {
        ognpGroup.FlowNumber = this.FlowNumber;
        ArgumentNullException.ThrowIfNull(ognpGroup);
        ListOfOgnpGroups.Add(ognpGroup);
    }

    public bool CheckMegaFacultyAndNumberOfFlow(string megafFacultyName, int numberOfFLow)
    {
        ArgumentNullException.ThrowIfNull(megafFacultyName);
        ArgumentNullException.ThrowIfNull(numberOfFLow);
        return megafFacultyName.Equals(this.MegaFacultyName) && numberOfFLow == FlowNumber;
    }
}