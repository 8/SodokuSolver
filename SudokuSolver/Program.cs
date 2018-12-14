using SudokuSolver.Factory;
using SudokuSolver.Model;
using SudokuSolver.Service;
using System.IO;

namespace SudokuSolver
{
  class Program
  {
    private readonly IParameterFactory parameterFactory;
    private readonly ISudokuStateService sudokuStateService;

    public Program(IParameterFactory parameterFactory,
                   ISudokuStateService sudokuStateService)
    {
      this.parameterFactory = parameterFactory;
      this.sudokuStateService = sudokuStateService;
    }

    public void Run(string[] args)
    {
      var p = this.parameterFactory.Get(args);

      HandleParameters(p);
    }

    private void HandleParameters(ParameterModel p)
    {
      SudokuStateModel state;

      /* load from input file */
      using (var sr = File.OpenText(p.InputFile))
         state = this.sudokuStateService.GetState(sr);

      /* write to output file */
      using (var fs = File.OpenWrite(p.OutputFile))
        this.sudokuStateService.WriteTo(state, fs);
    }
  }
}
