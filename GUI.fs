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
                saveToFile "C:\Phonee\Parallel repo\final version\F# Solution\Dictionary.json"
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
                saveToFile "C:\Phonee\Parallel repo\final version\F# Solution\Dictionary.json"
                txtWord.Clear()
                txtDefinition.Clear()
            else
                MessageBox.Show("Please select a word to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )
        // Load data from file at startup
        loadFromFile "C:\Phonee\Parallel repo\final version\F# Solution\Dictionary.json"
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
    let filePath = "C:\Phonee\Parallel repo\final version\F# Solution\Dictionary.json"
    loadFromFile filePath
    startGUI ()
    0 // Return exit code
