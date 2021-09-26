using System.Threading.Tasks;

namespace DataImporter.Core.Abstractions
{
  public interface IDataImporterService
  {
        public Task<ImportResult> importAll();

  }
}
