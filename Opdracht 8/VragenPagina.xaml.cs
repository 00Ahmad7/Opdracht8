using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using System.Net;

namespace Opdracht_8
{
    public partial class VragenPagina : ContentPage
    {
        private double score = 0.0;
        private int answeredQuestions = 0;
        private int totalQuestions;
        private int hardCorrectCount = 0;
        private int mediumCorrectCount = 0;
        private int easyCorrectCount = 0;

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
                totalQuestions = triviaData.results.Count;

                foreach (var question in triviaData.results)
                {
                    string decodedQuestion = WebUtility.HtmlDecode(question.question);
                    TriviaLayout.Children.Add(new Label { Text = decodedQuestion, FontSize = 18, Margin = new Thickness(20) });

                    foreach (var answer in question.RandomizedAnswers)
                    {
                        var decodedAnswer = WebUtility.HtmlDecode(answer);

                        var button = new Button
                        {
                            Text = decodedAnswer,
                            Margin = new Thickness(0, 0, 0, 10),
                            WidthRequest = 200,
                            BorderColor = Color.FromRgb(44, 196, 196),
                            TextColor = Color.FromRgb(0, 0, 0)
                        };
                        button.Clicked += (s, e) => OnAnswerClicked(s, e, question, decodedAnswer);
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
                // Als het antwoord correct is, update de score op basis van de moeilijkheidsgraad
                switch (question.difficulty.ToLower())
                {
                    case "hard":
                        score += 1.0;
                        hardCorrectCount++;
                        break;
                    case "medium":
                        score += 0.5;
                        mediumCorrectCount++;
                        break;
                    case "easy":
                        score += 0.25;
                        easyCorrectCount++;
                        break;
                }

                // Zet de achtergrondkleur op groen
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
            answeredQuestions++;

            // Controleer of alle vragen zijn beantwoord
            if (answeredQuestions == totalQuestions)
            {
                ShowScorePopup();
            }
        }

        private async void ShowScorePopup()
        {
            string explanation = $"Hard: {hardCorrectCount} correct, Medium: {mediumCorrectCount} correct, Easy: {easyCorrectCount} correct";
            await DisplayAlert("Quiz Resultaat", $"Je score is: {score}\n\n{explanation}", "Ok");
            await Navigation.PopAsync();
        }

    }
}
