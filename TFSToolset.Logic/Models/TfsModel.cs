namespace TFSToolset.Logic.Models
{
    public class TfsModel
    {
        string TfsServerName { get; set; }
        string TfsProjectName { get; set; }
        string OriginalQueryFolder { get; set; }
        string NewQueryFolder { get; set; }
        string OriginalQueryText { get; set; }
        string NewQueryText { get; set; }
    }
}