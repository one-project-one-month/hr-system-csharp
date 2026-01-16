using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using SkiaSharp;

namespace HRSystem.Csharp.Domain.Helpers;

public class ExportService
{
    public async Task<byte[]> ExportToCsv(List<Dictionary<string, object>> data, CsvConfiguration? config = null)
    {
        if (data == null || data.Count == 0)
            throw new ArgumentException("Data cannot be null or empty", nameof(data));

        config ??= new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true,
            MissingFieldFound = null,
        };

        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        using var csvWriter = new CsvWriter(streamWriter, config);

        if (data.Count > 0)
        {
            foreach (var key in data[0].Keys)
            {
                csvWriter.WriteField(key);
            }
            csvWriter.NextRecord();
        }

        foreach (var row in data)
        {
            foreach (var key in row.Keys)
            {
                csvWriter.WriteField(row[key]?.ToString() ?? string.Empty);
            }
            csvWriter.NextRecord();
        }

        await streamWriter.FlushAsync();
        return memoryStream.ToArray();
    }

    public Task<byte[]> ExportToPdf(List<Dictionary<string, object>> data, string title = "Export")
    {
        if (data == null || data.Count == 0)
            throw new ArgumentException("Data cannot be null or empty", nameof(data));

        using var memoryStream = new MemoryStream();
        using var document = SKDocument.CreatePdf(memoryStream, new SKDocumentPdfMetadata
        {
            Title = title,
            Author = "HR Management System",
            Subject = "Data Export",
            Creator = "Export Service"
        });

        using var canvas = document.BeginPage(595, 842);

        var titlePaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 20,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
        };

        var headerPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 14,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
        };

        var cellPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 12,
            IsAntialias = true
        };

        canvas.DrawText(title, 595 / 2 - titlePaint.MeasureText(title) / 2, 50, titlePaint);

        var headers = data.Count > 0 ? data[0].Keys.ToList() : new List<string>();
        float yPosition = 80;
        const float rowHeight = 20;
        const float cellPadding = 10;

        float[] columnWidths = new float[headers.Count];
        for (int i = 0; i < headers.Count; i++)
        {
            columnWidths[i] = headerPaint.MeasureText(headers[i]) + cellPadding * 2;
        }

        float xPosition = 50;
        for (int i = 0; i < headers.Count; i++)
        {
            canvas.DrawRect(xPosition, yPosition, columnWidths[i], rowHeight,
                new SKPaint { Color = new SKColor(240, 240, 240) });

            canvas.DrawText(headers[i],
                xPosition + cellPadding,
                yPosition + rowHeight - cellPadding,
                headerPaint);

            xPosition += columnWidths[i];
        }

        yPosition += rowHeight;

        foreach (var row in data)
        {
            xPosition = 50;
            for (int i = 0; i < headers.Count; i++)
            {
                var value = row.ContainsKey(headers[i]) ? row[headers[i]]?.ToString() ?? string.Empty : string.Empty;

                canvas.DrawText(value,
                    xPosition + cellPadding,
                    yPosition + rowHeight - cellPadding,
                    cellPaint);

                canvas.DrawRect(xPosition, yPosition, columnWidths[i], rowHeight,
                    new SKPaint { Color = SKColors.LightGray, Style = SKPaintStyle.Stroke });

                xPosition += columnWidths[i];
            }
            yPosition += rowHeight;
        }

        document.EndPage();
        document.Close();

        return Task.FromResult(memoryStream.ToArray());
    }

    public Task<byte[]> ExportToExcel(List<Dictionary<string, object>> data, string sheetName = "Export", Action<IXLWorksheet>? styleAction = null)
    {
        if (data is null || data.Count == 0)
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var headers = new List<string>();
        foreach (var dict in data)
        {
            foreach (var key in dict.Keys)
            {
                if (!headers.Contains(key))
                {
                    headers.Add(key);
                }
            }
        }

        for (int i = 0; i < headers.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
        }

        for (int row = 0; row < data.Count; row++)
        {
            var dict = data[row];
            for (int col = 0; col < headers.Count; col++)
            {
                var header = headers[col];
                if (dict.TryGetValue(header, out var value))
                {
                    worksheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? string.Empty;
                }
            }
        }

        styleAction?.Invoke(worksheet);

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return Task.FromResult(memoryStream.ToArray());
    }
}