open System
open FSharpSound

let sampleRate = 48000

let samplesToSec sample = (float)sample / (float)sampleRate

let sinWave t = sin(2.0 * Math.PI * t)
let squareWave t = 1.0 - round(double t) * 2.0

let volume rate value = rate * value

let nl (noteName: NoteName) (octave: int) (sec: float) = sec |> Note(noteName, octave).perCycle |> squareWave |> volume 0.5

[<EntryPoint>]
let main argv = 
    [0..sampleRate * 2]
    |> List.map(fun x ->
        x
        |> samplesToSec
        |> (fun x -> (x |> (nl NoteName.c 4)) + (x |> (nl NoteName.e 4)) + (x |> (nl NoteName.g 4)))
        |> float32)
    |> SoundIOOutStreamUtil.playsound sampleRate
    0 // return an integer exit code
