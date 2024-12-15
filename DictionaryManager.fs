module DictionaryManager

open System
open Newtonsoft.Json
open System.IO
open System.Windows.Forms

// Define DictionaryEntry type
type DictionaryEntry = {
    Word: string
    Definition: string
}

// Dictionary for storing words and definitions
let mutable dictionary: Map<string, string> = Map.empty

// Add a new word to the dictionary
let addWord (word: string) (definition: string) : unit =
    let lowerWord = word.ToLower()
    dictionary <- dictionary.Add(lowerWord, definition)
    MessageBox.Show($"Word '{word}' added successfully.") |> ignore

// Update the word's definition in the dictionary
let updateWord (word: string) (newDefinition: string) : unit =
    let lowerWord = word.ToLower()
    if dictionary.ContainsKey(lowerWord) then
        dictionary <- dictionary.Add(lowerWord, newDefinition)
        MessageBox.Show($"Word '{word}' updated successfully.") |> ignore
    else
        MessageBox.Show($"Word '{word}' not found. Please check capitalization and spelling.") |> ignore

// Delete a word from the dictionary
let deleteWord (word: string) : unit =
    let lowerWord = word.ToLower()
    if dictionary.ContainsKey(lowerWord) then
        dictionary <- dictionary.Remove(lowerWord)
        MessageBox.Show($"Word '{word}' deleted successfully.") |> ignore
    else
        MessageBox.Show($"Word '{word}' not found. Please check capitalization and spelling.") |> ignore

// Search for a word in the dictionary
let searchWord (keyword: string) : unit =
    let lowerKeyword = keyword.ToLower()
    let results =
        dictionary
        |> Map.filter (fun key _ -> key.Contains(lowerKeyword))
    if results.IsEmpty then
        MessageBox.Show($"No matches found for '{keyword}'.") |> ignore
    else
        let resultText = 
            results |> Map.fold (fun acc key value -> acc + $"\\n  {key}: {value}") ""
        MessageBox.Show($"Search results for '{keyword}': {resultText}") |> ignore

// Save the dictionary to a file
let saveToFile (filePath: string) : unit =
    try
        let json = JsonConvert.SerializeObject(dictionary)
        File.WriteAllText(filePath, json)
    with
    | ex -> MessageBox.Show($"Error saving dictionary: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

// Load the dictionary from a file
let loadFromFile (filePath: string) : unit =
    try
        if File.Exists(filePath) then
            let json = File.ReadAllText(filePath)
            dictionary <- JsonConvert.DeserializeObject<Map<string, string>>(json)
    with
    | ex -> MessageBox.Show($"Error loading dictionary: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
