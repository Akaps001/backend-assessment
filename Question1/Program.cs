Console.WriteLine(ScoreCalculator.Calculate([1, 2, 3, 4, 5])); // 11
Console.WriteLine(ScoreCalculator.Calculate([15, 25, 35]));    // 9
Console.WriteLine(ScoreCalculator.Calculate([8, 8]));          // 12

public static class ScoreCalculator
{
    private const int EvenPoints = 1;
    private const int OddPoints = 3;
    private const int EightBonusPoints = 5;

    public static int Calculate(int[] numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        var score = 0;

        foreach (var number in numbers)
        {
            score += number % 2 == 0 ? EvenPoints : OddPoints;

            if (number == 8)
            {
                score += EightBonusPoints;
            }
        }

        return score;
    }
}
