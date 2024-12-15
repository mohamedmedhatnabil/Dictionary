open System
open System.IO
open System.Windows.Forms
open GUI 

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
// Main Program
[<EntryPoint>]
let main argv =

    // Start the GUI
    GUI.startGUI()

    0 // Exit code
