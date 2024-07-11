using SimpleChatApplication.BLL.Mappings;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.Models.Lookups {
    public abstract class BaseLookup<EntityType> : IMapWith<EntityType> where EntityType : class, IEntity {

    }
}
