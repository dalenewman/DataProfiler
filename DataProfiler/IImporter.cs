namespace DataProfiler {
    public interface IImporter {
        Result Import(string resource, decimal sample = 100m);
    }
}