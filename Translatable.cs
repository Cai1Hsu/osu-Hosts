using System.Diagnostics;

namespace osuHosts;

public class LanguageSpecificString
{
    private string _value = null!;
    public string Value
    {
        get => _value;
        set
        {
            if (Value == value) return;
            
            // Check value is not null or empty
            if (string.IsNullOrEmpty(value)) return;
            
            _value = value;
            
            // Update Translations
            UpdateTranslations();
        }
    }

    public void UpdateTranslations()
    {
        if (Translations == null || Value == null) return;
            
        if (Translations.ContainsKey(this.Language))
        {
            Translations[this.Language] = Value;
        }
        else
        {
            Translations.Add(this.Language, Value);
        }
    }

    public void SetTranslate(string translate) => this.Value = translate;
    
    public LanguageSpecificString(string value)
    {
        Value = value;
    }
    
    public LanguageSpecificString()
    {
        Value = String.Empty;
    }

    /// <summary>
    /// Do not access this. You should access TranslatableString.Translations instead.
    /// </summary>
    public Dictionary<Languages, string> Translations { get; set; } = null!;

    public Languages Language { get; set; }
}

public class TranslatableString
{
    public LanguageSpecificString English { get; set; } = null!;

    public LanguageSpecificString SChinese { get; set; } = null!;
    
    /// <summary>
    /// Do not change this!
    /// </summary>
    public Dictionary<Languages, string> Translations { get; set; } = new();

    public string GetTranslated()
    {
        var targetLanguage = TranslationAssets.SystemLanguage;

        if (!Translations.ContainsKey(targetLanguage)) return Translations[Languages.English];
        
        return this.Translations[targetLanguage];
    }

    public override string ToString()
    {
        return GetTranslated();
    }
}

public enum Languages
{
    English,
    SChinese,
}