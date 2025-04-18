using JournalAPI.Services.Journal.Interfaces;
using System.Globalization;

namespace JournalAPI.Models.Test;

public record TestDataModel(int Id, string Name, DateTime DateTime) : IUniqueObject
{ 
    public string GetUniqueKey()
    {
        return Id.ToString(CultureInfo.InvariantCulture);
    }
}
