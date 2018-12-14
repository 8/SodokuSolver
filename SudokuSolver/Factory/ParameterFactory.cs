using Mono.Options;
using SudokuSolver.Model;

namespace SudokuSolver.Factory
{
  interface IParameterFactory
  {
    ParameterModel Get(string[] args);
  }

  class ParameterFactory : IParameterFactory
  {
    public ParameterModel Get(string[] args)
    {
      var p = new ParameterModel();
      OptionSet options = new OptionSet();

      options.Add("i|input-file=", "the csv input file", s => p.InputFile = s);
      options.Add("o|output-file=", "the csv output file", s => p.OutputFile = s);

      var rest = options.Parse(args);
      return p;
    }
  }
}
