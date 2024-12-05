namespace HangmanAssignment;

public partial class HangmanGamePage : ContentPage
{
    private string targetWord;
    private string maskedWord;
    private int incorrectGuesses;
    private List<char> correctGuesses;
    private readonly string[] hangmanStages =
    {
        "hang1.png", "hang2.png", "hang3.png", "hang4.png",
        "hang5.png", "hang6.png", "hang7.png", "hang8.png"
    };
    private readonly string[] possibleWords = { "APPLE", "BANANA", "EXPERIENCE", "LAB", "VANESSA", "HANGMAN", "LIMPOPO" };

    public HangmanGamePage()
    {
        InitializeComponent();
        StartGame();
    }

    private void StartGame()
    {
        // Select a random word
        Random random = new Random();
        targetWord = possibleWords[random.Next(possibleWords.Length)].ToUpper();

        // Prepare the masked word
        incorrectGuesses = 0;
        correctGuesses = new List<char>();
        maskedWord = new string('_', targetWord.Length);

        // Update UI for the new game
        WordToGuessLabel.Text = maskedWord;
        HangmanImage.Source = hangmanStages[incorrectGuesses];
        ResultLabel.Text = string.Empty;
    }

    private void OnGuessClicked(object sender, EventArgs e)
    {
        string input = GuessEntry.Text?.ToUpper();
        if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
        {
            ResultLabel.Text = "Please enter a single valid letter.";
            return;
        }

        char guessedLetter = input[0];

        // Check if the letter has already been guessed
        if (correctGuesses.Contains(guessedLetter) || maskedWord.Contains(guessedLetter))
        {
            ResultLabel.Text = "You already guessed that letter.";
            return;
        }

        // Determine if the guess is correct
        if (targetWord.Contains(guessedLetter))
        {
            correctGuesses.Add(guessedLetter);
            UpdateMaskedWord(guessedLetter);
        }
        else
        {
            incorrectGuesses++;
            HangmanImage.Source = hangmanStages[incorrectGuesses];
        }

        // Check if the game has ended
        if (incorrectGuesses >= hangmanStages.Length - 1)
        {
            ResultLabel.Text = $" Player Died! The correct word was: {targetWord}";
            return;
        }

        if (!maskedWord.Contains('_'))
        {
            ResultLabel.Text = "Player Survived! You guessed the word!";
            return;
        }

        // Clear input field for the next guess
        GuessEntry.Text = string.Empty;
    }

    private void UpdateMaskedWord(char guessedLetter)
    {
        char[] maskedArray = maskedWord.ToCharArray();

        for (int i = 0; i < targetWord.Length; i++)
        {
            if (targetWord[i] == guessedLetter)
            {
                maskedArray[i] = guessedLetter;
            }
        }

        maskedWord = new string(maskedArray);
        WordToGuessLabel.Text = maskedWord;
    }
}