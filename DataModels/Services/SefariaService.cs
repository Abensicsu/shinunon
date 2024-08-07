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
        public List<string> tanachBooksHebrew { get; private set; }
        public List<string> mishnaTractatesHebrew { get; private set; }

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


            tanachBooksHebrew = new List<string>
            {
                "בראשית", "שמות", "ויקרא", "במדבר", "דברים", "יהושע", "שופטים", "שמואל א", "שמואל ב",
                "מלכים א", "מלכים ב", "ישעיהו", "ירמיהו", "יחזקאל", "הושע", "יואל", "עמוס", "עובדיה",
                "יונה", "מיכה", "נחום", "חבקוק", "צפניה", "חגי", "זכריה", "מלאכי", "תהילים", "משלי",
                "איוב", "שיר השירים", "רות", "איכה", "קוהלת", "אסתר", "דניאל", "עזרא", "נחמיה",
                "דברי הימים א", "דברי הימים ב"
            };

            mishnaTractatesHebrew = new List<string>
            {
                "ברכות", "פאה", "דמאי", "כלאים", "שביעית", "תרומות", "מעשרות", "מעשר שני", "חלה",
                "ערלה", "ביכורים", "שבת", "עירובין", "פסחים", "שקלים", "יומא",
                "סוכה", "ביצה", "ראש השנה", "תענית", "מגילה", "מועד קטן", "חגיגה", "יבמות",
                "כתובות", "נדרים", "נזיר", "סוטה", "גיטין", "קידושין", "בבא קמא", "בבא מציעא",
                "בבא בתרא", "סנהדרין", "מכות", "שבועות", "עדויות", "עבודה זרה", "אבות", "הוריות",
                "זבחים", "מנחות", "חולין", "בכורות", "ערכין", "תמורה", "כריתות", "מעילה", "תמיד", "מדות",
                "קינים", "כלים", "אהלות", "נגעים", "פרה", "טהרות",
                "מקואות", "נדה", "מכשירין", "זבים", "טבול יום", "ידים", "עוקצים"
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
                                SubjectName = $"{bookName} Chapter {chapterNumber}",
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