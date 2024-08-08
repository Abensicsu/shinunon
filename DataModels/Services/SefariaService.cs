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

        public List<string> TanachBooks { get; private set; }
        public List<string> MishnaTractates { get; private set; }
        public List<string> TanachBooksHebrew { get; private set; }
        public List<string> MishnaTractatesHebrew { get; private set; }
        private Dictionary<string, string> BookNameMapping { get; set; }

        public SefariaService(SHcx cx)
        {
            Cx = cx;
            _httpClient = new HttpClient();

            // Initialize lists
            TanachBooks = new List<string>
            {
                "Bereshit", "Shemot", "Vayikra", "Bamidbar", "Devarim",
                "Yehoshua", "Shoftim", "I Samuel", "II Samuel", "I Kings",
                "II Kings", "Yeshayahu", "Yirmeyahu", "Yechezkel", "Hoshea",
                "Yoel", "Amos", "Ovadiah", "Yonah", "Micha", "Nachum",
                "Habakkuk", "Zephaniah", "Haggai", "Zechariah", "Malachi",
                "Tehillim", "Mishlei", "Iyov", "Shir HaShirim", "Rut",
                "Eichah", "Kohelet", "Ester", "Daniel", "Ezra", "Nehemiah",
                "I Chronicles", "II Chronicles"
            };

            MishnaTractates = new List<string>
            {
                "Berakhot", "Peah", "D’mai", "Kilayim", "Sheviit", "Terumot",
                "Maʿasroth", "Maaser Sheni", "Hallah", "Mishnah Orlah", "Bikkurim",
                "Shabbat", "Eruvin", "Pesachim", "Mishnah Shekalim", "Yoma", "Sukkah",
                "Beitzah", "Rosh Hashanah", "Ta'anit", "Megillah", "Moed Katan",
                "Chagigah", "Yevamot", "Ketubot", "Nedarim", "Nazir", "Sotah",
                "Gittin", "Kiddushin", "Bava Kamma", "Bava Metzia", "Bava Batra",
                "Sanhedrin", "Makkot", "Shevuot", "Eduyot", "Avodah Zarah",
                "Avot", "Horayot", "Zevachim", "Menachot", "Chullin", "Bekhorot",
                "Arakhin", "Temurah", "Keritot", "Meilah", "Tamid", "Middoth",
                "Mishna Kinnim", "Kelim", "Mishnah Ohalot", "N’gaʿim", "Parah", "Mishna Tahorot",
                "Mishna Mikvaot", "Niddah", "Makhshirin", "Zavim", "Mishnah Tevul Yom", "Yadayim",
                "Mishnah Oktzin"
            };

            BookNameMapping = new Dictionary<string, string>
            {
                {"Bereshit", "בראשית"},
                {"Shemot", "שמות"},
                {"Vayikra", "ויקרא"},
                {"Bamidbar", "במדבר"},
                {"Devarim", "דברים"},
                {"Yehoshua", "יהושע"},
                {"Shoftim", "שופטים"},
                {"I Samuel", "שמואל א"},
                {"II Samuel", "שמואל ב"},
                {"I Kings", "מלכים א"},
                {"II Kings", "מלכים ב"},
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
                {"I Chronicles", "דברי הימים א"},
                {"II Chronicles", "דברי הימים ב"},

                // Mappings for Mishna tractates
                {"Berakhot", "ברכות"},
                {"Peah", "פאה"},
                {"D’mai", "דמאי"},
                {"Kilayim", "כלאים"},
                {"Sheviit", "שביעית"},
                {"Terumot", "תרומות"},
                {"Maʿasroth", "מעשרות"},
                {"Maaser Sheni", "מעשר שני"},
                {"Hallah", "חלה"},
                {"Mishnah Orlah", "מִשְׁנָה ערלה"},
                {"Bikkurim", "ביקורים"},
                {"Shabbat", "שבת"},
                {"Eruvin", "עירובין"},
                {"Pesachim", "פסחים"},
                {"Mishnah Shekalim", "מִשְׁנָה שקלים"},
                {"Yoma", "יומא"},
                {"Sukkah", "סוכה"},
                {"Beitzah", "ביצה"},
                {"Rosh Hashanah", "ראש השנה"},
                {"Ta'anit", "תענית"},
                {"Megillah", "מגילה"},
                {"Moed Katan", "מועד קטן"},
                {"Chagigah", "חגיגה"},
                {"Yevamot", "יבמות"},
                {"Ketubot", "כתובות"},
                {"Nedarim", "נדרים"},
                {"Nazir", "נזיר"},
                {"Sotah", "סוטה"},
                {"Gittin", "גטין"},
                {"Kiddushin", "קידושין"},
                {"Bava Kamma", "בבא קמא"},
                {"Bava Metzia", "בבא מציעא"},
                {"Bava Batra", "בבא בתרא"},
                {"Sanhedrin", "סנהדרין"},
                {"Makkot", "מכות"},
                {"Shevuot", "שבועות"},
                {"Eduyot", "עדויות"},
                {"Avodah Zarah", "עבודה זרה"},
                {"Avot", "אבות"},
                {"Horayot", "הוריות"},
                {"Zevachim", "זבחים"},
                {"Menachot", "מנחות"},
                {"Chullin", "חולין"},
                {"Bekhorot", "בכורות"},
                {"Arakhin", "ערכין"},
                {"Temurah", "תמורה"},
                {"Keritot", "כריתות"},
                {"Meilah", "מעילה"},
                {"Tamid", "תמיד"},
                {"Middoth", "מדות"},
                {"Mishna Kinnim", "קינים"},
                {"Kelim", "כלים"},
                {"Mishnah Ohalot", "אהלות"},
                {"N’gaʿim", "נגעים"},
                {"Parah", "פרה"},
                {"Mishna Tahorot", "טהרות"},
                {"Mishna Mikvaot", "מקואות"},
                {"Niddah", "נדה"},
                {"Makhshirin", "מכשירין"},
                {"Zavim", "זבים"},
                {"Mishnah Tevul Yom", "טבול יום"},
                {"Yadayim", "ידים"},
                {"Mishnah Oktzin", "עוקצים"}
            };
        }

        public async Task FetchAndSaveAllBooks()
        {
            // Fetch and save Tanach books
            for (int i = 0; i < TanachBooks.Count; i++)
            {
                await FetchAndSaveBook(TanachBooks[i], (BookTypeEnum)((i < 5) ? 0 : ((i < 26) ? 1 : 2)), i + 1);
            }

            // Fetch and save Mishna tractates
            for (int i = 0; i < MishnaTractates.Count; i++)
            {
                await FetchAndSaveBook(MishnaTractates[i], BookTypeEnum.Mishna, i + 1);
            }
        }

        private async Task FetchAndSaveBook(string bookName, BookTypeEnum bookType, int bookOrder)
        {
            // Get the Hebrew book name
            var heBookName = BookNameMapping.ContainsKey(bookName) ? BookNameMapping[bookName] : bookName;

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
                            BookName = heBookName,
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