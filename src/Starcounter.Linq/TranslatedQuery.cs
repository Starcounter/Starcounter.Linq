namespace Starcounter.Linq
{
    public class TranslatedQuery
    {
        public string SqlStatement { get; set; }
        public QueryResultMethod ResultMethod { get; set; }
        public AggregationOperation? AggregationOperation { get; set; }
    }
}
