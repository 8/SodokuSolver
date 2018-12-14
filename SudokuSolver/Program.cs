using SudokuSolver.Factory;
using SudokuSolver.Model;
using SudokuSolver.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
      StateModel state;

      /* load from input file */
      using (var sr = File.OpenText(p.InputFile))
        state = this.sudokuStateService.GetState(sr);

      /* simple solver */
      state = Solve(state);

      /* write to output file */
      using (var fs = File.OpenWrite(p.OutputFile))
        this.sudokuStateService.WriteTo(state, fs);
    }

    private StateModel Solve(StateModel state)
    {
      bool IsXAxisValid(StateModel s, (int x, int y, int value) newValue)
      {
        for (int sX = 0; sX < 9; sX++)
          if (sX != newValue.x && s.State[sX, newValue.y] == newValue.value)
            return false;

        return true;
      }

      bool IsYAxisValid(StateModel s, (int x, int y, int value) newValue)
      {
        for (int sY = 0; sY < 9; sY++)
          if (sY != newValue.y && s.State[newValue.x, sY] == newValue.value)
            return false;

        return true;
      }

      bool IsRegionValid(StateModel s, (int x, int y, int value) newValue)
      {
        for (int sX = (newValue.x / 3)*3; sX < (newValue.x / 3)*3 + 3; sX++)
          for (int sY = (newValue.y / 3)*3; sY < (newValue.y / 3)*3 + 3; sY++)
          {
              if (!(sY == newValue.y && sX == newValue.x) && s.State[sX, sY] == newValue.value)
                return false;
          }
        return true;
      }

      bool AreConstraintsFulfilled(StateModel s, (int x, int y, int value) newValue)
        => IsXAxisValid(s, newValue) && IsYAxisValid(s, newValue) && IsRegionValid(s, newValue);

      int[] GetValidValues(StateModel s, (int x, int y) position)
      {
        var validValues = new List<int>();
        foreach (var value in Enumerable.Range(1, 9))
        {
          if (AreConstraintsFulfilled(s, (position.x, position.y, value)))
            validValues.Add(value);
        }
        return validValues.ToArray();
      }

      loopit:
      for (int x = 0; x < 9; x++)
        for (int y = 0; y < 9; y++)
        {
          if (state.State[x, y] == null)
          {
            var validValues = GetValidValues(state, (x, y));

            if (validValues.Length == 1)
            {
              state.State[x, y] = validValues.Single();
              goto loopit;
            }
          }
        }

      return state;
    }
  }
}
