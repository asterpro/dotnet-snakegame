// This enum contains a list of the possible directions.
enum Direction
{
    up,
    down,
    left,
    right
};

// This class is to be used to store co-ordinates, size or anything related to positioning.
class Vector
{
    public int X; // X co-ordinate/value
    public int Y; // Y co-ordinate/value

    // Vector class constructer when the co-ordinates are given.
    public Vector(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    // Vector class constructor when another Vector instance is given.
    public Vector(Vector anotherVector)
    {
        this.X = anotherVector.X;
        this.Y = anotherVector.Y;
    }

    // Operator overloading to compare if two Vector instance are equal.
    public static bool operator ==(Vector A, Vector B)
    {
        if (A.X == B.X && A.Y == B.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator !=(Vector A, Vector B)
    {
        if (A.X == B.X && A.Y == B.Y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}

// This class contains the properties of snake.
class Snake
{
    // The object "headPosition" stores the X and Y co-ordinates of the head of the snake.
    public Vector headPosition = new Vector(15, 15);

    public int snakeSize = 5;

    public Direction direction = Direction.right; // Stores the direction of the snake.

    public List<Vector> snakeBody = new List<Vector>(); // Stores the X and Y co-ordinates of all the parts of the snake's body.

    public Snake()
    {
        for (int i = 1; i <= snakeSize; i++)
        {
            Vector temp = new Vector(headPosition.X - i, headPosition.Y);
            snakeBody.Add(temp);
        }
    }

}


// This class contains the properties of food.
class Food
{
    public Vector foodPos;

    public Food()
    {
        Random randomGen = new Random();
        int X = randomGen.Next(1, 29);
        int Y = randomGen.Next(1, 29);
        foodPos = new Vector(X, Y);
    }
}


// This class is the game itself.
class Game
{
    static Snake player = new Snake(); // This instance of the class Snake is the player itself.
    static Food food; // This instance of the class Food contains the position of the food.
    static int refreshRate = 500; // This variable is the refresh rate of the game in millisecond. Note: 1 second = 1000 millisecond.
    static int score = 0; // This variable stores the player's current score.
    static bool gameOver = false; // This boolean stores whether the game is over or not.

    // The Main method, mostly used for calling other functions and thread activities.
    public static void Main()
    {
        Console.CursorVisible = false; // This statement gets rid of the input cursor thing.

        constructNewFood(); // This sets the food's position.

        Thread IO = new Thread(new ThreadStart(Controls)); // This thread is responsible for recieving and managing the game's controls.
        IO.Start(); // Starts the IO thread.

        while (true) // A traditional while loop.
        {
            drawBoard(); // Draws the UI.
            playerMovement(); // Updates the snake's position.
            checkGameOver(); // Checks if the game is over.
            if (gameOver == true)
            {
                break; // The while loop ends if the game is over.
            }
            Thread.Sleep(refreshRate); // The thread pauses for "refreshRate" milliseconds.
        }

        // This part of the code will only run after the game is over, and will notify that to the player.
        Console.SetCursorPosition(10, 32);
        Console.WriteLine("Game Over.");
    }

    // This function manages the controls and is to be run in a different thread.
    public static void Controls()
    {
        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey(true); // This accepts the controls.

            // A switch case for setting the directions based on the input controls.
            switch (input.Key)
            {
                case ConsoleKey.W:
                    if (player.headPosition.Y - player.snakeBody[0].Y != 1)
                    {
                        player.direction = Direction.up;
                    }
                    break;
                case ConsoleKey.S:
                    if (player.headPosition.Y - player.snakeBody[0].Y != -1)
                    {
                        player.direction = Direction.down;
                    }
                    break;
                case ConsoleKey.A:
                    if (player.headPosition.X - player.snakeBody[0].X != 1)
                    {
                        player.direction = Direction.left;
                    }
                    break;
                case ConsoleKey.D:
                    if (player.headPosition.X - player.snakeBody[0].X != -1)
                    {
                        player.direction = Direction.right;
                    }
                    break;
                case ConsoleKey.Escape:
                    gameOver = true;
                    break;
            }
        }
    }

    // This function draws the board, with the snake, food, scores, etc.
    public static void drawBoard()
    {
        Vector boardSize = new Vector(30, 30); // This stores the size of the board.

        Console.Clear(); // Clears the screen to avoid any visual glitches.

        // Draws the board's outline.
        Console.SetCursorPosition(0, 0);
        for (int i = 0; i < boardSize.X; i++)
        {
            Console.Write("_");
        }
        for (int i = 1; i < boardSize.Y; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("|");
            Console.SetCursorPosition(boardSize.X - 1, i);
            Console.Write("|");
        }
        Console.SetCursorPosition(0, boardSize.Y);
        Console.Write("|");
        for (int i = 1; i < boardSize.X - 1; i++)
        {
            Console.Write("_");
        }
        Console.Write("|");

        // This part of the code prints the food.
        Console.SetCursorPosition(food.foodPos.X, food.foodPos.Y);
        Console.Write("F");

        // This part of the code prints the snake's body, excluding the head.
        List<Vector> tempList = player.snakeBody.ToList<Vector>();
        for (int i = 0; i < tempList.Count(); i++)
        {
            Console.SetCursorPosition(tempList[i].X, tempList[i].Y);
            Console.Write("O");
        }

        // This part of the code prints the snake's head.
        Console.SetCursorPosition(player.headPosition.X, player.headPosition.Y);
        Console.Write("@");

        // This part of the code displays the current score below the board.
        Console.SetCursorPosition(0, boardSize.Y + 1);
        Console.Write("Your score: " + score);
    }

    // This function updates the player's position and stuffs.
    public static void playerMovement()
    {
        Vector formerHeadPosition = new Vector(player.headPosition); // Stores the current position of snake's head as it will be updated afterwards.

        // The following switch case statements updates the position of the snake's head.
        switch (player.direction)
        {
            case Direction.up:
                player.headPosition.Y--;
                break;
            case Direction.down:
                player.headPosition.Y++;
                break;
            case Direction.left:
                player.headPosition.X--;
                break;
            case Direction.right:
                player.headPosition.X++;
                break;
        }

        // This part of the code makes sure the snake doesn't move out of the board.
        if (player.headPosition.X <= 0 || player.headPosition.X >= 29)
        {
            player.headPosition.X = Math.Abs(player.headPosition.X - 28);
        }
        if (player.headPosition.Y <= 0 || player.headPosition.Y >= 30)
        {
            player.headPosition.Y = Math.Abs(player.headPosition.Y - 29);
        }


        // The following line of code updates the snake's body position.
        Vector[] formerSnakeBody = player.snakeBody.ToArray();
        player.snakeBody.Clear();
        player.snakeBody.Add(formerHeadPosition);
        for (int i = 0; i < player.snakeSize - 1; i++)
        {
            Vector tempVector = new Vector(formerSnakeBody[i]);
            player.snakeBody.Add(tempVector);
        }

        updateScore(); // Updates the current score if food is eaten.
    }

    // This function checks if the game is over.
    // Note: The game is only over if the snake's head touch one of its body parts.
    public static void checkGameOver()
    {
        foreach (Vector temp in player.snakeBody)
        {
            if (temp == player.headPosition)
            {
                gameOver = true;
            }
        }
    }

    // This function produces new position for the food
    public static void constructNewFood()
    {
    redo:
        Food tempFood = new Food();
        if (tempFood.foodPos == player.headPosition)
        {
            goto redo;
        }
        foreach (Vector tempVector in player.snakeBody)
        {
            if (tempFood.foodPos == tempVector)
            {
                goto redo;
            }
        }

        food = tempFood;
    }

    // This function updates the score if food is eaten.
    public static void updateScore()
    {
        if (food.foodPos == player.headPosition)
        {
            score++; // Updates the score.
            player.snakeSize++; // Increases the snake's size.
            constructNewFood(); // Constructs a new food, as the player just ate the current one.
        }
    }
}
