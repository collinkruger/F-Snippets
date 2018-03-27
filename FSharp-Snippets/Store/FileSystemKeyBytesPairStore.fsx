open System
open System.IO

module FileSystemKeyBytesPairStoreModule =

    let private path rootDirectory key = Path.Combine(rootDirectory, key)


    let set rootDirectory key bytes = File.WriteAllBytes(path rootDirectory key, bytes)

    let get rootDirectory key =
        try
            Some (File.ReadAllBytes(path rootDirectory key))
        with
        | :? FileNotFoundException -> None

    let remove rootDirectory key = File.Delete(path rootDirectory key)

    let clear rootDirectory =
        let directoryInfo = DirectoryInfo(rootDirectory)
        directoryInfo.EnumerateDirectories() |> Seq.iter (fun di -> di.Delete(true))
        directoryInfo.EnumerateFiles() |> Seq.iter (fun fi -> fi.Delete())

type FileSystemKeyBytesPairStore(rootDirectory) =
    member this.set key bytes = FileSystemKeyBytesPairStoreModule.set rootDirectory key bytes
    member this.get key       = FileSystemKeyBytesPairStoreModule.get rootDirectory key
    member this.remove key    = FileSystemKeyBytesPairStoreModule.remove rootDirectory key
    member this.clear ()      = FileSystemKeyBytesPairStoreModule.clear rootDirectory