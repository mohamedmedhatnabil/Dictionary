open System
open System.Windows.Forms
open System.Drawing

let createForm () =
    // Create a new Form
    let form = new Form(Text = "Dictionary Application", Width = 400, Height = 300)

    // Set the window state to maximized
    form.WindowState <- FormWindowState.Maximized

    // Create and configure the Word Label
    let wordLabel = new Label(Text = "Word:", Location = Point(10, 10), Width = 100)
    form.Controls.Add(wordLabel)

    // Create and configure the Meaning Label
    let meaningLabel = new Label(Text = "Meaning:", Location = Point(10, 40), Width = 100)
    form.Controls.Add(meaningLabel)

    // Create the TextBox for the Word
    let wordTextBox = new TextBox(Location = Point(120, 10), Width = 200)
    form.Controls.Add(wordTextBox)

    // Create the TextBox for the Meaning
    let meaningTextBox = new TextBox(Location = Point(120, 40), Width = 200)
    form.Controls.Add(meaningTextBox)

    // Create the DataGridView for displaying dictionary entries
    let dataGridView = new DataGridView(Location = Point(10, 80), Size = Size(360, 150))
    form.Controls.Add(dataGridView)

    // Create Add Button
    let addButton = new Button(Text = "Add", Location = Point(10, 240), Width = 100)
    form.Controls.Add(addButton)

    // Create Update Button
    let updateButton = new Button(Text = "Update", Location = Point(120, 240), Width = 100)
    form.Controls.Add(updateButton)

    // Create Delete Button
    let deleteButton = new Button(Text = "Delete", Location = Point(230, 240), Width = 100)
    form.Controls.Add(deleteButton)

    // Show the Form
    Application.Run(form)

// Call createForm to display the form
createForm()
