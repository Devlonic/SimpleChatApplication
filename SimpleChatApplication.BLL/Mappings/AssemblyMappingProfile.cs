using AutoMapper;
using System.Reflection;

namespace SimpleChatApplication.BLL.Mappings {
    public class AssemblyMappingProfile : Profile {
        public AssemblyMappingProfile(Assembly assembly) => ApplyMappingsFromAssembly(assembly);

        private void ApplyMappingsFromAssembly(Assembly assembly) {
            // get all classes, that implements IMapWith<>
            var types = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                .ToList();

            foreach ( var type in types ) {
                // code below creates objects by the type, takes Mapping methods from objects
                // and calls this method, having passed into object array with only one element - 
                // reference to current instance of AssemblyMappingProfile
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }

        }
    }
}
