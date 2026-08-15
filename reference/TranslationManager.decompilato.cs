using System;
using System.Collections;
using System.Collections.Generic;
using Google2u;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class TranslationManager : MonoBehaviour
{
	public enum Language
	{
		DE,
		EN,
		ES,
		FR,
		PL,
		PT,
		RU,
		CN,
		JP,
		TR,
		IT
	}

	public enum TranslationTypes
	{
		MenuUI,
		Quests,
		DictionaryDatabase,
		ActorNames,
		Other,
		ActorNamesShadows,
		DictionaryDatabaseShadows,
		QuestsShadows
	}

	private List<LanguageSO> languages;

	public LanguageSO englishLanguage;

	public LanguageSO frenchLanguage;

	public LanguageSO portugueseLanguage;

	public LanguageSO russianLanguage;

	public LanguageSO chineseLanguage;

	public LanguageSO italianLanguage;

	public LanguageSO spanishLanguage;

	public List<TranslationSprite> sprites;

	public Font EuropeFont;

	public Font rusFont;

	public Font JapanFont;

	public Font ChineseFont;

	public Font TurkishFont;

	private int currentSelectedLanguage;

	private Gender currentSelectedGender;

	private bool isUsingGenderedLocalization;

	private string genderTag = "";

	private List<PixelCrushers.DialogueSystem.Actor> actorPlayer;

	public Action onLanguageChanged;

	private static TranslationManager instance;

	public List<LanguageSO> Languages
	{
		get
		{
			if (languages == null)
			{
				languages = new List<LanguageSO> { englishLanguage, frenchLanguage, portugueseLanguage, russianLanguage };
			}
			return languages;
		}
	}

	public static TranslationManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType<TranslationManager>();
			}
			return instance;
		}
	}

	private void OnEnable()
	{
		StartCoroutine(CheckSavedLanguage());
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
		StartCoroutine(RefreshActorNames());
	}

	private IEnumerator CheckSavedLanguage()
	{
		yield return new WaitWhile(() => SaveManager.Instance == null);
		yield return new WaitWhile(() => SaveManager.Instance.SettingsData == null);
		int languageIndex = SaveManager.Instance.SettingsData.LanguageIndex;
		if (languageIndex < 0 || languageIndex >= Languages.Count)
		{
			SaveManager.Instance.SettingsData.LanguageIndex = GetDefaultLanguage();
		}
	}

	public void ChangeLanguage(int newLanguage)
	{
		onLanguageChanged?.Invoke();
	}

	public int GetDefaultLanguage()
	{
		return Languages.IndexOf(GetSystemLanguage());
	}

	private LanguageSO GetSystemLanguage()
	{
		switch ("".ToLower())
		{
		case "fr":
		case "french":
			return frenchLanguage;
		case "pt-br":
		case "portuguese":
			return portugueseLanguage;
		case "ru":
		case "russian":
			return russianLanguage;
		case "zh-hans":
		case "chinesesimplified":
		case "chinese":
			return chineseLanguage;
		default:
			return englishLanguage;
		}
	}

	public string GetTranslatedText(int textId, TranslationTypes translationType)
	{
		return GetCurrentLanguage() switch
		{
			Language.EN => GetTextEn(textId, translationType), 
			Language.FR => GetTextFr(textId, translationType), 
			Language.DE => GetTextDe(textId, translationType), 
			Language.ES => GetTextEs(textId, translationType), 
			Language.RU => GetTextRu(textId, translationType), 
			Language.PT => GetTextPt(textId, translationType), 
			Language.PL => GetTextPl(textId, translationType), 
			Language.JP => GetTextJp(textId, translationType), 
			Language.CN => GetTextCn(textId, translationType), 
			Language.IT => GetTextIt(textId, translationType), 
			_ => GetTextEn(textId, translationType), 
		};
	}

	public Sprite GetSprite(Translations.rowIds rowIds)
	{
		Language currentLanguage = GetCurrentLanguage();
		TranslationSprite translationSprite = sprites.Find((TranslationSprite x) => x.id == rowIds);
		if (translationSprite != null)
		{
			Sprite sprite = translationSprite.GetSprite(currentLanguage);
			if (sprite != null)
			{
				return sprite;
			}
			Language languageIndex = (Language)SaveManager.Instance.SettingsData.LanguageIndex;
			Debug.LogError("Brak sprite w obiekcie w translation manager dla jezyka: " + languageIndex);
		}
		else
		{
			Debug.LogError("Brak takiego obiektu w translation manager: " + rowIds);
		}
		return null;
	}

	public Font GetFont()
	{
		return GetCurrentLanguage() switch
		{
			Language.JP => JapanFont, 
			Language.CN => ChineseFont, 
			Language.TR => TurkishFont, 
			Language.RU => rusFont, 
			_ => EuropeFont, 
		};
	}

	public void SetGenderedLanguage(Gender gender)
	{
		currentSelectedLanguage = GetLanguageIndex();
		currentSelectedGender = gender;
		isUsingGenderedLocalization = Languages[currentSelectedLanguage].isUsingGenderedLocalization;
		SetLanguage();
	}

	public void ChangeGameLanguage(int _currentSelectedLanguageID)
	{
		currentSelectedLanguage = _currentSelectedLanguageID;
		isUsingGenderedLocalization = Languages[currentSelectedLanguage].isUsingGenderedLocalization;
		SetLanguage();
	}

	private void SetLanguage()
	{
		genderTag = GetGenderTag(currentSelectedGender, isUsingGenderedLocalization);
		DialogueManager.instance.SetLanguage(Languages[currentSelectedLanguage].languageTag + genderTag);
		StartCoroutine(RefreshActorNames());
	}

	private string GetGenderTag(Gender gender, bool _isUsingGenderedLocalization)
	{
		string result = "";
		if (_isUsingGenderedLocalization)
		{
			switch (gender)
			{
			case Gender.MALE:
				result = "[Male]";
				break;
			case Gender.FEMALE:
				result = "[Female]";
				break;
			case Gender.OTHER:
				result = "[Other]";
				break;
			}
		}
		return result;
	}

	private string GetTextEn(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._EN, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._EN, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._EN, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._EN, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._EN, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._EN, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._EN, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._EN, 
			_ => "", 
		};
	}

	private string GetTextFr(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._FR, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._FR, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._FR, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._FR, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._FR, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._FR, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._FR, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._FR, 
			_ => "", 
		};
	}

	private string GetTextPt(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._PT, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._PT, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._PT, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._PT, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._PT, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._PT, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._PT, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._PT, 
			_ => "", 
		};
	}

	private string GetTextDe(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._DE, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._DE, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._DE, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._DE, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._DE, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._DE, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._DE, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._DE, 
			_ => "", 
		};
	}

	private string GetTextEs(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._ES, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._ES, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._ES, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._ES, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._ES, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._ES, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._ES, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._ES, 
			_ => "", 
		};
	}

	private string GetTextPl(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._PL, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._PL, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._PL, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._PL, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._PL, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._PL, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._PL, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._PL, 
			_ => "", 
		};
	}

	private string GetTextRu(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._RU, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._RU, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._RU, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._RU, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._RU, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._RU, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._RU, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._RU, 
			_ => "", 
		};
	}

	private string GetTextJp(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._JP, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._JP, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._JP, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._JP, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._JP, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._JP, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._JP, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._JP, 
			_ => "", 
		};
	}

	private string GetTextCn(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._CN, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._CN, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._CN, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._CN, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._CN, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._CN, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._CN, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._CN, 
			_ => "", 
		};
	}

	private string GetTextIt(int textID, TranslationTypes translationType)
	{
		return translationType switch
		{
			TranslationTypes.MenuUI => MenuUITranslations.Instance.GetRow((MenuUITranslations.rowIds)textID)._IT, 
			TranslationTypes.Quests => QuestsTranslations.Instance.GetRow((QuestsTranslations.rowIds)textID)._IT, 
			TranslationTypes.DictionaryDatabase => DictionaryDatabaseTranslations.Instance.GetRow((DictionaryDatabaseTranslations.rowIds)textID)._IT, 
			TranslationTypes.ActorNames => ActorNamesTranslations.Instance.GetRow((ActorNamesTranslations.rowIds)textID)._IT, 
			TranslationTypes.Other => OtherTranslations.Instance.GetRow((OtherTranslations.rowIds)textID)._IT, 
			TranslationTypes.ActorNamesShadows => ShadowsActorNamesTranslations.Instance.GetRow((ShadowsActorNamesTranslations.rowIds)textID)._IT, 
			TranslationTypes.DictionaryDatabaseShadows => ShadowsDictionaryDatabaseTranslations.Instance.GetRow((ShadowsDictionaryDatabaseTranslations.rowIds)textID)._IT, 
			TranslationTypes.QuestsShadows => ShadowsQuestTranslations.Instance.GetRow((ShadowsQuestTranslations.rowIds)textID)._IT, 
			_ => "", 
		};
	}

	private IEnumerator RefreshActorNames()
	{
		yield return new WaitWhile(() => DialogueManager.instance == null);
		yield return new WaitWhile(() => DialogueManager.instance.databaseManager == null);
		actorPlayer = new List<PixelCrushers.DialogueSystem.Actor>();
		DatabaseManager databaseManager = DialogueManager.instance.databaseManager;
		for (int i = 0; i < databaseManager.defaultDatabase.actors.Count; i++)
		{
			PixelCrushers.DialogueSystem.Actor actor = databaseManager.defaultDatabase.actors[i];
			for (int j = 0; j < actor.fields.Count; j++)
			{
				Field field = actor.fields[j];
				if (field.title == "Display Name")
				{
					if (actor.IsPlayer)
					{
						actorPlayer.Add(actor);
					}
					else
					{
						field.value = GetTranslatedText((int)field.textID_actorNamesTranslationShadows, TranslationTypes.ActorNamesShadows);
					}
				}
			}
		}
		RefreshPlayerName();
	}

	public void RefreshPlayerName()
	{
		foreach (PixelCrushers.DialogueSystem.Actor item in actorPlayer)
		{
			for (int i = 0; i < item.fields.Count; i++)
			{
				Field field = item.fields[i];
				if (field.title == "Display Name" && SaveManager.Instance.Slots[SaveManager.Instance.CurrentSlot] != null)
				{
					field.value = SaveManager.Instance.StateData.playerName;
				}
			}
		}
	}

	public Language GetCurrentLanguage()
	{
		int languageIndex = GetLanguageIndex();
		return Languages[languageIndex].languageENUM;
	}

	public int GetLanguageIndex()
	{
		if (SaveManager.Instance.SettingsData == null)
		{
			return GetDefaultLanguage();
		}
		int num = SaveManager.Instance.SettingsData.LanguageIndex;
		if (num < 0)
		{
			num = GetDefaultLanguage();
		}
		return Mathf.Min(num, Languages.Count - 1);
	}
}
You are not using the latest version of the tool, please update.
Latest version is '11.0.0.9375' (yours is '8.2.0.7535-95108c96')
