using System.Reflection;
using Autofac;
using SudokuSolver.Factory;
using SudokuSolver.Service;

namespace SudokuSolver
{
  public class SudokuModule : Autofac.Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      var assembly = Assembly.GetExecutingAssembly();

      builder.RegisterType<Program>().AsSelf();

      /* register factories */
      builder.RegisterAssemblyTypes(assembly)
        .InNamespaceOf<IParameterFactory>()
        .AsImplementedInterfaces();

      /* register services */
      builder.RegisterAssemblyTypes(assembly)
        .InNamespaceOf<ISudokuStateService>()
        .AsImplementedInterfaces()
        .SingleInstance();
    }
  }
}
