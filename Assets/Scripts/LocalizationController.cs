using System.Collections.Generic;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    public static LocalizationController Instance;

    private Dictionary<string, string> currentLanguage;

    private const string LANGUAGE_KEY = "language";

    private List<LocalizedText> texts = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        int lang = PlayerPrefs.GetInt(LANGUAGE_KEY, 0);
        SetLanguage(lang);
    }

    public void SetLanguage(int index)
    {
        PlayerPrefs.SetInt(LANGUAGE_KEY, index);

        switch (index)
        {
            case 0:
                currentLanguage = english; break;
            case 1:
                currentLanguage = italiano; break;
            case 2:
                currentLanguage = espanol; break;
            case 3:
                currentLanguage = francais; break;
            case 4:
                currentLanguage = portugues; break;
        }

        UpdateAllTexts();
    }

    public string Get(string key)
    {
        if (currentLanguage == null) return key;

        if (currentLanguage.ContainsKey(key))
            return currentLanguage[key];

        return key;
    }

    private Dictionary<string, string> english = new()
    {
        {"tap_to_start", "Tap to Start"},
        {"select_difficulty", "Select Difficulty"},
        {"4_pairs", "4\r\npairs"},
        {"8_pairs", "8\r\npairs"},
        {"12_pairs", "12\r\npairs"},
        {"16_pairs", "16\r\npairs"},
        {"easy", "Easy"},
        {"medium", "Medium"},
        {"hard", "Hard"},
        {"extreme", "Extreme"},
        {"<_back", "< Back"},
        {"you_win!", "You Win!"},
        {"time", "Time"},
        {"score", "Score"},
        {"moves", "Moves"},
        {"new_record", "New Record!"},
        {"replay", "Replay"},
        {"change", "Change"},
        {"help", "Help"},
        {"how_to_play", "How To Play"},
        {"how_to_play_description", "Flip two cards at a time.\r\nIf they match, they stay open.\r\nFind all pairs to win."},
        {"scoring", "Scoring"},
        {"scoring_description", "Score depends on:\r\n• Number of moves\r\n• Time spent\r\n• Difficulty level"},
        {"difficulty", "Difficulty"},
        {"close", "Close"},
        {"pause", "Pause"},
        {"resume", "Resume"},
        {"restart", "Restart"},
        {"settings", "Settings"},
        {"quit", "Quit"},
        {"statistics", "Statistics"},
        {"best_score", "Best Score"},
        {"best_time", "Best Time"},
        {"best_moves", "Best Moves"},
        {"games", "Games"},
        {"reset_stats", "Reset Stats"},
        {"are_you_sure?", "Are You Sure?"},
        {"are_you_sure_subtitle", "This will permanently delete all your statistics.\r\nThis action cannot be undone."},
        {"cancel", "Cancel"},
        {"delete", "Delete"},
        {"stats_success_reset", "Statistics Successfully Reset"},
        {"audio", "Audio"},
        {"music_volume", "Music Volume"},
        {"sfx_volume", "SFX Volume"},
        {"music_track", "Music Track"},
        {"shuffle", "Shuffle"},
        {"orchestral", "Orchestral"},
        {"gameplay", "Gameplay"},
        {"vibration", "Vibration"},
        {"appearance", "Appearance"},
        {"theme", "Theme"},
        {"dark", "Dark"},
        {"light", "Light"},
        {"system", "System"},
        {"language", "Language"},
        {"version", "Version"},
        {"restore_defaults", "Restore Defaults"},
        {"exit_game?", "Exit Game?"},
        {"exit_description", "Are you sure you want to quit?"},
        {"no", "No"},
        {"yes", "Yes"},
        {"thanks_for_playing!", "Thanks for playing!"}
    };

    private Dictionary<string, string> italiano = new()
    {
        {"tap_to_start", "Premi per Iniziare"},
        {"select_difficulty", "Selezione Difficoltà"},
        {"4_pairs", "4\r\ncoppie"},
        {"8_pairs", "8\r\ncoppie"},
        {"12_pairs", "12\r\ncoppie"},
        {"16_pairs", "16\r\ncoppie"},
        {"easy", "Facile"},
        {"medium", "Media"},
        {"hard", "Difficile"},
        {"extreme", "Estrema"},
        {"<_back", "< Indietro"},
        {"you_win!", "Hai Vinto!"},
        {"time", "Tempo"},
        {"score", "Punteggio"},
        {"moves", "Mosse"},
        {"new_record", "Nuovo Record!"},
        {"replay", "Rigioca"},
        {"change", "Cambia"},
        {"help", "Aiuto"},
        {"how_to_play", "Come Giocare"},
        {"how_to_play_description", "Gira due carte alla volta.\r\nSe uguali, restano scoperte.\r\nTrova le coppie per vincere."},
        {"scoring", "Punteggio"},
        {"scoring_description", "Il punteggio dipende da:\r\n• Numero di mosse\r\n• Tempo impiegato\r\n• Livello di difficoltà"},
        {"difficulty", "Difficoltà"},
        {"close", "Chiudi"},
        {"pause", "Pausa"},
        {"resume", "Riprendi"},
        {"restart", "Ricomincia"},
        {"settings", "Impostazioni"},
        {"quit", "Abbandona"},
        {"statistics", "Statistiche"},
        {"best_score", "Punti Max"},
        {"best_time", "Tempo Min"},
        {"best_moves", "Mosse Min"},
        {"games", "Partite"},
        {"reset_stats", "Azzera Dati"},
        {"are_you_sure?", "Sei Sicuro?"},
        {"are_you_sure_subtitle", "Questa operazione eliminerà definitivamente tutte le tue statistiche.\r\nQuesta azione non può essere annullata."},
        {"cancel", "Annulla"},
        {"delete", "Elimina"},
        {"stats_success_reset", "Statistiche Azzerate con Successo"},
        {"audio", "Audio"},
        {"music_volume", "Volume Musica"},
        {"sfx_volume", "Volume Effetti"},
        {"music_track", "Traccia Musicale"},
        {"shuffle", "A Giro"},
        {"orchestral", "Orchestrale"},
        {"gameplay", "Gioco"},
        {"vibration", "Vibrazione"},
        {"appearance", "Apparenza"},
        {"theme", "Tema"},
        {"dark", "Scuro"},
        {"light", "Chiaro"},
        {"system", "Sistema"},
        {"language", "Lingua"},
        {"version", "Versione"},
        {"restore_defaults", "Ripristina"},
        {"exit_game?", "Uscire dal Gioco?"},
        {"exit_description", "Sei sicuro di voler abbandonare?"},
        {"no", "No"},
        {"yes", "Sì"},
        {"thanks_for_playing!", "Grazie per aver giocato!"}
    };

    private Dictionary<string, string> espanol = new()
    {
        {"tap_to_start", "Pulsa para Iniciar"},
        {"select_difficulty", "Selección Dificultad"},
        {"4_pairs", "4\r\npares"},
        {"8_pairs", "8\r\npares"},
        {"12_pairs", "12\r\npares"},
        {"16_pairs", "16\r\npares"},
        {"easy", "Fácil"},
        {"medium", "Media"},
        {"hard", "Difícil"},
        {"extreme", "Extrema"},
        {"<_back", "< Volver"},
        {"you_win!", "¡Has Ganado!"},
        {"time", "Tiempo"},
        {"score", "Puntuación"},
        {"moves", "Movimientos"},
        {"new_record", "¡Nuevo Récord!"},
        {"replay", "Rejugar"},
        {"change", "Cambiar"},
        {"help", "Ayuda"},
        {"how_to_play", "Cómo Jugar"},
        {"how_to_play_description", "Gira dos cartas cada vez.\r\nSi coinciden, quedan abiertas.\r\nEncuentra las parejas para ganar."},
        {"scoring", "Puntuación"},
        {"scoring_description", "La puntuación depende de:\r\n• Número de movimientos\r\n• Tiempo empleado\r\n• Nivel de dificultad"},
        {"difficulty", "Dificultad"},
        {"close", "Cerrar"},
        {"pause", "Pausa"},
        {"resume", "Continuar"},
        {"restart", "Reiniciar"},
        {"settings", "Ajustes"},
        {"quit", "Salir"},
        {"statistics", "Estadísticas"},
        {"best_score", "Puntos Máx"},
        {"best_time", "Tiempo Min"},
        {"best_moves", "Mov. Min"},
        {"games", "Partidas"},
        {"reset_stats", "Vaciar"},
        {"are_you_sure?", "¿Seguro?"},
        {"are_you_sure_subtitle", "Esta operación eliminará todas tus estadísticas.\r\nEsta acción no se puede deshacer."},
        {"cancel", "Cancelar"},
        {"delete", "Eliminar"},
        {"stats_success_reset", "Estadísticas Borradas con Éxito"},
        {"audio", "Audio"},
        {"music_volume", "Volumen Música"},
        {"sfx_volume", "Volumen Efectos"},
        {"music_track", "Pista Musical"},
        {"shuffle", "Variable"},
        {"orchestral", "Orquestal"},
        {"gameplay", "Juego"},
        {"vibration", "Vibración"},
        {"appearance", "Apariencia"},
        {"theme", "Tema"},
        {"dark", "Oscuro"},
        {"light", "Claro"},
        {"system", "Sistema"},
        {"language", "Idioma"},
        {"version", "Versión"},
        {"restore_defaults", "Restaurar"},
        {"exit_game?", "¿Salir del Juego?"},
        {"exit_description", "¿Seguro que quieres salir?"},
        {"no", "No"},
        {"yes", "Sí"},
        {"thanks_for_playing!", "¡Gracias por jugar!"}
    };

    private Dictionary<string, string> francais = new()
    {
        {"tap_to_start", "Appuie pour Démarrer"},
        {"select_difficulty", "Sélection Difficulté"},
        {"4_pairs", "4\r\npaires"},
        {"8_pairs", "8\r\npaires"},
        {"12_pairs", "12\r\npaires"},
        {"16_pairs", "16\r\npaires"},
        {"easy", "Facile"},
        {"medium", "Moyen"},
        {"hard", "Difficile"},
        {"extreme", "Extrême"},
        {"<_back", "< Retour"},
        {"you_win!", "Tu as Gagné!"},
        {"time", "Temps"},
        {"score", "Score"},
        {"moves", "Coups"},
        {"new_record", "Nouveau Record!"},
        {"replay", "Rejouer"},
        {"change", "Changer"},
        {"help", "Aide"},
        {"how_to_play", "Comment Jouer"},
        {"how_to_play_description", "Retourne deux cartes.\r\nSi identiques, restent visibles.\r\nTrouve les paires pour gagner."},
        {"scoring", "Score"},
        {"scoring_description", "Le score dépend de:\r\n• Nombre de coups\r\n• Temps écoulé\r\n• Niveau de difficulté"},
        {"difficulty", "Difficulté"},
        {"close", "Fermer"},
        {"pause", "Pause"},
        {"resume", "Reprendre"},
        {"restart", "Redémarrer"},
        {"settings", "Paramètres"},
        {"quit", "Quitter"},
        {"statistics", "Statistiques"},
        {"best_score", "Score Max"},
        {"best_time", "Temps Min"},
        {"best_moves", "Coups Min"},
        {"games", "Parties"},
        {"reset_stats", "Réinitialiser"},
        {"are_you_sure?", "Es-tu sûr?"},
        {"are_you_sure_subtitle", "Cette opération supprimera toutes tes statistiques.\r\nCette action est irréversible."},
        {"cancel", "Annuler"},
        {"delete", "Supprimer"},
        {"stats_success_reset", "Statistiques Réinit avec Succès"},
        {"audio", "Audio"},
        {"music_volume", "Volume Musique"},
        {"sfx_volume", "Volume Effets"},
        {"music_track", "Piste Musicale"},
        {"shuffle", "Aléatoire"},
        {"orchestral", "Orchestral"},
        {"gameplay", "Jeu"},
        {"vibration", "Vibration"},
        {"appearance", "Apparence"},
        {"theme", "Thème"},
        {"dark", "Sombre"},
        {"light", "Clair"},
        {"system", "Système"},
        {"language", "Langue"},
        {"version", "Version"},
        {"restore_defaults", "Restaurer"},
        {"exit_game?", "Quitter le Jeu?"},
        {"exit_description", "Veux-tu vraiment quitter?"},
        {"no", "Non"},
        {"yes", "Oui"},
        {"thanks_for_playing!", "Merci d'avoir joué!"}
    };

    private Dictionary<string, string> portugues = new()
    {
        {"tap_to_start", "Toque para Iniciar"},
        {"select_difficulty", "Seleção Dificuldade"},
        {"4_pairs", "4\r\npares"},
        {"8_pairs", "8\r\npares"},
        {"12_pairs", "12\r\npares"},
        {"16_pairs", "16\r\npares"},
        {"easy", "Fácil"},
        {"medium", "Médio"},
        {"hard", "Difícil"},
        {"extreme", "Extremo"},
        {"<_back", "< Voltar"},
        {"you_win!", "Você Venceu!"},
        {"time", "Tempo"},
        {"score", "Pontuação"},
        {"moves", "Movimentos"},
        {"new_record", "Novo Recorde!"},
        {"replay", "Rejogar"},
        {"change", "Mudar"},
        {"help", "Ajuda"},
        {"how_to_play", "Como Jogar"},
        {"how_to_play_description", "Vire duas cartas.\r\nSe iguais, ficam abertas.\r\nEncontre os pares para vencer."},
        {"scoring", "Pontuação"},
        {"scoring_description", "A pontuação depende de:\r\n• Número de movimentos\r\n• Tempo gasto\r\n• Nível de dificuldade"},
        {"difficulty", "Dificuldade"},
        {"close", "Fechar"},
        {"pause", "Pausa"},
        {"resume", "Retomar"},
        {"restart", "Reiniciar"},
        {"settings", "Ajustes"},
        {"quit", "Sair"},
        {"statistics", "Estatísticas"},
        {"best_score", "Ptos. Máx"},
        {"best_time", "Tempo Min"},
        {"best_moves", "Mov. Min"},
        {"games", "Partidas"},
        {"reset_stats", "Zerar"},
        {"are_you_sure?", "Tem certeza?"},
        {"are_you_sure_subtitle", "Esta ação apagará todas as estatísticas.\r\nEsta ação não pode ser desfeita."},
        {"cancel", "Cancelar"},
        {"delete", "Excluir"},
        {"stats_success_reset", "Estatísticas Apagadas com Sucesso"},
        {"audio", "Áudio"},
        {"music_volume", "Volume Música"},
        {"sfx_volume", "Volume Efeitos"},
        {"music_track", "Faixa Musical"},
        {"shuffle", "Aleatório"},
        {"orchestral", "Orquestral"},
        {"gameplay", "Jogo"},
        {"vibration", "Vibração"},
        {"appearance", "Aparência"},
        {"theme", "Tema"},
        {"dark", "Escuro"},
        {"light", "Claro"},
        {"system", "Sistema"},
        {"language", "Idioma"},
        {"version", "Versão"},
        {"restore_defaults", "Restaurar"},
        {"exit_game?", "Sair do Jogo?"},
        {"exit_description", "Tem certeza que quer sair?"},
        {"no", "Não"},
        {"yes", "Sim"},
        {"thanks_for_playing!", "Obrigado por jogar!"}
    };

    public void Register(LocalizedText text)
    {
        texts.Add(text);
    }

    private void UpdateAllTexts()
    {
        foreach (LocalizedText t in texts)
            t.UpdateText();
    }
}