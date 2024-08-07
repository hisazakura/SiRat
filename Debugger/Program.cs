using SiRat.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        GlobalData.LoadAll();
        Console.WriteLine("Santri: " + GlobalData.SantriList.Count.ToString());
        if (GlobalData.SantriList.Count == 0) return;
        Console.WriteLine("[0]Reports: " + GlobalData.SantriList[0].Reports.Count.ToString());
        if (GlobalData.SantriList[0].Reports.Count == 0) return;
        Console.WriteLine("Queried Result: " + GlobalData.SantriList[0].Reports[1].SpreadsheetData.Query("SUMACROSS(\"Doa\".\"Nilai\"[0])")?.ToString());
    }
}