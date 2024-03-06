using KeyboardDefense.Prompts;

namespace KeyboardDefense.Services
{
    public interface IWordDatabase
    {
        PromptData GetRandomWordOfGivenDifficulty(WordDifficulty difficulty);
    }
}