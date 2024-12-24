using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.IO;
using DataModels.Data;

namespace DataModels.Services
{
    public class SefariaService
    {
        private readonly SHcx Cx;
        private readonly HttpClient _httpClient;

        public Dictionary<string, string> TanachBooks { get; private set; }
        public Dictionary<string, string> MishnaTractates { get; private set; }
        private Dictionary<string, string> BookNameMapping { get; set; }

        public SefariaService(SHcx cx)
        {
            Cx = cx;
            _httpClient = new HttpClient();

            // Initialize Dictionaries
            TanachBooks = new Dictionary<string, string>
            {
                {"Bereshit", "בראשית"},
                {"Shemot", "שמות"},
                {"Vayikra", "ויקרא"},
                {"Bamidbar", "במדבר"},
                {"Devarim", "דברים"},
                {"Yehoshua", "יהושע"},
                {"Shoftim", "שופטים"},
                {"I Samuel", "שמואל א'"},
                {"II Samuel", "שמואל ב'"},
                {"I Kings", "מלכים א'"},
                {"II Kings", "מלכים ב'"},
                {"Yeshayahu", "ישעיהו"},
                {"Yirmeyahu", "ירמיהו"},
                {"Yechezkel", "יחזקאל"},
                {"Hoshea", "הושע"},
                {"Yoel", "יואל"},
                {"Amos", "עמוס"},
                {"Ovadiah", "עובדיה"},
                {"Yonah", "יונה"},
                {"Micha", "מיכה"},
                {"Nachum", "נחום"},
                {"Habakkuk", "חבקוק"},
                {"Zephaniah", "צפניה"},
                {"Haggai", "חגי"},
                {"Zechariah", "זכריה"},
                {"Malachi", "מלאכי"},
                {"Tehillim", "תהילים"},
                {"Mishlei", "משלי"},
                {"Iyov", "איוב"},
                {"Shir HaShirim", "שיר השירים"},
                {"Rut", "רות"},
                {"Eichah", "איכה"},
                {"Kohelet", "קוהלת"},
                {"Ester", "אסתר"},
                {"Daniel", "דניאל"},
                {"Ezra", "עזרא"},
                {"Nehemiah", "נחמיה"},
                {"I Chronicles", "דברי הימים א'"},
                {"II Chronicles", "דברי הימים ב'"},
            };

            MishnaTractates = new Dictionary<string, string>
            {     
                // Mappings for Mishna tractates
                {"Berakhot", "ברכות"},
                { "Peah", "פאה"},
                { "D’mai", "דמאי"},
                { "Kilayim", "כלאים"},
                { "Sheviit", "שביעית"},
                { "Terumot", "תרומות"},
                { "Maʿasroth", "מעשרות"},
                { "Maaser Sheni", "מעשר שני"},
                { "Hallah", "חלה"},
                { "Mishnah Orlah", "מִשְׁנָה ערלה"},
                { "Bikkurim", "ביקורים"},
                { "Shabbat", "שבת"},
                { "Eruvin", "עירובין"},
                { "Pesachim", "פסחים"},
                { "Mishnah Shekalim", "מִשְׁנָה שקלים"},
                { "Yoma", "יומא"},
                { "Sukkah", "סוכה"},
                { "Beitzah", "ביצה"},
                { "Rosh Hashanah", "ראש השנה"},
                { "Ta'anit", "תענית"},
                { "Megillah", "מגילה"},
                { "Moed Katan", "מועד קטן"},
                { "Chagigah", "חגיגה"},
                { "Yevamot", "יבמות"},
                { "Ketubot", "כתובות"},
                { "Nedarim", "נדרים"},
                { "Nazir", "נזיר"},
                { "Sotah", "סוטה"},
                { "Gittin", "גטין"},
                { "Kiddushin", "קידושין"},
                { "Bava Kamma", "בבא קמא"},
                { "Bava Metzia", "בבא מציעא"},
                { "Bava Batra", "בבא בתרא"},
                { "Sanhedrin", "סנהדרין"},
                { "Makkot", "מכות"},
                { "Shevuot", "שבועות"},
                { "Eduyot", "עדויות"},
                { "Avodah Zarah", "עבודה זרה"},
                { "Avot", "אבות"},
                { "Horayot", "הוריות"},
                { "Zevachim", "זבחים"},
                { "Menachot", "מנחות"},
                { "Chullin", "חולין"},
                { "Bekhorot", "בכורות"},
                { "Arakhin", "ערכין"},
                { "Temurah", "תמורה"},
                { "Keritot", "כריתות"},
                { "Meilah", "מעילה"},
                { "Tamid", "תמיד"},
                { "Middoth", "מדות"},
                { "Mishna Kinnim", "קינים"},
                { "Kelim", "כלים"},
                { "Mishnah Ohalot", "אהלות"},
                { "N’gaʿim", "נגעים"},
                { "Parah", "פרה"},
                { "Mishna Tahorot", "טהרות"},
                { "Mishna Mikvaot", "מקואות"},
                { "Niddah", "נדה"},
                { "Makhshirin", "מכשירין"},
                { "Zavim", "זבים"},
                { "Mishnah Tevul Yom", "טבול יום"},
                { "Yadayim", "ידים"},
                { "Mishnah Oktzin", "עוקצים"}
        };
        }

        public async Task FetchAndSaveAllBooks()
        {
            // Fetch and save Tanach books
            foreach (var (englishName, hebrewName) in TanachBooks)
            {
                await FetchAndSaveBook(englishName, hebrewName, (BookTypeEnum)(englishName switch
                {
                    var name when TanachBooks.Keys.Take(5).Contains(name) => 0,
                    var name when TanachBooks.Keys.Skip(5).Take(21).Contains(name) => 1,
                    _ => 2
                }), TanachBooks.Keys.ToList().IndexOf(englishName) + 1);
            }

            // Fetch and save Mishna tractates
            foreach (var (englishName, hebrewName) in MishnaTractates)
            {
                await FetchAndSaveBook(englishName, hebrewName, BookTypeEnum.Mishna, MishnaTractates.Keys.ToList().IndexOf(englishName) + 1);
            }
        }

        private async Task FetchAndSaveBook(string bookName, string heBookName, BookTypeEnum bookType, int bookOrder)
        {
            var url = $"https://www.sefaria.org/api/v3/texts/{bookName}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonResponse);

                // Check if "versions" array exists and has elements
                if (jsonDoc.RootElement.TryGetProperty("versions", out JsonElement versionsElement) && versionsElement.GetArrayLength() > 0)
                {
                    var firstVersion = versionsElement[0];

                    // Extract "text" property from the first version
                    if (firstVersion.TryGetProperty("text", out JsonElement textElement) && textElement.ValueKind == JsonValueKind.Array)
                    {
                        var book = new Book
                        {
                            BookName = bookName,
                            HebrewBookName = heBookName,
                            BookType = bookType,
                            HasSubSubject = false, // Update this as needed
                            BookOrder = bookOrder
                        };
                        Cx.Books.Add(book);
                        await Cx.SaveChangesAsync();

                        var chapters = textElement.EnumerateArray();
                        int chapterNumber = 1;

                        foreach (var chapter in chapters)
                        {
                            var subject = new Subject
                            {
                                SubjectName = $"{heBookName} פרק {chapterNumber}",
                                Ordinal = chapterNumber,
                                BookId = book.BookId,
                                SubjectText = new SubjectText
                                {
                                    SubjectTextContent = chapter.ToString()
                                }
                            };

                            Cx.Subjects.Add(subject);
                            chapterNumber++;
                        }
                        await Cx.SaveChangesAsync();
                    }
                }
            }
        }
    }
}