/*
//Fonctionnalit� graphique avec menu d�roulant ou bouton radio "RadioButton"
private void BtnSaveLogSettings_Click(object sender, EventArgs e)
{
    // Supposons qu'on ait un ComboBox avec les choix "JSON" et "XAML"
    string selectedFormat = comboBoxLogFormat.SelectedItem.ToString();
    LogFormat format = selectedFormat == "XAML" ? LogFormat.XAML : LogFormat.JSON;

    // Sauvegarder cette pr�f�rence dans le fichier de configuration
    SaveLogFormatToConfig(format);
}

private void SaveLogFormatToConfig(LogFormat format)
{
    var config = new Config { LogFormat = format.ToString() };
    string json = JsonSerializer.Serialize(config);
    File.WriteAllText("config.json", json);
}
//Fonctionnalit� graphique avec une option de r�initialisation du format de log � tout moment pour l'utilisateur
public void ChangerFormat()
{
    Console.WriteLine("Choisissez un nouveau format de log :");
    Console.WriteLine("1 - JSON");
    Console.WriteLine("2 - XAML");

    string choix = Console.ReadLine();
    LogFormat formatChoisi = choix == "2" ? LogFormat.XAML : LogFormat.JSON;

    // Mettre � jour la configuration ou la logique du programme en fonction du format choisi
    Console.WriteLine($"Le format de log a �t� chang� en : {formatChoisi}");
}
*/