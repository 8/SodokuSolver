using Autofac;

namespace SudokuSolver
{
  public class ContainerFactory
  {
    public ILifetimeScope GetContainer()
    {
      var builder = new ContainerBuilder();
      builder.RegisterModule<SudokuModule>();
      return builder.Build();
    }
  }
}
