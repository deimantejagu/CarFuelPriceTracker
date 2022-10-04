using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

class PriceScanner
{
    static void Main(string[] args)
    {
        // Source code for API from mindee.com:
        var url = "https://api.mindee.net/v1/products/vrapas/test/v1/predict";

        // Gets list of all images in your given path:
        string[] files = Directory.GetFiles(@"C:\Users\martynas\Desktop\prices", "*.jpg");
        File.WriteAllText(@"C:\Users\martynas\Desktop\output.txt", "");
        File.WriteAllText(@"C:\Users\martynas\Desktop\prices.txt", "");
        File.WriteAllText(@"C:\Users\martynas\Desktop\rawPrices.txt", "");

        // Loops through each image
        foreach (string image in files)
        {
            Console.WriteLine(image);
            var filePath = image;

            // Change the API key (token) with your own? (Probably not necessary)
            var token = "eb22ad7dad7171577372b9b4c93b1c35";

            var file = File.OpenRead(filePath);
            var streamContent = new StreamContent(file);
            var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            var form = new MultipartFormDataContent();
            form.Add(imageContent, "document", Path.GetFileName(filePath));

            var httpClient = new HttpClient();
            // If you changed the API key (token) before, change it here also (skip if you didn't change)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            var response = httpClient.PostAsync(url, form).Result;
            // Change the path with your own: 
            File.WriteAllText(@"C:\Users\martynas\Desktop\output.txt", response.Content.ReadAsStringAsync().Result);

            // Following program removes unnecessary things from output,
            // leaving the prices only. It then prints them to your chosen text file
            int location = 0;
            List<string> prices = new List<string>();
            List<string> words = new List<string>();
            string oneWord = "";
            // Change the path with your own:
            string readText = File.ReadAllText(@"C:\Users\martynas\Desktop\output.txt");

            // Collects all "words" into a list
            for (int i = 0; i < readText.Length; i++)
            {
                if ((readText[i] != ' ') && (i < readText.Length - 1))
                {
                    oneWord = oneWord + readText[i];
                }
                else
                {
                    words.Add(oneWord);
                    oneWord = "";
                }
            }

            // Since the prices in the output are always after the word "content",
            // this part looks for the word "content" and gets the price that follows it
            for (int i = 0; i < words.Count; i++)
            {
                // Removes unecessary symbols
                words[i] = Regex.Replace(words[i], @"(\s+|@|&|'|\(|\)|<|>|#|,|{|}|\[|\]|:)", "");
                if (words[i] == "\"content\"")
                {
                    prices.Add(words[i + 1]);
                    location++;
                }
            }

            // The prices in the output duplicate (the API scans the same text twice).
            // Therefore, only unique price values are left.
            // However, issues might arise when two different fuel types have the same price.
            // Changes will have to be made if that happens
            prices = prices.Distinct().ToList();

            // Outputs raw prices: 
            System.IO.File.AppendAllLines(@"C:\Users\martynas\Desktop\rawPrices.txt", prices);

            // Formats the prices:
            var count = prices.Count;
            for (int i = 0; i < count; i++)
            {
                int digitCount = 0;
                bool hasDigitalSeparator = false;
                for (int j = 0; j < prices[i].Length; j++)
                {
                    if ((prices[i][j] > 47) && (prices[i][j] < 58))
                    {
                        digitCount++;
                        if (digitCount > 4)
                        {
                            prices[i] = "";
                        }
                    }
                    else if (prices[i][j] == '.')
                    {
                        hasDigitalSeparator = true;
                    }
                }

                if ((hasDigitalSeparator == false) && (digitCount == 4))
                {
                    prices[i] = prices[i].Insert(2, ".");
                }
                else if (digitCount < 3)
                {
                    prices[i] = "";
                }
            }

            // Change the path with your own:
            System.IO.File.AppendAllLines(@"C:\Users\martynas\Desktop\prices.txt", prices);
        }
    }
}