using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace Opdracht_8
{
    public partial class VragenPagina : ContentPage
    {
        public VragenPagina()
        {
            InitializeComponent();
            GetTriviaData();
        }

        private async void GetTriviaData()
        {
            HttpClient client = new HttpClient();
            string apiUrl = "https://opentdb.com/api.php?amount=10&type=multiple";

            try
            {

                string json = await client.GetStringAsync(apiUrl);
                TriviaData triviaData = JsonConvert.DeserializeObject<TriviaData>(json);

                foreach (var question in triviaData.results)
                {
                    string decodedQuestion = question.question.Replace("&quot;", "\"");
                    TriviaLayout.Children.Add(new Label { Text = decodedQuestion, FontSize = 18, Margin = new Thickness(20) });
                    
                    foreach (var answer in question.RandomizedAnswers)
                    {
                        var button = new Button
                        {
                            Text = answer,
                            Margin = new Thickness(0, 0, 0, 10),
                            WidthRequest = 200,
                            BorderColor = Color.FromRgb(44, 196, 196),
                            TextColor = Color.FromRgb(0, 0, 0)
                        };
                        button.Clicked += (s, e) => OnAnswerClicked(s, e, question, answer);
                        TriviaLayout.Children.Add(button);
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void OnAnswerClicked(object sender, EventArgs e, TriviaQuestion question, string selectedAnswer)
        {
            if (question.HasAnswered)
            {
                return;
            }

            Button clickedButton = (Button)sender;
            if (selectedAnswer == question.correct_answer)
            {
                // Als het antwoord correct is, zet de achtergrondkleur op groen
                clickedButton.BackgroundColor = Color.FromRgb(0, 255, 0);
            }
            else
            {
                // Als het antwoord onjuist is, zet de achtergrondkleur op rood
                clickedButton.BackgroundColor = Color.FromRgb(255, 0, 0);

                // Zoek de knop met het juiste antwoord en zet de achtergrondkleur op groen
                foreach (var view in TriviaLayout.Children)
                {
                    if (view is Button button && button.Text == question.correct_answer)
                    {
                        button.BackgroundColor = Color.FromRgb(0, 255, 0);
                        break;
                    }
                }
            }

            question.HasAnswered = true;
        }

    }
}
