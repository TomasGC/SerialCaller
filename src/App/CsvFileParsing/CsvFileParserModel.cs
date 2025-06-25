namespace SerialCaller.App.CsvFileParsing;

public class CsvFileParserModel(string ticketId, string csvFilePath)
{
    public class Filter(int column, IEnumerable<string>? authorizedValues)
    {
        public int Column { get; set; } = column;
        public IEnumerable<string>? AuthorizedValues { get; set; } = authorizedValues;
    }

    public string TicketId { get; set; } = ticketId;
    public char Delimiter { get; set; }
    public int RowsToSkip { get; set; }
    public string CsvFilePath { get; set; } = csvFilePath;
    public IEnumerable<Filter>? Filters { get; set; } = [];
    public IDictionary<string, int>? ColumnsByPropertyName { get; set; }
}
