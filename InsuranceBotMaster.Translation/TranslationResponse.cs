using System.Collections.Generic;

namespace InsuranceBotMaster.Translation
{
    public class TranslationResponse
    {
        public LanguageItem DetectedLanguage { get; set; }
        public List<TranslationItem> Translations { get; set; }
    }

    public class LanguageItem
    {
        public string Language { get; set; }
        public decimal Score { get; set; }
    }

    public class TranslationItem
    {
        public string Text { get; set; }
        public string To { get; set; }
    }
}
