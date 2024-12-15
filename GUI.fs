open System
open System.Windows.Forms

/// وظيفة لبدء واجهة المستخدم
let startGUI () =
    // إنشاء الفورم الرئيسي
    let form = new Form(Text = "Digital Dictionary", Width = 800, Height = 600)

    // إنشاء عناصر واجهة المستخدم
    let lblWord = new Label(Text = "Word:", Top = 20, Left = 20, Width = 100)
    let txtWord = new TextBox(Top = 20, Left = 130, Width = 200)
    txtWord.Visible <- true  // جعل الـ TextBox مرئيًا

    let lblDefinition = new Label(Text = "Definition:", Top = 60, Left = 20, Width = 100)
    let txtDefinition = new TextBox(Top = 60, Left = 130, Width = 200)
    txtDefinition.Visible <- true  // جعل الـ TextBox مرئيًا

    let btnAdd = new Button(Text = "Add", Top = 100, Left = 20, Width = 100)
    btnAdd.Visible <- true  // جعل الـ Button مرئيًا

    let btnSearch = new Button(Text = "Search", Top = 100, Left = 130, Width = 100)
    btnSearch.Visible <- true  // جعل الـ Button مرئيًا
    let lstDictionary = new ListBox(Top = 150, Left = 20, Width = 750, Height = 350)
    lstDictionary.Visible <- true  // جعل الـ ListBox مرئيًا

    // الأحداث المرتبطة بالأزرار
    btnAdd.Click.Add(fun _ ->
        let word = txtWord.Text
        let definition = txtDefinition.Text
        if not (String.IsNullOrWhiteSpace(word)) && not (String.IsNullOrWhiteSpace(definition)) then
            lstDictionary.Items.Add($"{word}: {definition}")
            txtWord.Clear()
            txtDefinition.Clear()
        else
            MessageBox.Show("Please enter both a word and its definition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    )

    btnSearch.Click.Add(fun _ ->
        let keyword = txtWord.Text.ToLower()
        let mutable found = false
        for item in lstDictionary.Items do
            if item.ToString().ToLower().Contains(keyword) then
                found <- true
                MessageBox.Show($"Found: {item}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        if not found then
            MessageBox.Show("No matches found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
    )

    // إضافة العناصر إلى الفورم
    form.Controls.Add(lblWord)
    form.Controls.Add(txtWord)
    form.Controls.Add(lblDefinition)
    form.Controls.Add(txtDefinition)
    form.Controls.Add(btnAdd)
    form.Controls.Add(btnSearch)
    form.Controls.Add(lstDictionary)

    // تشغيل الفورم
    Application.Run(form)

/// الدالة الرئيسية
[<EntryPoint>]
let main argv =
    startGUI ()
    0
