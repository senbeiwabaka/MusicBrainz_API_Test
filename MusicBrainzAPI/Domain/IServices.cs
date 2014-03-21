using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicBrainzAPI.Domain
{
    internal interface IServices
    {
        bool Get(string baseURL = "", string query = "");

        bool SearchGet(string baseURL = "", string query = "");
    }
}
