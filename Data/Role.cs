using Azure;
using System.Data;

namespace SagErpBlazor.Data
{
    public class Role
    {
      
            public int RoleId { get; set; }
            public string RoleDescription { get; set; } =string.Empty;

        

    }
    public static class RoleDescriptionClass
    {

        public static Dictionary<int, string> RoleDescriptions = new Dictionary<int, string>
        {
            { 1, "Administrative" },
            { 2, "Support Engineer" },
            { 3, "Technical Engineer" },
            { 4, "Client" },
            { 5, "Operator" },
            
        };

        public static Dictionary<string, int> RoleIds = RoleDescriptions.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);


        public static int GetRoleIDFromDescription(string description)
        {
            int RoleID = 0;
            if (RoleDescriptionClass.RoleIds.TryGetValue(description, out var roleId))
            {
                RoleID = roleId;
            }

            return RoleID;
        }

    }
}
