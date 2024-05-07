

using System.Security.Claims;

namespace API_EF.Helper
{
    public static class ClaimHelper
    {
       
            public static string SerializePermissions(params int[] permissions)
            {
                return permissions.Serialize();
            }

        public static List<int> DeserializePermissions(this Claim claim)
        {
            return claim.Value.Deserialize<List<int>>();
        }

    }
}
