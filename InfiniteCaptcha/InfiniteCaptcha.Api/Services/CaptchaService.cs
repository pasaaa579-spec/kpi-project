using InfiniteCaptcha.Shared.Models;
using System.Collections.Concurrent;

namespace InfiniteCaptcha.Api.Services
{
    public class CaptchaService : ICaptchaService
    {
        private static readonly ConcurrentDictionary<Guid, string> _answers = new();
        private readonly Random _random = new();

        public CaptchaChallengeDto GenerateChallenge(int level)
        {
            var challengeId = Guid.NewGuid();
            string question = "";
            string correctAnswer = "";

            int maxCategory = Math.Min(6, (level / 3) + 1);
            int category = _random.Next(1, maxCategory + 1);

            switch (category)
            {
                case 1:
                    if (level < 5)
                    {
                        int a = _random.Next(1, 10 + level * 4);
                        int b = _random.Next(1, 10 + level * 4);
                        if (_random.NextDouble() > 0.5)
                        {
                            question = $"{a} + {b} = ?";
                            correctAnswer = (a + b).ToString();
                        }
                        else
                        {
                            int max = Math.Max(a, b);
                            int min = Math.Min(a, b);
                            question = $"{max} - {min} = ?";
                            correctAnswer = (max - min).ToString();
                        }
                    }
                    else if (level < 12)
                    {
                        int a = _random.Next(2, 10 + (level - 5));
                        int b = _random.Next(2, 12);
                        question = $"{a} * {b} = ?";
                        correctAnswer = (a * b).ToString();
                    }
                    else
                    {
                        int a = _random.Next(10, 50 + level);
                        int b = _random.Next(10, 50 + level);
                        int c = _random.Next(5, 20 + level);
                        question = $"{a} + {b} - {c} = ?";
                        correctAnswer = (a + b - c).ToString();
                    }
                    break;

                case 2:
                    string[,] easyIt = {
                        { "HTTP статус 'Not Found'?", "404" },
                        { "Скільки біт в 1 байті?", "8" },
                        { "Стандартний порт HTTP?", "80" }
                    };
                    string[,] medIt = {
                        { "HTTP статус 'OK'?", "200" },
                        { "Стандартний порт HTTPS?", "443" },
                        { "2 в 10 ступені?", "1024" },
                        { "Головна парадигма C# (три літери)?", "oop" },
                        { "Який фреймворк ми юзаємо для БД (.NET)?", "ef" }
                    };
                    string[,] hardIt = {
                        { "Який тип даних в C# використовують для точних фінансових розрахунків?", "decimal" },
                        { "Структура даних, що працює за принципом LIFO (5 літер)?", "stack" },
                        { "Базовий клас для всіх типів у .NET (6 літер)?", "object" },
                        { "Який оператор LINQ використовують для фільтрації?", "where" },
                        { "HTTP метод для повного оновлення ресурсу (3 літери)?", "put" }
                    };

                    string[,] selectedIt = level switch
                    {
                        < 6 => easyIt,
                        < 13 => medIt,
                        _ => hardIt
                    };

                    int qIndex = _random.Next(selectedIt.GetLength(0));
                    question = selectedIt[qIndex, 0];
                    correctAnswer = selectedIt[qIndex, 1];
                    break;

                case 3:
                    string[] easyWords = { "kpi", "bug", "git", "api", "url" };
                    string[] medWords = { "code", "fpm", "hash", "null", "push", "stack", "class" };
                    string[] hardWords = { "framework", "asynchronous", "polymorphism", "encapsulation", "interface" };

                    string[] selectedWords = level switch
                    {
                        < 6 => easyWords,
                        < 14 => medWords,
                        _ => hardWords
                    };

                    string word = selectedWords[_random.Next(selectedWords.Length)];
                    question = $"Напиши задом наперед: {word}";

                    char[] charArray = word.ToCharArray();
                    Array.Reverse(charArray);
                    correctAnswer = new string(charArray).ToLower();
                    break;

                case 4:
                    int dec = _random.Next(6 + level, 15 + level * 4);
                    if (_random.NextDouble() > 0.5)
                    {
                        question = $"Переведи з BIN (двійкової) у DEC: {Convert.ToString(dec, 2)}";
                    }
                    else
                    {
                        question = $"Переведи з HEX (шістнадцяткової) у DEC: {Convert.ToString(dec, 16).ToUpper()}";
                    }
                    correctAnswer = dec.ToString();
                    break;

                case 5:
                    int maxMatrixVal = Math.Min(15, 4 + level / 2);
                    int m11 = _random.Next(1, maxMatrixVal), m12 = _random.Next(1, maxMatrixVal);
                    int m21 = _random.Next(1, maxMatrixVal), m22 = _random.Next(1, maxMatrixVal);

                    if (level > 12)
                    {
                        if (_random.NextDouble() > 0.5) m12 = -_random.Next(1, 6);
                        if (_random.NextDouble() > 0.5) m21 = -_random.Next(1, 6);
                    }

                    if (_random.NextDouble() > 0.5)
                    {
                        question = $"Слід матриці (Trace) [[{m11}, {m12}], [{m21}, {m22}]] = ?";
                        correctAnswer = (m11 + m22).ToString();
                    }
                    else
                    {
                        question = $"Визначник (Det) [[{m11}, {m12}], [{m21}, {m22}]] = ?";
                        correctAnswer = ((m11 * m22) - (m12 * m21)).ToString();
                    }
                    break;

                case 6:
                    int coef = _random.Next(2, 5 + level / 4);
                    int power = _random.Next(2, 4 + level / 6);

                    if (_random.NextDouble() > 0.5)
                    {
                        if (level > 14)
                        {
                            int linear = _random.Next(2, 10);
                            question = $"f'(1) для f(x) = {coef}x^{power} + {linear}x ?";
                            correctAnswer = (coef * power + linear).ToString();
                        }
                        else
                        {
                            question = $"f'(1) для f(x) = {coef}x^{power} ?";
                            correctAnswer = (coef * power).ToString();
                        }
                    }
                    else
                    {
                        if (level < 16)
                        {
                            int evenCoef = _random.Next(1, 4 + level / 4) * 2;
                            question = $"Визначений інтеграл від 0 до 1 для f(x) = {evenCoef}x dx ?";
                            correctAnswer = (evenCoef / 2).ToString();
                        }
                        else
                        {
                            int tripleCoef = _random.Next(1, 5) * 3;
                            question = $"Визначений інтеграл від 0 до 1 для f(x) = {tripleCoef}x^2 dx ?";
                            correctAnswer = (tripleCoef / 3).ToString();
                        }
                    }
                    break;

                default:
                    question = "1 + 1 = ?";
                    correctAnswer = "2";
                    break;
            }

            _answers[challengeId] = correctAnswer.ToLower();

            return new CaptchaChallengeDto
            {
                Id = challengeId,
                QuestionText = question,
                DifficultyLevel = level
            };
        }

        public bool VerifyAnswer(Guid challengeId, string answer)
        {
            if (_answers.TryRemove(challengeId, out var correct))
            {
                return correct == answer.Trim().ToLower();
            }
            return false;
        }
    }
}