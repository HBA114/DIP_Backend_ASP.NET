namespace DIP_Backend.Entities;

public class HistogramEqualizeTable
{
    public int GrayTone { get; set; }
    public int CumulativeSummary { get; set; }
    public double CumulativeProbability { get; set; }
    public int NewGrayTone { get; set; }
}
