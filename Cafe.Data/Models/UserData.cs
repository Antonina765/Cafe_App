using Cafe.Data.Interface.Models;
using Enums.Users;

namespace Cafe.Data.Models;

public class UserData : BaseModel, IUser
{
        public IEnumerable<CafeData>? Cafes { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public Languages Language { get; set; }
        public Roles Role { get; set; }
        public virtual List<ChatMessageData> ChatMessages { get; set;} = new();
}
