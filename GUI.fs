module GUI

open System
open System.Windows.Forms
open DictionaryManager

// User interface
let startGUI () =
    try
        let form = new Form(Text = "Digital Dictionary", Width = 800, Height = 600, StartPosition = FormStartPosition.CenterScreen)

        let lblWord = new Label(Text = "Word:", Top = 20, Left = 20, Width = 100)
        let txtWord = new TextBox(Top = 20, Left = 130, Width = 200) // Word input
        let lblDefinition = new Label(Text = "Definition:", Top = 60, Left = 20, Width = 100)
        let txtDefinition = new TextBox(Top = 60, Left = 130, Width = 200) // Definition input
        let btnAdd = new Button(Text = "Add", Top = 100, Left = 20, Width = 100, Enabled = false) // Initially disabled
        let btnSearch = new Button(Text = "Search", Top = 100, Left = 130, Width = 100)
        let btnDelete = new Button(Text = "Delete", Top = 100, Left = 240, Width = 100)
        let btnUpdate = new Button(Text = "Update", Top = 100, Left = 350, Width = 100)
        let lstDictionary = new ListBox(Top = 150, Left = 20, Width = 750, Height = 350)

        let mutable originalWord = "" // Variable to store the selected word for updates

        // Function to refresh the dictionary list
        let refreshList () =
            lstDictionary.Items.Clear()
            dictionary |> Map.iter (fun key value -> lstDictionary.Items.Add($"{key}: {value}") |> ignore)

        // Handle when an item is selected in the ListBox
        lstDictionary.SelectedIndexChanged.Add(fun _ -> 
            if lstDictionary.SelectedItem <> null then
                let selectedWord = lstDictionary.SelectedItem.ToString().Split(':').[0].Trim()
                originalWord <- selectedWord // Store the original word for tracking changes
                txtWord.Text <- selectedWord
                let definition = dictionary.[selectedWord.ToLower()]
                txtDefinition.Text <- definition
        )

        // Add a word to the dictionary
        btnAdd.Click.Add(fun _ -> 
            let word = txtWord.Text
            let definition = txtDefinition.Text
            if not (String.IsNullOrWhiteSpace(word)) && not (String.IsNullOrWhiteSpace(definition)) then
                addWord word definition
                refreshList()
                saveToFile "dictionary.json"
                txtWord.Clear()
                txtDefinition.Clear()
            else
                MessageBox.Show("Please enter both a word and its definition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )

        // Search for a word in the dictionary
        btnSearch.Click.Add(fun _ -> 
            let keyword = txtWord.Text
            if not (String.IsNullOrWhiteSpace(keyword)) then
                let lowerKeyword = keyword.ToLower()
                let results = dictionary |> Map.filter (fun key _ -> key.Contains(lowerKeyword))
                lstDictionary.Items.Clear()
                results |> Map.iter (fun key value -> lstDictionary.Items.Add($"{key}: {value}") |> ignore)
            else
                MessageBox.Show("Please enter a word to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )

        // Delete the selected word from the dictionary
        btnDelete.Click.Add(fun _ -> 
            let word = txtWord.Text
            if not (String.IsNullOrWhiteSpace(word)) then
                deleteWord word
                refreshList()
                saveToFile "dictionary.json"
                txtWord.Clear()
                txtDefinition.Clear()
            else
                MessageBox.Show("Please select a word to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )

        // Update the selected word and/or its definition in the dictionary
        btnUpdate.Click.Add(fun _ -> 
            let newWord = txtWord.Text
            let newDefinition = txtDefinition.Text
            if not (String.IsNullOrWhiteSpace(originalWord)) && not (String.IsNullOrWhiteSpace(newWord)) && not (String.IsNullOrWhiteSpace(newDefinition)) then
                // Update the dictionary
                if originalWord <> newWord then
                    // If the word has changed, update the key in the dictionary directly
                    dictionary <- dictionary.Remove(originalWord.ToLower()).Add(newWord.ToLower(), newDefinition)
                else
                    // If only the definition is updated
                    updateWord originalWord newDefinition

                // Update the ListBox item directly
                let selectedIndex = lstDictionary.SelectedIndex
                if selectedIndex >= 0 then
                    lstDictionary.Items.[selectedIndex] <- $"{newWord}: {newDefinition}" // Update the selected item text

                // Save changes to file
                saveToFile "dictionary.json"

                // Clear input fields and reset originalWord
                txtWord.Clear()
                txtDefinition.Clear()
                originalWord <- "" 

                // Show success message
                MessageBox.Show("Word updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
            else
                MessageBox.Show("Please select a word and enter valid changes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )

        // Load data from file at startup
        loadFromFile "dictionary.json"
        refreshList()

        // Enable Add button only if both fields are populated
        txtWord.TextChanged.Add(fun _ -> 
            btnAdd.Enabled <- not (String.IsNullOrWhiteSpace(txtWord.Text) || String.IsNullOrWhiteSpace(txtDefinition.Text))
        )

        txtDefinition.TextChanged.Add(fun _ -> 
            btnAdd.Enabled <- not (String.IsNullOrWhiteSpace(txtWord.Text) || String.IsNullOrWhiteSpace(txtDefinition.Text))
        )

        form.Controls.AddRange([| lblWord; txtWord; lblDefinition; txtDefinition; btnAdd; btnSearch; btnDelete; btnUpdate; lstDictionary |])

        Application.Run(form)
    with
    | ex -> MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

[<EntryPoint>]
let main argv =
    // Load the dictionary from a file at startup
    let filePath = "dictionary.json"
    loadFromFile filePath
    startGUI ()
    0 // Return exit code
