open System

let sampleRate = 48000
let pitch = 440.0

[<EntryPoint>]
let main argv = 
    [0..sampleRate * 2]
    |> List.map(fun x -> (float32)(Math.Sin(2.0 * Math.PI * pitch * (float)x / (float)sampleRate)))
    |> FSharpSound.SoundIOOutStreamUtil.playsound sampleRate
    0 // return an integer exit code
