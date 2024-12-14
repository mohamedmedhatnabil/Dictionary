open System
open System.IO
open System.Windows.Forms
open Newtonsoft.Json
open GUI  // استدعاء وحدة الواجهة

// Define a type for the dictionary entries
type DictionaryEntry = {
    Word: string
    Definition: string
}

// Dictionary Manager Module
module DictionaryManager =
    // Store dictionary as an F# Map
    let mutable dictionary = Map.empty<string, string>

    // Add a word to the dictionary
    let addWord word definition =
        dictionary <- dictionary.Add(word.ToLower(), definition)
        MessageBox.Show($"Word '{word}' added successfully.") |> ignore

    // Update a word's definition
    let updateWord word newDefinition =
        if dictionary.ContainsKey(word.ToLower()) then
            dictionary <- dictionary.Add(word.ToLower(), newDefinition)
            MessageBox.Show($"Word '{word}' updated successfully.") |> ignore
        else
            MessageBox.Show($"Word '{word}' not found.") |> ignore

    // Delete a word
    let deleteWord word =
        if dictionary.ContainsKey(word.ToLower()) then
            dictionary <- dictionary.Remove(word.ToLower())
            MessageBox.Show($"Word '{word}' deleted successfully.") |> ignore
        else
            MessageBox.Show($"Word '{word}' not found.") |> ignore

    // Search for words (case-insensitive)
    let searchWord keyword =
        let results =
            dictionary
            |> Map.filter (fun key _ -> key.Contains(keyword.ToLower()))
        if results.IsEmpty then
            MessageBox.Show($"No matches found for '{keyword}'.") |> ignore
        else
            let resultText = 
                results |> Map.fold (fun acc key value -> acc + $"\n  {key}: {value}") ""
            MessageBox.Show($"Search results for '{keyword}': {resultText}") |> ignore

    // Save dictionary to a JSON file
    let saveToFile filePath =
        let json = JsonConvert.SerializeObject(dictionary)
        File.WriteAllText(filePath, json)
        MessageBox.Show($"Dictionary saved to '{filePath}'.") |> ignore

    // Load dictionary from a JSON file
    let loadFromFile filePath =
        if File.Exists(filePath) then
            let json = File.ReadAllText(filePath)
            dictionary <- JsonConvert.DeserializeObject<Map<string, string>>(json)
            MessageBox.Show($"Dictionary loaded from '{filePath}'.") |> ignore
        else
            MessageBox.Show($"File '{filePath}' does not exist.") |> ignore

// Main Program
[<EntryPoint>]
let main argv =
    let filePath = "dictionary.json"

    // Load dictionary from file at startup
    DictionaryManager.loadFromFile filePath

    // Start the GUI
    GUI.startGUI()

    0 // Exit code
