using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht_8
{
    public class TriviaData
    {
        public int response_code { get; set; }
        public List<TriviaQuestion> results { get; set; }
    }



    public class TriviaQuestion
    {
        public string type { get; set; }
        public string difficulty { get; set; }
        public string category { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
        public bool HasAnswered { get; set; }
        private List<string> randomizedAnswers;

        public List<string> RandomizedAnswers
        {
            get
            {
                if (randomizedAnswers == null)
                {
                    randomizedAnswers = GetRandomizedAnswers();
                }
                return randomizedAnswers;
            }
        }

        private List<string> GetRandomizedAnswers()
        {
            List<string> allAnswers = new List<string>();
            if (incorrect_answers != null)
            {
                allAnswers.AddRange(incorrect_answers);
            }
            allAnswers.Add(correct_answer);


            Random rng = new Random();
            int n = allAnswers.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = allAnswers[k];
                allAnswers[k] = allAnswers[n];
                allAnswers[n] = value;
            }

            return allAnswers;
        }
    }


}
