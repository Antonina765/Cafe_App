using System.Reflection;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Cafe.Data.Repositories;

namespace Cafe_App.Services;

public class RegistrationHelper
{
        public void AutoRegisterRepositories(IServiceCollection services)
        {
            var baseRepositoryClassType = typeof(BaseRepository<>);
            var baseRepositoryInterfaceType = typeof(IBaseRepository<>);
            var assembly = Assembly.GetAssembly(baseRepositoryClassType);

            var repositoryInterfaces = assembly
                .GetTypes()
                .Where(t => t.IsInterface // ICakeRepositoryReal
                    && t.GetInterfaces()
                        .Any(i => //ICakeRepository<CakeData>
                            i.GetInterfaces()
                                .Any(i2 => i2.IsGenericType
                                    && i2.GetGenericTypeDefinition() == baseRepositoryInterfaceType)
                        )
                    );

            var repositoryClasses = assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == baseRepositoryClassType);

            foreach (var repositoryInterface in repositoryInterfaces)
            {
                var repositoryClass = repositoryClasses
                    .FirstOrDefault(c => c.GetInterfaces().Any(i => i == repositoryInterface));
                if (repositoryClass == null)
                {
                    Console.WriteLine($"!!! {repositoryInterface.Name} => NO CLASSES");
                }
                else
                {
                    //services.AddScoped<IFilmDirectorRepositoryReal, FilmDirectorRepository>();
                    //services.AddScoped(typeof(IFilmDirectorRepositoryReal), typeof(FilmDirectorRepository));
                    services.AddScoped(repositoryInterface, repositoryClass);

                    Console.WriteLine($"{repositoryInterface.Name} => {repositoryClass.Name} Registered +");
                }
            }
        }

        public void AutoRegisterServiceByAttribute(IServiceCollection services)
        {
            var attributeType = typeof(AutoRegisterFlagAttribute);
            var assembly = Assembly.GetAssembly(attributeType);

            var classesToRegistration = assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && t.GetCustomAttribute<AutoRegisterFlagAttribute>() != null);
            foreach (var classToRegistration in classesToRegistration)
            {
                services.AddScoped(classToRegistration);
                Console.WriteLine($"{classToRegistration.Name} Registered +");
            }
        }

        public void AutoRegisterServiceByAttributeOnConstructor(IServiceCollection services)
        {
            var attributeType = typeof(AutoRegisterFlagAttribute);
            var assembly = Assembly.GetAssembly(attributeType);

            var classesToRegistration = assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && t.GetConstructors()
                        .Any(c => c.GetCustomAttribute<AutoRegisterFlagAttribute>() != null));

            foreach (var classToRegistration in classesToRegistration)
            {
                // classToRegistration == GeneratorAnimeGirls
                services.AddScoped(classToRegistration, (diContainer) =>
                {
                    var constructor = classToRegistration
                        .GetConstructors()
                        .First(c => c.GetCustomAttribute<AutoRegisterFlagAttribute>() != null);

                    var parameterInfos = constructor
                        .GetParameters();

                    var parametersForConstructor = parameterInfos
                        .Select(parameterInfo => diContainer.GetService(parameterInfo.ParameterType))
                        .ToArray();

                    var serviceObject = constructor.Invoke(parametersForConstructor);// new GeneratorAnimeGirls();

                    return serviceObject;
                });

                //services.AddScoped<GeneratorAnimeGirls>(di =>
                //{
                //    var repository = di.GetService<IAnimeGirlRepositoryReal>();
                //    return new GeneratorAnimeGirls(repository);
                //});
                
                Console.WriteLine($"{classToRegistration.Name} Registered +");
            }
        }
}