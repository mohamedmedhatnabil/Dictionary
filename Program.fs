open System
open DictionaryManager
open GUI

[<EntryPoint>]
let main argv =
    let filePath = "dictionary.json"

    // تحميل القاموس من ملف عند بدء التشغيل
    printfn "Loading dictionary from file..."
    loadFromFile filePath

    // بدء واجهة المستخدم
    printfn "Starting GUI..."
    startGUI()

    // حفظ القاموس إلى الملف عند إغلاق التطبيق
    AppDomain.CurrentDomain.ProcessExit.Add(fun _ -> 
        printfn "Saving dictionary to file..."
        saveToFile filePath
    )

    0 // Exit code
