using System.Text;
using static System.Console;
const double sideProb = 0.2;
const double downProb = 0.2;
const double upProb = 0;
const double contProb = 0.4;
const double sideDownProb = sideProb * downProb;
const double sideUpProb = sideProb * upProb;
const ConsoleColor textColor = ConsoleColor.Yellow;
const ConsoleColor fireColor0 = ConsoleColor.DarkYellow;
const ConsoleColor fireColor1 = ConsoleColor.Red;
int GetWindowHeight() => WindowHeight;
int GetWindowWidth() => WindowWidth - 1;
Title = "Fireplace";
string[] lines = Fireplace.Resource1.Image.Split("\r\n");
Random random = new();
bool[,] fire = new bool[GetWindowHeight(), GetWindowWidth()];
StringBuilder builder = new();
while (true)
{
    for (int i = 0; i < fire.GetLength(1); i++)
        fire[0, i] = true;
    bool[,] newFire = new bool[fire.GetLength(0), fire.GetLength(1)];
    for (int i = 1; i < fire.GetLength(0); i++)
        for (int j = 0; j < fire.GetLength(1); j++)
        {
            bool left = j > 0;
            bool right = j < fire.GetUpperBound(1);
            bool down = i > 0;
            bool up = i < fire.GetUpperBound(0);
            double notProb = 1;
            if (left && fire[i, j - 1])
                notProb *= 1 - sideProb;
            if (left && down && fire[i - 1, j - 1])
                notProb *= 1 - sideDownProb;
            if (down && fire[i - 1, j])
                notProb *= 1 - downProb;
            if (down && right && fire[i - 1, j + 1])
                notProb *= 1 - sideDownProb;
            if (right && fire[i, j + 1])
                notProb *= 1 - sideProb;
            if (right && up && fire[i + 1, j + 1])
                notProb *= 1 - sideUpProb;
            if (up && fire[i + 1, j])
                notProb *= 1 - upProb;
            if (up && left && fire[i + 1, j - 1])
                notProb *= 1 - sideUpProb;
            if (fire[i, j])
                notProb *= 1 - contProb;
            double prob = 1 - notProb;
            newFire[i, j] = random.NextDouble() < prob;
        }
    fire = newFire;
    for (int i = 0; i < fire.GetLength(0) - 1; i++)
    {
        for (int j = 0; j < fire.GetLength(1); j++)
            if (fire[fire.GetUpperBound(0) - i, j])
            {
                ConsoleColor color = (random.NextDouble() > 0.2) ? fireColor0 : fireColor1;
                if (color != ForegroundColor)
                {
                    Write(builder.ToString());
                    builder.Clear();
                    ForegroundColor = color;
                }
                builder.Append('#');
            }
            else if (i < lines.Length && j < lines[i].Length)
            {
                if (textColor != ForegroundColor)
                {
                    Write(builder.ToString());
                    builder.Clear();
                    ForegroundColor = textColor;
                }
                builder.Append(lines[i][j]);
            }
            else
                builder.Append(' ');
        if (i < fire.GetUpperBound(0))
            builder.AppendLine();
    }
    Write(builder.ToString());
    builder.Clear();
    CursorTop = 0;
    CursorLeft = 0;
    _ = builder.Clear();
    await Task.Delay(50);
    int xL = GetWindowHeight();
    int yL = GetWindowWidth();
    if (fire.GetLength(0) != xL || fire.GetLength(1) != yL)
    {
        Clear();
        newFire = new bool[xL, yL];
        int xC = Math.Min(fire.GetLength(0), xL);
        int yC = Math.Min(fire.GetLength(1), yL);
        for (int i = 0; i < xC; i++)
            for (int j = 0; j < yC; j++)
                newFire[i, j] = fire[i, j];
        fire = newFire;
    }
}