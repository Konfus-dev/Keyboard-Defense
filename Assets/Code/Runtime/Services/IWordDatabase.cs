using KeyboardDefense.Prompts;

namespace KeyboardDefense.Services
{
    public interface IWordDatabase : IGameService
    {
        PromptData GetRandomWordOfGivenDifficulty(WordDifficulty difficulty);
    }
}