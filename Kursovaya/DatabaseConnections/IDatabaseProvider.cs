using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.DatabaseConnections
{
    public interface IDatabaseProvider
    {
        List<string[]> GetGameData();
        int GetLevel(string characterName);
    }
}
