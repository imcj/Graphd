namespace Graphd.NameStrategy;

internal class CamcelCaseNameStrategy : INameStrategy
{
    public string Convert(string name)
    {
        string[] words = name.Split('_');
        for (int i = 1; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
        }

        // 将单词合并为一个字符串
        string camelCase = string.Join("", words);

        return camelCase;
    }
}
