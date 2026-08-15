using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SoNY.Ita
{
    /// <summary>
    /// Adds Italian to the game's language selector.
    ///
    /// Italian was never excluded, only left out: TranslationManager.Languages builds a
    /// hand-written list of four entries and simply omits italianLanguage, which is a
    /// field on the class already. The Language enum already contains IT, and
    /// SetLanguage() composes the identifier as languageTag + genderTag, giving
    /// "it" + "[Female]" = "it[Female]" - exactly the field name the injection writes to.
    ///
    /// Appending one entry to the returned list is therefore enough.
    /// </summary>
    [HarmonyPatch(typeof(TranslationManager), "Languages", MethodType.Getter)]
    internal static class AddItalianLanguage
    {
        private static LanguageSO _italian;

        private static void Postfix(TranslationManager __instance, List<LanguageSO> __result)
        {
            if (__result == null) return;

            foreach (var language in __result)
            {
                if (language != null && language.languageTag == "it") return; // already there
            }

            if (_italian == null) _italian = Build(__instance);
            __result.Add(_italian);
        }

        private static LanguageSO Build(TranslationManager manager)
        {
            // Use the scene's own Italian LanguageSO when it is already configured.
            if (manager != null && manager.italianLanguage != null) return manager.italianLanguage;

            var language = ScriptableObject.CreateInstance<LanguageSO>();
            language.name = "Language it";
            language.languageOptionName = "Italiano";
            language.languageTag = "it";
            language.languageENUM = TranslationManager.Language.IT;
            // Like French, Portuguese and Russian: the database only has [Female] variants.
            language.isUsingGenderedLocalization = true;
            Object.DontDestroyOnLoad(language);
            return language;
        }
    }
}
