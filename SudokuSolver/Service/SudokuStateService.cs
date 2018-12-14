using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using SudokuSolver.Model;

namespace SudokuSolver.Service
{
  public interface ISudokuStateService
  {
    StateModel GetState(StreamReader stream);
    void WriteTo(StateModel state, Stream stream);
  }

  public class SudokuStateService : ISudokuStateService
  {
    public StateModel GetState(StreamReader stream)
    {
      var state = new StateModel();
      var config = new Configuration { Delimiter = "," };
      using (var reader = new CsvReader(stream, config))
      {
        for (int y = 0; y < 9; y++)
        {
          reader.Read();

          for (int x = 0; x < 9; x++)
            state.State[x, y] = reader.GetField<int?>(x);
        }
      }
      return state;
    }

    public void WriteTo(StateModel state, Stream stream)
    {
      using (var sw = new StreamWriter(stream))
      using (var writer = new CsvWriter(sw))
      {
        for (int y = 0; y < 9; y++)
        {
          for (int x = 0; x < 9; x++)
          {
            writer.WriteField<int?>(state.State[x, y]);
          }
          writer.NextRecord();
        }
      }
    }
  }
}
