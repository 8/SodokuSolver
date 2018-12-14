using Autofac;

namespace SudokuSolver
{
  static class EntryPoint
  {
    static void Main(string[] args)
    {
      using (var container = new ContainerFactory().GetContainer())
      {
        var program = container.Resolve<Program>();
        program.Run(args);
      }
    }
  }
}
