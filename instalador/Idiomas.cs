using System;
using System.Collections.Generic;
using System.Globalization;

namespace PreviewVolumeSetup
{
    // Textos del instalador en los 10 idiomas de Divolandia Labs. Clave = texto en español.
    // Orden de cada fila: es, en, pt, fr, de, it, pl, ru, ko, ja.
    static class L
    {
        public static readonly string[] Codigos = { "es", "en", "pt", "fr", "de", "it", "pl", "ru", "ko", "ja" };
        public static readonly string[] Nombres = { "Español", "English", "Português", "Français", "Deutsch", "Italiano", "Polski", "Русский", "한국어", "日本語" };
        public static int Actual = Detectar();

        static int Detectar()
        {
            string c = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            int i = Array.IndexOf(Codigos, c);
            return i >= 0 ? i : 1; // inglés si el idioma de Windows no está
        }

        public static string T(string es)
        {
            string[] f;
            if (Actual == 0 || !Tabla.TryGetValue(es, out f)) return es;
            return f[Actual - 1];
        }

        public static string F(string es, params object[] a) { return string.Format(T(es), a); }

        static readonly Dictionary<string, string[]> Tabla = new Dictionary<string, string[]>
        {
            { "Instalar {0}", new[] { "Install {0}", "Instalar {0}", "Installer {0}", "{0} installieren", "Installa {0}", "Zainstaluj {0}", "Установка {0}", "{0} 설치", "{0} のインストール" } },
            { "Versión {0} · Divolandia Labs", new[] { "Version {0} · Divolandia Labs", "Versão {0} · Divolandia Labs", "Version {0} · Divolandia Labs", "Version {0} · Divolandia Labs", "Versione {0} · Divolandia Labs", "Wersja {0} · Divolandia Labs", "Версия {0} · Divolandia Labs", "버전 {0} · Divolandia Labs", "バージョン {0} · Divolandia Labs" } },
            { "Ya está instalado: se actualizará en la misma carpeta.", new[] { "Already installed: it will be updated in the same folder.", "Já está instalado: será atualizado na mesma pasta.", "Déjà installé : il sera mis à jour dans le même dossier.", "Bereits installiert: wird im selben Ordner aktualisiert.", "Già installato: verrà aggiornato nella stessa cartella.", "Już zainstalowany: zostanie zaktualizowany w tym samym folderze.", "Уже установлено: будет обновлено в той же папке.", "이미 설치되어 있습니다. 같은 폴더에서 업데이트합니다.", "インストール済みです。同じフォルダーで更新します。" } },
            { "Carpeta de instalación (solo para tu usuario, sin permisos de administrador):", new[] { "Install folder (for your user only, no admin rights needed):", "Pasta de instalação (só para o seu usuário, sem administrador):", "Dossier d'installation (pour vous seul, sans droits d'admin) :", "Installationsordner (nur für Sie, ohne Adminrechte):", "Cartella di installazione (solo per te, senza amministratore):", "Folder instalacji (tylko dla Ciebie, bez administratora):", "Папка установки (только для вас, без прав администратора):", "설치 폴더 (현재 사용자 전용, 관리자 권한 불필요):", "インストール先（このユーザーのみ・管理者権限は不要）：" } },
            { "Examinar…", new[] { "Browse…", "Procurar…", "Parcourir…", "Durchsuchen…", "Sfoglia…", "Przeglądaj…", "Обзор…", "찾아보기…", "参照…" } },
            { "Iniciar {0} con Windows", new[] { "Start {0} with Windows", "Iniciar o {0} com o Windows", "Lancer {0} au démarrage de Windows", "{0} mit Windows starten", "Avvia {0} con Windows", "Uruchamiaj {0} razem z Windows", "Запускать {0} вместе с Windows", "Windows 시작 시 {0} 실행", "Windows の起動時に {0} を開始" } },
            { "Crear acceso directo en el escritorio", new[] { "Create a desktop shortcut", "Criar atalho na área de trabalho", "Créer un raccourci sur le bureau", "Desktopverknüpfung erstellen", "Crea un collegamento sul desktop", "Utwórz skrót na pulpicie", "Создать ярлык на рабочем столе", "바탕 화면에 바로 가기 만들기", "デスクトップにショートカットを作成" } },
            { "Abrir {0} al terminar", new[] { "Open {0} when finished", "Abrir o {0} ao terminar", "Ouvrir {0} à la fin", "{0} nach Abschluss öffnen", "Apri {0} al termine", "Otwórz {0} po zakończeniu", "Открыть {0} после установки", "완료 후 {0} 열기", "完了後に {0} を開く" } },
            { "Instalar", new[] { "Install", "Instalar", "Installer", "Installieren", "Installa", "Instaluj", "Установить", "설치", "インストール" } },
            { "Actualizar", new[] { "Update", "Atualizar", "Mettre à jour", "Aktualisieren", "Aggiorna", "Aktualizuj", "Обновить", "업데이트", "更新" } },
            { "Cancelar", new[] { "Cancel", "Cancelar", "Annuler", "Abbrechen", "Annulla", "Anuluj", "Отмена", "취소", "キャンセル" } },
            { "Finalizar", new[] { "Finish", "Concluir", "Terminer", "Fertigstellen", "Fine", "Zakończ", "Готово", "마침", "完了" } },
            { "Cerrar", new[] { "Close", "Fechar", "Fermer", "Schließen", "Chiudi", "Zamknij", "Закрыть", "닫기", "閉じる" } },
            { "Barra de tareas:", new[] { "Taskbar:", "Barra de tarefas:", "Barre des tâches :", "Taskleiste:", "Barra delle applicazioni:", "Pasek zadań:", "Панель задач:", "작업 표시줄:", "タスクバー：" } },
            { "Windows 10 (o ExplorerPatcher)", new[] { "Windows 10 (or ExplorerPatcher)", "Windows 10 (ou ExplorerPatcher)", "Windows 10 (ou ExplorerPatcher)", "Windows 10 (oder ExplorerPatcher)", "Windows 10 (o ExplorerPatcher)", "Windows 10 (lub ExplorerPatcher)", "Windows 10 (или ExplorerPatcher)", "Windows 10 (또는 ExplorerPatcher)", "Windows 10（または ExplorerPatcher）" } },
            { "Idioma", new[] { "Language", "Idioma", "Langue", "Sprache", "Lingua", "Język", "Язык", "언어", "言語" } },
            { "Elige una carpeta válida.", new[] { "Choose a valid folder.", "Escolha uma pasta válida.", "Choisissez un dossier valide.", "Wählen Sie einen gültigen Ordner.", "Scegli una cartella valida.", "Wybierz prawidłowy folder.", "Выберите правильную папку.", "올바른 폴더를 선택하세요.", "有効なフォルダーを選んでください。" } },
            { "No hay permiso para escribir en esa carpeta. Elige otra (por ejemplo, la propuesta).", new[] { "No permission to write to that folder. Choose another one (for example, the suggested one).", "Sem permissão para gravar nessa pasta. Escolha outra (por exemplo, a sugerida).", "Impossible d'écrire dans ce dossier. Choisissez-en un autre (par exemple celui proposé).", "Keine Schreibrechte für diesen Ordner. Wählen Sie einen anderen (z. B. den vorgeschlagenen).", "Non è possibile scrivere in quella cartella. Scegline un'altra (ad esempio quella proposta).", "Brak uprawnień do zapisu w tym folderze. Wybierz inny (np. proponowany).", "Нет прав на запись в эту папку. Выберите другую (например, предложенную).", "이 폴더에 쓸 권한이 없습니다. 다른 폴더(예: 기본 제안 폴더)를 선택하세요.", "このフォルダーには書き込めません。別のフォルダー（例：既定のフォルダー）を選んでください。" } },
            { "No se pudo instalar:", new[] { "Installation failed:", "Não foi possível instalar:", "L'installation a échoué :", "Installation fehlgeschlagen:", "Installazione non riuscita:", "Nie udało się zainstalować:", "Не удалось установить:", "설치하지 못했습니다:", "インストールできませんでした：" } },
            { "Instalado. Está junto al reloj: pasa el ratón por el icono de Spotify.", new[] { "Installed. It sits next to the clock: hover over the Spotify icon.", "Instalado. Fica ao lado do relógio: passe o mouse no ícone do Spotify.", "Installé. Il est près de l'horloge : survolez l'icône de Spotify.", "Installiert. Es sitzt neben der Uhr: Zeigen Sie auf das Spotify-Symbol.", "Installato. È accanto all'orologio: passa il mouse sull'icona di Spotify.", "Zainstalowano. Jest obok zegara: najedź na ikonę Spotify.", "Установлено. Значок у часов: наведите курсор на значок Spotify.", "설치되었습니다. 시계 옆에 있으니 Spotify 아이콘에 마우스를 올려 보세요.", "インストールしました。時計の横にあります。Spotify のアイコンにマウスを合わせてください。" } },
            { "Desinstalar {0}", new[] { "Uninstall {0}", "Desinstalar {0}", "Désinstaller {0}", "{0} deinstallieren", "Disinstalla {0}", "Odinstaluj {0}", "Удаление {0}", "{0} 제거", "{0} のアンインストール" } },
            { "Se quitará el programa, sus accesos directos y su registro", new[] { "The program, its shortcuts and its registry entries will be removed", "O programa, os atalhos e o registro serão removidos", "Le programme, ses raccourcis et son registre seront supprimés", "Programm, Verknüpfungen und Registrierung werden entfernt", "Verranno rimossi il programma, i collegamenti e il registro", "Program, skróty i wpisy w rejestrze zostaną usunięte", "Будут удалены программа, ярлыки и записи реестра", "프로그램, 바로 가기, 레지스트리 항목이 제거됩니다", "プログラム、ショートカット、レジストリの項目を削除します" } },
            { "Carpeta: {0}", new[] { "Folder: {0}", "Pasta: {0}", "Dossier : {0}", "Ordner: {0}", "Cartella: {0}", "Folder: {0}", "Папка: {0}", "폴더: {0}", "フォルダー：{0}" } },
            { "Desinstalar", new[] { "Uninstall", "Desinstalar", "Désinstaller", "Deinstallieren", "Disinstalla", "Odinstaluj", "Удалить", "제거", "アンインストール" } },
            { "{0} se ha desinstalado.", new[] { "{0} has been uninstalled.", "O {0} foi desinstalado.", "{0} a été désinstallé.", "{0} wurde deinstalliert.", "{0} è stato disinstallato.", "{0} został odinstalowany.", "{0} удалён.", "{0}이(가) 제거되었습니다.", "{0} をアンインストールしました。" } },
            { "Preparando la carpeta…", new[] { "Preparing the folder…", "Preparando a pasta…", "Préparation du dossier…", "Ordner wird vorbereitet…", "Preparazione della cartella…", "Przygotowywanie folderu…", "Подготовка папки…", "폴더 준비 중…", "フォルダーを準備しています…" } },
            { "Cerrando {0} si está abierto…", new[] { "Closing {0} if it is running…", "Fechando o {0} se estiver aberto…", "Fermeture de {0} s'il est ouvert…", "{0} wird geschlossen, falls geöffnet…", "Chiusura di {0} se è aperto…", "Zamykanie {0}, jeśli jest uruchomiony…", "Закрытие {0}, если он запущен…", "실행 중이면 {0} 닫는 중…", "{0} を実行中なら終了しています…" } },
            { "Copiando {0}…", new[] { "Copying {0}…", "Copiando {0}…", "Copie de {0}…", "{0} wird kopiert…", "Copia di {0}…", "Kopiowanie {0}…", "Копирование {0}…", "{0} 복사 중…", "{0} をコピーしています…" } },
            { "Creando el desinstalador…", new[] { "Creating the uninstaller…", "Criando o desinstalador…", "Création du programme de désinstallation…", "Deinstallationsprogramm wird erstellt…", "Creazione del programma di disinstallazione…", "Tworzenie deinstalatora…", "Создание программы удаления…", "제거 프로그램 만드는 중…", "アンインストーラーを作成しています…" } },
            { "Creando accesos directos…", new[] { "Creating shortcuts…", "Criando atalhos…", "Création des raccourcis…", "Verknüpfungen werden erstellt…", "Creazione dei collegamenti…", "Tworzenie skrótów…", "Создание ярлыков…", "바로 가기 만드는 중…", "ショートカットを作成しています…" } },
            { "Registrando en Windows…", new[] { "Registering with Windows…", "Registrando no Windows…", "Enregistrement dans Windows…", "Registrierung in Windows…", "Registrazione in Windows…", "Rejestrowanie w Windows…", "Регистрация в Windows…", "Windows에 등록 중…", "Windows に登録しています…" } },
            { "Actualizando los iconos de Windows…", new[] { "Refreshing Windows icons…", "Atualizando os ícones do Windows…", "Actualisation des icônes de Windows…", "Windows-Symbole werden aktualisiert…", "Aggiornamento delle icone di Windows…", "Odświeżanie ikon Windows…", "Обновление значков Windows…", "Windows 아이콘 새로 고치는 중…", "Windows のアイコンを更新しています…" } },
            { "Listo", new[] { "Done", "Pronto", "Terminé", "Fertig", "Fatto", "Gotowe", "Готово", "완료", "完了" } },
            { "Cerrando {0}…", new[] { "Closing {0}…", "Fechando o {0}…", "Fermeture de {0}…", "{0} wird geschlossen…", "Chiusura di {0}…", "Zamykanie {0}…", "Закрытие {0}…", "{0} 닫는 중…", "{0} を終了しています…" } },
            { "Quitando accesos directos…", new[] { "Removing shortcuts…", "Removendo atalhos…", "Suppression des raccourcis…", "Verknüpfungen werden entfernt…", "Rimozione dei collegamenti…", "Usuwanie skrótów…", "Удаление ярлыков…", "바로 가기 제거 중…", "ショートカットを削除しています…" } },
            { "Quitando el registro…", new[] { "Removing registry entries…", "Removendo o registro…", "Suppression du registre…", "Registrierung wird entfernt…", "Rimozione dal registro…", "Usuwanie wpisów rejestru…", "Удаление записей реестра…", "레지스트리 항목 제거 중…", "レジストリの項目を削除しています…" } },
            { "Borrando archivos…", new[] { "Deleting files…", "Apagando arquivos…", "Suppression des fichiers…", "Dateien werden gelöscht…", "Eliminazione dei file…", "Usuwanie plików…", "Удаление файлов…", "파일 삭제 중…", "ファイルを削除しています…" } },
            { "Barra de volumen en la vista previa de la barra de tareas", new[] { "Volume bar on the taskbar preview", "Barra de volume na pré-visualização da barra de tarefas", "Barre de volume sur l'aperçu de la barre des tâches", "Lautstärkeregler in der Taskleistenvorschau", "Barra del volume sull'anteprima della barra delle applicazioni", "Suwak głośności w podglądzie paska zadań", "Регулятор громкости в миниатюре панели задач", "작업 표시줄 미리 보기의 볼륨 바", "タスクバーのプレビューに出る音量バー" } },
            { "El instalador está dañado: no contiene {0}", new[] { "The installer is damaged: {0} is missing", "O instalador está danificado: não contém {0}", "Le programme d'installation est endommagé : {0} est absent", "Das Installationsprogramm ist beschädigt: {0} fehlt", "Il programma di installazione è danneggiato: manca {0}", "Instalator jest uszkodzony: brak pliku {0}", "Установщик повреждён: нет файла {0}", "설치 프로그램이 손상되었습니다: {0} 없음", "インストーラーが壊れています：{0} がありません" } },
        };
    }
}
